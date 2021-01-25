using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField][Range(0.0f, 1.0f)] private float bullet_lifespan;

    private Rigidbody rb;

    private void Start()
    {
        bullet_lifespan = 1.0f;   
    }

    private void OnTriggerEnter(Collider entity)
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);

        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(bullet_lifespan);

        Destroy(gameObject);
    }
   
    
}
