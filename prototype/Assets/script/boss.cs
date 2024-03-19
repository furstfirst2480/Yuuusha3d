using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class boss : MonoBehaviour
{
    public AudioSource source ;
    public AudioClip clip;
    public NavMeshAgent agent;
    public Animator animator;
    public float rotationSpeed = 5f;
    public Image BossHPBar;
    public float BossHP = 100;
    public float MaxBossHP = 100;
    public bool bossstopmove = false;
    public Image BossArmorBar;
    public float BossArmor = 100;
    public float MaxBossArmor = 100;
    bool hasBroken = false;
    public bool bossintro = true;
    public bool bossdead = false;
    // Tracking boss state
    public enum AIState
    {
        isDead, isSeekPlayer, isAttacking1 , isAttacking2 , Stunning , isAttacking3
    }
    public AIState state;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(BossIntro());
    }

    // Update is called once per frame
    void Update()
    {
        if(bossintro == false)
        { 
        BossHPBar.fillAmount = BossHP / MaxBossHP;
        BossArmorBar.fillAmount = BossArmor / MaxBossArmor;
        // Calculate distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, ThePlayer.instance.transform.position);

        // Rotate towards the player
        RotateTowards(ThePlayer.instance.transform.position);
            if (BossHP <= 0 && bossdead == false )
        {
            
            state= AIState.isDead;
            StartCoroutine(YouWin());
            }
        if( BossArmor <= 0 )
            {
                BossArmor = 0;
            }
        if(BossArmor == 0 &&!hasBroken && BossHP != 0)
        {
            animator.SetTrigger("break");
            hasBroken = true;
            StartCoroutine(BossStun());
            state = AIState.Stunning;

        }
        // Update boss state based on distance to player
        if (distanceToPlayer >= 4f  && BossArmor != 0 && BossHP != 0 && !animator.GetBool("Attack1") && !animator.GetBool("Attack2")  )
        {
            state = AIState.isSeekPlayer;
        }
        else if (distanceToPlayer <= 4f && !animator.GetBool("Attack1") && !animator.GetBool("Attack2") && !animator.GetBool("Attack3") && BossHP != 0)
        {
      

            // If the random value is less than 0.5, choose attacking move set 1, else choose move set 2
            float randomValue = Random.value; // Assuming Random.value gives a random value between 0 and 1

            if (randomValue < 0.33f) // Adjust the range for isAttacking1
            {
                state = AIState.isAttacking1;
            }
            else if (randomValue < 0.66f) // Adjust the range for isAttacking2
            {
                state = AIState.isAttacking2;
            }
            else // The rest of the range goes to isAttacking3
            {
                state = AIState.isAttacking3;
            }

        }


            switch (state)
            {
                case AIState.isDead:
                    agent.speed = 0;
                    
                    if(bossdead == false)
                    {
                        animator.SetTrigger("dead");
                        bossdead = true;
                    }
                   
                    animator.SetBool("Attack1", false);
                    animator.SetBool("Run", false);
                    animator.SetBool("Attack2", false);
                    break;

                case AIState.isSeekPlayer:
                    // Enable NavMeshAgent and set destination to the player
                    agent.SetDestination(ThePlayer.instance.transform.position);
                
                        agent.speed = 5f;
                   
                    // Set animation for running
                    animator.SetBool("Attack1", false);
                    animator.SetBool("Run", true);
                    animator.SetBool("Attack2", false);
                    animator.SetBool("Attack3", false);
                    break;

                case AIState.isAttacking1:
                    agent.speed = 0;
                    // Set animation for attack
                    animator.SetBool("Run", false);
                    animator.SetBool("Attack1", true);
                    animator.SetBool("Attack2", false);
                    animator.SetBool("Attack3", false);
                
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                    {
                        animator.SetBool("Attack1", false);
                      
                    }
                    break;

                case AIState.isAttacking2:
                    agent.speed = 0;
                    // Set animation for attack
                    animator.SetBool("Run", false);
                    animator.SetBool("Attack1", false);
                    animator.SetBool("Attack2", true);
                    animator.SetBool("Attack3", false);
                  
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                    {
                        animator.SetBool("Attack2", false);
                       
                    }
                    break;

                case AIState.isAttacking3:
              
                    animator.SetBool("Run", false);
                    animator.SetBool("Attack1", false);
                    animator.SetBool("Attack2", false);
                    animator.SetBool("Attack3", true);
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
                    {
                        animator.SetBool("Attack3", false);
                      
                    }
                    break;

                case AIState.Stunning:
                    agent.speed = 0;
                    animator.SetBool("Run", false);
                    animator.SetBool("Attack1", false);
                    animator.SetBool("Attack2", false);
                    animator.SetBool("Attack3", false);
                    break;
            }


        }
    }
    

    // Method to rotate towards a target position
    void RotateTowards(Vector3 targetPosition)
    {
        // Calculate the direction from the boss to the player
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate the rotation needed to look at the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }
  
    public void TakeDamage(float damage)
    {
        BossHP -= damage/2;
        BossArmor -= damage ;
        source.PlayOneShot(clip);
    }
   
    private IEnumerator BossStun()
    {
        yield return new WaitForSeconds(8f);
        BossArmor = 100;
        hasBroken = false;
    }
    private IEnumerator BossIntro()
    {
        yield return new WaitForSeconds(4f);
        {
        bossintro = false;
        }
    }
    private IEnumerator YouWin()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("Win");
    }
    
}
