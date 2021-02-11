using UnityEngine;

public class Slide : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider playerCollider;

    // Player Height
    private float originalHeight;
    public float reducedHeight = 1.25f;

    // Slide
    public float slideSpeed = 10f;

    private float slideTime = 2.0f;
    private float currentSlideTime;

    public bool isSliding;

    void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        originalHeight = playerCollider.height;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && Input.GetKey(KeyCode.W))
            StartSliding();

        if (Input.GetKeyUp(KeyCode.LeftControl) || currentSlideTime > slideTime)
            StopSliding();

        if (isSliding)
            UpdateSlideTime();
    }

    private void StartSliding()
    {
        isSliding = true;

        playerCollider.height = reducedHeight;
        rb.AddForce(transform.forward * slideSpeed, ForceMode.Impulse);
    }

    private void StopSliding()
    {
        isSliding = false;
        currentSlideTime = 0f;

        playerCollider.height = originalHeight;
    }

    private void UpdateSlideTime()
    {
        currentSlideTime += Time.deltaTime;
    }
}