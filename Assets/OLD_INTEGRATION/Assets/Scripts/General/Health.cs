using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int starting_health;

    private int current_health;
    private bool isActive = true;
    public event Action<int, int> UpdateHealth = delegate {};

    void Start()
    {
        if (gameObject.tag == "Bullet") starting_health = 1;
        current_health = starting_health;
        isActive = true;
        TakeDamage(0);
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    private void Update()
    {
        
    }

    public void TakeDamage(int amount)
    {
        current_health -= amount;

        UpdateHealth(current_health, starting_health);

        if (current_health <= 0)
            Die();
    }

    private void Die()
    {
        switch (gameObject.tag)
        {
            case "Player":
                gameObject.SetActive(false);
                //GetComponent<Inventory_Manager>().OnDeath();
                break;
            case "Bullet":
                Destroy(gameObject);
                break;
        }

    }
}
