using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckWinner : MonoBehaviour


{
    public static CheckWinner instance;
    public Camera defaultCamera; // Set this in the Inspector
    public Camera winnerCamera;  // Set this in the Inspector
    public bool isWinner = false;
    public Transform target; // Reference to the player's transform
    public float smoothSpeed = 1.0f; // Adjust the smoothness of camera movement
    private void Awake()
    {
        instance = this;
    }
    public Transform playerRotation;

    // Start is called before the first frame update
    void Start()
    {
        defaultCamera.enabled = true;
        winnerCamera.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isWinner)
        {
            // Winner is determined; switch to the winner camera
            defaultCamera.enabled = false;
            winnerCamera.enabled = true;
        }

    }
    private void LateUpdate()
    {
        if (target != null && isWinner)
        {
            // Calculate the desired position for the camera
            Vector3 desiredPosition = new Vector3(target.position.x , target.position.y , target.position.z - 3f);

            // Smoothly move the camera towards the desired position
            Vector3 smoothedPosition = Vector3.Lerp(winnerCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            winnerCamera.transform.position = smoothedPosition;
            
            playerRotation.LookAt(new Vector3(playerRotation.position.x, playerRotation.position.y, winnerCamera.transform.position.z));
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && PlayerController.instance.groundedPlayer)
        {
            isWinner = true;
        }
    }

}
