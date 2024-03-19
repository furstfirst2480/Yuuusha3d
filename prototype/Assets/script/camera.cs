using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3.0f;
    [SerializeField] private float switchDelay = 4.0f; // Time to switch targets
    private float switchTimer = 0.0f; // Timer to track when to switch targets

    private float rotationY;
    private float rotationX;

    [SerializeField] private Transform bossTarget;
    [SerializeField] private Transform playerTarget;
    private Transform currentTarget;

    [SerializeField] private float distanceFromTarget = 9f;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    [SerializeField] private float smoothTime = 0.2f;

    [SerializeField] private Vector2 rotationXMinMax = new Vector2(-20, 40);


    // Start is called before the first frame update
    void Start()
    {
        currentTarget = bossTarget; // Start with boss as target
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX += mouseY;

        // Apply clamping for x rotation 
        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        // Apply damping between rotation changes
        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        transform.localEulerAngles = currentRotation;
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            distanceFromTarget -= scrollInput * mouseSensitivity;
            // Clamp the distance to avoid negative values or getting too close to the target
            distanceFromTarget = Mathf.Clamp(distanceFromTarget, 2f, 9f); // Adjust min and max distance as needed
        }

        // Check if it's time to switch targets
        switchTimer += Time.deltaTime;
        if (switchTimer >= switchDelay)
        {
            currentTarget = playerTarget;
        }

        // Substract forward vector of the GameObject to point its forward vector to the target
        transform.position = currentTarget.position - transform.forward * distanceFromTarget;
    }
}
