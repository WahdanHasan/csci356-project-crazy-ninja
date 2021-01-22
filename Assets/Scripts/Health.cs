using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int starting_health;

    private int current_health;

    void OnEnable()
    {
        current_health = starting_health;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        current_health -= amount;

        if (current_health >= 0)
            Die();
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
