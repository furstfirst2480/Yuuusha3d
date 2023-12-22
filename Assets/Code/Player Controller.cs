using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //playerMove
    private CharacterController controller;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Camera followCamera;
    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;
    [SerializeField] private float rotationSpeed = 10f;
    public Animator animator;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 3f;
    public static PlayerController instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame

    private void Update()
    {
        switch (CheckWinner.instance.isWinner) // if iswinner true set animation if false able to move
        {
            case true:
                animator.SetBool("Victory", CheckWinner.instance.isWinner);
                break;
            case false:
                Movement();
                break;
        }
    }
    void Movement()
    {
      

        groundedPlayer = controller.isGrounded;
        if (controller.isGrounded && playerVelocity.y < -2f)
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


            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.5f * gravityValue);
            }


            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", controller.isGrounded);
      if (Input.GetButtonDown("Jump") && groundedPlayer)
        {

            playerVelocity.y += Mathf.Sqrt(jumpHeight * -0.1f * gravityValue);
            animator.SetTrigger("Jumping");
        }


    }
  
}
