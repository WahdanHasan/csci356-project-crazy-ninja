using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float bullet_lifespan;
    private int bullet_damage;

    private Rigidbody rb;

    public void Setup(float bullet_lifespan, int bullet_damage)
    {
        this.bullet_lifespan = bullet_lifespan;
        this.bullet_damage = bullet_damage;
    }

    private void OnTriggerEnter(Collider entity)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);

        StartCoroutine(DestroyBullet());

        var entity_health = entity.GetComponent<Health>();

        if (entity_health == null) return;

        entity_health.TakeDamage(bullet_damage);
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bullet_lifespan);

        GetComponent<Health>().TakeDamage(1);
    }
   
    
}
