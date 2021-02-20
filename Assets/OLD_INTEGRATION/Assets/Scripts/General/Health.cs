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
        if (gameObject.tag == "Bullet") starting_health = 1;
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
                //gameObject.SetActive(false);
                isDeadOrAlive(true);
                PlayerMovement.Instance.KillPlayer();
                //GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Inventory_Manager>().OnDeath();
                break;
            case "Bullet":
                Destroy(gameObject);
                break;
        }

    }
}
