using UnityEngine;

public class Gun : MonoBehaviour
{

    [SerializeField] [Range(0.1f, 1.5f)] private float fire_rate = 1.0f;
    [SerializeField] [Range(1, 10)] private int bullet_damage = 1;
    [SerializeField] [Range(1, 10)] private int bullet_lifespan = 1;
    [SerializeField] private bool is_bot = false;
    [SerializeField] private bool is_hitscan = true;
    [SerializeField] private Transform bullet_origin;
    [SerializeField] private GameObject bullet;

    private Ray gun_shot;
    private RaycastHit hit_info;
    private GameObject bulletClone;
    private float bullet_speed = 30.0f;
    private float timer;

    private void Start()
    {
        bullet.GetComponent<Bullet>().Setup(bullet_lifespan, bullet_damage);
    }

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
