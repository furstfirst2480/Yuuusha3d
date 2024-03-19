using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ThePlayer : MonoBehaviour
{
    

    private CharacterController controller;
    public float playerSpeed;
    [SerializeField] private Camera followCamera;
    public AudioSource source;
    public AudioClip clip;
    [SerializeField] private float rotationSpeed = 10f;

    public Animator animator;

    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 0.5f;

    public bool running = false;
    public bool rollingcd = true;
    public bool playerded = false;
    public bool isAttacking ;

    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float AttackCost;
    public float RunCost;

    public float ChargeRate;
    private Coroutine recharge;
    
    
    public Image HPBar;
    public float playerHp = 100;
    public float MaxHP = 100;

    public Image MPBar;
    public float playerMP = 100;
    public float MaxMP = 100;
    public static ThePlayer instance;
    public bool paralyze;
    public bool cantmove;
    public int currentItemNumber = 1;
    public int maxItemNumber = 3;
    public bool invincible;
    public bool playerdead = false;
    public GameObject InvincibleAura;
    
    private void Awake()
    {
        instance = this;
    }




    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        running = false; rollingcd = true;

      

    }

    // Update is called once per frame
    void Update()
    {
        MPBar.fillAmount = playerMP / MaxMP;
        HPBar.fillAmount = playerHp / MaxHP;
        StaminaBar.fillAmount = Stamina / MaxStamina;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");

        }
        if (invincible ==true )
        {
            InvincibleAura.SetActive(true);
        }
        else
        {
            InvincibleAura.SetActive(false);
        }
        {
            if (playerHp == 0 && playerdead == false)
            {
                playerdead = true;
                StartCoroutine(YouLose());
                animator.SetBool("death",true);
                animator.SetTrigger("dead");
            }
          
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                currentItemNumber++;


                if (currentItemNumber > maxItemNumber)
                {
                    currentItemNumber = 1;
                    
                }

            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isAttacking && !running && animator.GetFloat("Speed") <= 0.09f && paralyze == false && groundedPlayer && cantmove == false  )
            {
                if(playerHp != MaxHP && currentItemNumber == 1)
                {
                animator.SetTrigger("Drinking");
                cantmove = true; 
                StartCoroutine(abletomove());
                playerHp += 10;
               

                }

                if (playerMP != MaxMP && currentItemNumber == 2)
                {
                    animator.SetTrigger("Drinking");
                    cantmove = true;
                    StartCoroutine(abletomove());
                    playerMP += 10;
                   
                }
                if(playerMP >= 20 && currentItemNumber == 3) 
                {
                    animator.SetTrigger("Skill");
                    cantmove = true;
                    StartCoroutine(abletomove());
                    playerMP -= 20;
                    invincible = true;
                    StartCoroutine(stopinvincible());
                    
                }
            }

            if (Input.GetButtonDown("Jump") && rollingcd == true && Stamina >= 20 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f && cantmove == false)
            {
                animator.SetTrigger("Rolling");
           
            
              
         
                Stamina -= 20;




            }
          
            if ( !isAttacking && playerHp != 0 && paralyze == false && cantmove == false)  // Check if the player is not attacking
            {
                Movement();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && !running && animator.GetFloat("Speed") <= 0.09f && paralyze == false && groundedPlayer && cantmove == false)
            {
                animator.SetTrigger("Attack");
                isAttacking = true;
           
                StartCoroutine(StopMovingWhenATK());
            }



        }




    }
   
    void Movement()
    {
     

        groundedPlayer = controller.isGrounded;

        if (controller.isGrounded && playerVelocity.y < -2)
        {
            playerVelocity.y = -1f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0)
        * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput.normalized;

        controller.Move(movementDirection * playerSpeed * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {

            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);

        }
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.F) && groundedPlayer)
        {

            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        animator.SetFloat("Speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", controller.isGrounded);

        if (Input.GetKey(KeyCode.LeftShift) && groundedPlayer && Stamina > 0  )
        {
            running = true;
            animator.SetBool("Running", true);
            
               
            playerSpeed = 14f;

            if (movementDirection.x != 0 || movementDirection.z != 0)
            {
                Stamina -= RunCost * Time.deltaTime;
            }
            if (Stamina < 0)
            { Stamina = 0; }
        
            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }

        else
        {
          playerSpeed = 7f;
                running = false;
                animator.SetBool("Running", false);
        
            if (Stamina == 0)
            {
                playerSpeed = 1.5f;
            }
        }
    }
    private IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(2f);

        while (Stamina < MaxStamina)
        {
            Stamina += ChargeRate / 10f;
            if (Stamina > MaxStamina) Stamina = MaxStamina;
            StaminaBar.fillAmount = Stamina / MaxStamina;
            yield return new WaitForSeconds(.5f);
        }
    }
   

    public void TakeDamage(float damage)
    {
        if (!invincible) { 
        playerHp -= damage; 
        animator.SetTrigger("gethit");
        paralyze = true;
        StartCoroutine(paralyzestun());
        }
        source.PlayOneShot(clip);

    }

    public void Manadown(float MPdown)
    {
        playerMP -= MPdown;
       
    }
    private IEnumerator StopMovingWhenATK()
    {
        yield return new WaitForSeconds(0.8f);
        isAttacking = false;
    }
    private IEnumerator paralyzestun()
    {
        yield return new WaitForSeconds(5f);

        { paralyze = false; }
    }
    private IEnumerator abletomove()
    {
        yield return new WaitForSeconds(3f);

        { cantmove = false; }
    }
    private IEnumerator stopinvincible()
    {
        if (cantmove == true)
        {
            yield return new WaitForSeconds(1.5f);
            invincible = false;
        }
     

        
    }
    private IEnumerator YouLose()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("GameOver");
    }
}


