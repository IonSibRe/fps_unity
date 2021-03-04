using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static int health = 100;

    void Update()
    {
        if (health <= 0)
        {
            Debug.Log("Finish");
        }
    }
    private void OnCollisionEnter (Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && health > 0)
        {
            Debug.Log("Yo");
            health -= 10;
        }
    }
}
