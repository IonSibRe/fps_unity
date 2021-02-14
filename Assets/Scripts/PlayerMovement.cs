using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Transform groundCheck;
    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    private Slide slide;

    public LayerMask groundMask;

    // Movement
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


    // Player Height
    public float originalHeight;
    public float reducedHeight = 1.25f;

    private float groundDistance = 0.4f;
    private float x, z;

    Vector3 moveDirection;

    public bool isGrounded;
    public bool isSprinting;
    public bool isCrouching;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCollider = GetComponent<CapsuleCollider>();
        slide = GetComponent<Slide>();

        originalHeight = playerCollider.height;

        stepRayUpper.transform.localPosition = new Vector3(stepRayUpper.transform.localPosition.x, stepHeight, stepRayUpper.transform.localPosition.z);
    }

    void Update()
    {
        UserInput();
        Jump();
        Sprint();
        //Crouch();
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

        if (slide.isSliding && Input.GetKey(KeyCode.S))
            return;

        // Walk
        if (!isSprinting)
            rb.AddForce(moveDirection * moveSpeed * Time.fixedDeltaTime);

        // Sprinting
        if (isSprinting && !slide.isSliding)
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
            isSprinting = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            isSprinting = false;
    }

    // Crouch
    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
            isCrouching = true;

        if (Input.GetKeyUp(KeyCode.C))
            isCrouching = false;
    }

    private void StepClimb()
    {
        Vector3 lowerRayPos = stepRayLower.transform.position;
        Vector3 upperRayPos = stepRayUpper.transform.position;

        Vector3 posChange = new Vector3(0f, -stepSmooth, 0f);

        if (Physics.Raycast(lowerRayPos, moveDirection, out RaycastHit lowerHit, rayLengthLower))
        {
            float dot = Vector3.Dot(lowerHit.normal, Vector3.up);

            if (!Physics.Raycast(upperRayPos, moveDirection, rayLengthUpper) && Mathf.Approximately(dot, 0))
            {
                rb.position -= posChange;
            }
        }
    }
}