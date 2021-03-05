using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static int health = 100;

    void Update()
    {
        if (health <= 0)
        {
            // Reset Health and Score
            health = 100;
            HUD.score = 0;

            // Game Over
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("GameOverMenu");
        }
    }
    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && health > 0)
        {
            // Destroy Bullet
            Destroy(collision.gameObject);

            health -= Random.Range(5, 16);
        }
    }
}
