using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Slide slide;
    private Rigidbody rb;
    public Transform groundCheck;

    public LayerMask groundMask;

    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float sprintSpeed = 20.0f;
    [SerializeField] private float jumpForce = 5.0f;

    // Stepping up Stairs
    [SerializeField] private GameObject stepRayUpper;
    [SerializeField] private GameObject stepRayLower;
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepSmooth = 0.1f;
    [SerializeField] private float rayLengthLower = 0.5f;
    [SerializeField] private float rayLengthUpper = 0.75f;


    private float groundDistance = 0.4f;
    private float x, z;

    Vector3 moveDirection;

    public bool isGrounded;
    public bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slide = GetComponent<Slide>();

        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
    }

    void Update()
    {
        UserInput();
        Jump();
        Sprint();
    }

    void FixedUpdate()
    {
        Movement();

        if (x != 0 || z != 0)
            StepClimb();
    }

    // Input
    private void UserInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
    }

    // Movement
    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        moveDirection = transform.right * x + transform.forward * z;
        moveDirection.Normalize();

        // Walk
        if (!isSprinting)
            rb.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime);

        // Sprinting
        if (isSprinting)
            rb.AddForce(moveDirection * sprintSpeed * Time.fixedDeltaTime);
    }

    // Jump
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }

    // Sprint
    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
        {
            isSprinting = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }
    }

    private void StepClimb()
    {
        Vector3 lowerRayPos = stepRayLower.transform.position;
        Vector3 upperRayPos = stepRayUpper.transform.position;

        Vector3 posChange = new Vector3(0f, -stepSmooth, 0f);

        if (Physics.Raycast(lowerRayPos, moveDirection, rayLengthLower))
        {
            if (!Physics.Raycast(upperRayPos, moveDirection, rayLengthUpper))
            {
                rb.position -= posChange;
            }
        }
    }
}