using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Slide slide;
    private Rigidbody rb;
    public Transform groundCheck;

    public LayerMask groundMask;

    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float springSpeed = 20.0f;
    [SerializeField] private float jumpForce = 5.0f;

    private float groundDistance = 0.4f;
    private float x, z;

    private bool isGrounded;
    private bool isSprinting;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        slide = GetComponent<Slide>();
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
    }

    // Input
    private void UserInput()
    {
        x = Input.GetAxis("Horizontal") * moveSpeed;
        z = Input.GetAxis("Vertical") * moveSpeed;
    }

    // Movement
    private void Movement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        Vector3 movePos = transform.right * x + transform.forward * z;
        Vector3 newMovePos = new Vector3(movePos.x, 0, movePos.z).normalized;

        // Walk
        if (!slide.isSliding && !isSprinting)
            rb.MovePosition(rb.position + newMovePos * moveSpeed * Time.fixedDeltaTime);

        // Sprinting
        if (!slide.isSliding && isSprinting)
            rb.MovePosition(rb.position + newMovePos * springSpeed * Time.fixedDeltaTime);
    }

    // Jump
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    }

    // Sprint
    private void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && isGrounded)
            isSprinting = true;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            isSprinting = false;
    }
}