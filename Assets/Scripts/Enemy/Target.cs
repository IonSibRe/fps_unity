using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private HUD hud;

    public float health = 100.0f;

    void Start()
    {
        hud = GameObject.Find("Player").GetComponent<HUD>();
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
            hud.UpdateScore();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
