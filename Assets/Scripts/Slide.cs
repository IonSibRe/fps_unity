using UnityEngine;

public class Slide : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider playerCollider;
    private PlayerMovement playerController;

    // Slide
    public float slideSpeed = 10f;

    // Raycasts
    private float rayLength;
    private float sphereCastRadius;

    // Timers
    private float slideTime = 2.0f;
    private float currentSlideTime;

    // Checks
    public bool isSliding;
    public bool ceilingBlock;
    
    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        playerController = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();

        playerController.originalHeight = playerCollider.height;
        rayLength = playerController.originalHeight - playerController.reducedHeight;
        sphereCastRadius = playerCollider.radius;
    }

    void Update()
    {
        Vector3 rayOrigin = new Vector3(playerCollider.transform.position.x, playerCollider.bounds.max.y, playerCollider.transform.position.z);
        ceilingBlock = Physics.SphereCast(rayOrigin, sphereCastRadius, Vector3.up, out _, rayLength);

        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.W))
            StartSliding();

        if ((!Input.GetKey(KeyCode.LeftControl) || currentSlideTime > slideTime) && !ceilingBlock)
            StopSliding();

        if (isSliding)
            UpdateSlideTime();
    }

    private void StartSliding()
    {
        isSliding = true;

        playerCollider.height = playerController.reducedHeight;
        rb.AddForce(transform.forward * slideSpeed, ForceMode.Impulse);
    }

    private void StopSliding()
    {
        isSliding = false;
        currentSlideTime = 0f;

        playerCollider.height = playerController.originalHeight;
    }

    private void UpdateSlideTime()
    {
        currentSlideTime += Time.deltaTime;
    }
}