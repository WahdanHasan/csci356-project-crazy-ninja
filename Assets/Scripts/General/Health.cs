using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int starting_health;

    private int current_health;

    void Start()
    {
        if (gameObject.tag == "Bullet") starting_health = 1;
        current_health = starting_health;
    }

    public void TakeDamage(int amount)
    {
        current_health -= amount;

        if (current_health <= 0)
            Die();
    }

    private void Die()
    {
        switch (gameObject.tag)
        {
            case "Player":
                gameObject.SetActive(false);
                GetComponent<Inventory_Manager>().OnDeath();
                break;
            case "Bullet":
                Destroy(gameObject);
                break;
        }

    }
}
