using System;
using UnityEngine;

public class Gun : ItemHandler
{

    [SerializeField] [Range(0.1f, 1.5f)] private float fire_rate;
    [SerializeField] [Range(1, 10)] private int bullet_damage;
    [SerializeField] [Range(1, 10)] private int bullet_lifespan;
    [SerializeField] private float bullet_speed = 30.0f;
    [SerializeField] private bool is_hitscan;
    [SerializeField] private Transform bullet_origin;
    [SerializeField] private GameObject bullet;
    [SerializeField] private int magazine_size;

    private int current_clip_count;
    private Ray gun_shot;
    private RaycastHit hit_info;
    private GameObject bulletClone;
    private float timer;

    public event Action<int, int> OnFire = delegate { };

    private void Start()
    {
        bullet.GetComponent<Bullet>().Setup(bullet_lifespan, bullet_damage);

        el = EquipLocation.Right_hand;

        current_clip_count = 0;
    }

    void Update()
    {
        if (disabled) return;

        timer += Time.deltaTime;

        if (timer >= fire_rate && Input.GetMouseButtonDown(0))
        {
            timer = 0.0f;

            if (is_hitscan) HitScanFire();
            else ProjectileFire();

            current_clip_count--;

            OnFire(current_clip_count, magazine_size);

            if (current_clip_count <= 0) Reload();
        }
    }

    private void Reload()
    {

    }

    private void HitScanFire()
    {
        Debug.DrawRay(transform.position, transform.forward * 100, Color.red, 2.0f);

        gun_shot = new Ray(bullet_origin.position, bullet_origin.forward);

        if (Physics.Raycast(gun_shot, out hit_info, 100))
        {
            var enemy = hit_info.collider.GetComponent<Health>();

            if (enemy != null) enemy.TakeDamage(bullet_damage);
        }

    }

    private void ProjectileFire()
    {

        bulletClone = Instantiate(bullet);

        bulletClone.transform.position = bullet_origin.position;

        bulletClone.transform.rotation = bullet_origin.transform.rotation;

        bulletClone.GetComponent<Rigidbody>().velocity = bullet_origin.transform.forward * bullet_speed;

    }

}
