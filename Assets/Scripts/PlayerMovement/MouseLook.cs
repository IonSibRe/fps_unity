using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform playerBody;

    public static float mouseSens = 350f;
    Vector3 offset;

    // Looking up or down = rotating around x axis which is why the variable is called xRotation
    private float xRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - playerBody.transform.position;

        // Hide & Lock Cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerBody.transform.position + offset;

        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        // Rotate on the y axis (Vector3.up = Vector3(0, 1, 0))
        playerBody.Rotate(Vector3.up * mouseX);
        transform.Rotate(Vector3.up * mouseX);

        // Decreasing so the rotation isn't flipped.
        xRotation -= mouseY;

        // Limit the rotation to -90 - 90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Set the rotation
        transform.rotation = Quaternion.Euler(xRotation, playerBody.rotation.eulerAngles.y, 0);
    }
}
