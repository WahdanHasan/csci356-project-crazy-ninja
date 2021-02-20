using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int starting_health;

    private int current_health;
    private bool isActive = true;
    public event Action<int, int> UpdateHealth = delegate {};
    public event Action<bool> isDeadOrAlive = delegate {};

    void Start()
    {
        current_health = starting_health;
        isActive = true;
    }

    public int GetCurrentHealth()
    {
        return current_health;
    }

    public int GetTotalHealth()
    {
        return starting_health;
    }

    public bool GetIsActive()
    {
        return isActive;
    }

    public void TakeDamage(int amount)
    {
        if (current_health <= 0) return;

        current_health -= amount;

        UpdateHealth(current_health, starting_health);

        if (current_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        switch (gameObject.tag)
        {
            case "Player":
                isDeadOrAlive(true);
                PlayerMovement.Instance.KillPlayer();
                GetComponent<Inventory_Manager>().OnDeath();
                break;
        }

    }
}
