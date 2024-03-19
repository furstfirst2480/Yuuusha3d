using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private CharacterController controller;
    static public float playerSpeed = 7f;
    [SerializeField] private Camera followCamera;
    
    [SerializeField] private float rotationSpeed = 10f;
    
    public Animator animator;

    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;
   
    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 0.5f;

    public bool running = false;
    public bool rolling = false;

    public Image StaminaBar;
    public float Stamina, MaxStamina;
    public float AttackCost;
    public float RunCost;
    
    public float ChargeRate;
    private Coroutine recharge;

    
 



    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        running = false; rolling = false;
    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        
      
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

        if (Input.GetKey(KeyCode.LeftShift) && groundedPlayer && Stamina > 0 && playerSpeed != 0)
        {
            running = true;
            animator.SetBool("Running", true);
            playerSpeed = 10f;

            if (movementDirection.x != 0 || movementDirection.z != 0)
            {
                Stamina -= RunCost * Time.deltaTime;
            }
            if (Stamina < 0)
            { Stamina = 0; }
            StaminaBar.fillAmount = Stamina / MaxStamina;
            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }
        
        else
        {
            running = false;
            animator.SetBool("Running", false);
            playerSpeed = 7f;

            if (Stamina == 0)
            {
                playerSpeed = 1.5f;
            }
        }
       
        
        




        if (Input.GetButtonDown("Jump"))
        {
            animator.SetBool("Rolling", true);
           

        }
        else if (Input.GetButtonDown("Jump") == false)
        {
            animator.SetBool("Rolling", false);
           

        }
        
       
    }
    private IEnumerator RechargeStamina(){
       yield return new WaitForSeconds(5f);
      
        while(Stamina < MaxStamina)
        {
            Stamina += ChargeRate / 10f;
            if(Stamina > MaxStamina) Stamina = MaxStamina;
            StaminaBar.fillAmount = Stamina / MaxStamina;
            yield return new WaitForSeconds(.5f);
        }
    }
}


