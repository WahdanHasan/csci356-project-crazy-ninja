using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] [Range(0.1f, 1.5f)] private float fire_rate = 1.0f;
    [SerializeField] [Range(1, 10)] private int damage = 1;
    [SerializeField] private bool is_bot = false;
    [SerializeField] private bool is_hitscan = true;
    [SerializeField] private Transform bullet_origin;
    [SerializeField] private GameObject bullet;

    private Ray gun_shot;
    private RaycastHit hit_info;
    private GameObject bulletClone;
    private float bullet_speed = 30.0f;
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= fire_rate && Input.GetMouseButtonDown(0))
        {
            timer = 0.0f;

            if (is_hitscan) HitScanFire();
            else ProjectileFire();
        }
    }

    private void HitScanFire()
    {
        Debug.DrawRay(transform.position, transform.forward * 100, Color.red, 2.0f);

        gun_shot = new Ray(bullet_origin.position, bullet_origin.forward);

        if (Physics.Raycast(gun_shot, out hit_info, 100))
        {
            var enemy = hit_info.collider.GetComponent<Health>();

            if (enemy != null) enemy.TakeDamage(damage);
        }

    }

    private void ProjectileFire()
    {

        bulletClone = Instantiate(bullet);
        
        bulletClone.transform.position = bullet_origin.position;
        Vector3 rotation = bullet.transform.rotation.eulerAngles;

        bulletClone.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * bullet_speed;

    }
}
