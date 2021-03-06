using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private HUD hud;

    public float health;

    void Start()
    {
        hud = GameObject.Find("Player").GetComponent<HUD>();
        health = Random.Range(100f, 151f);
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
