using UnityEngine;

public class Portal_Gun : MonoBehaviour
{
    [SerializeField] private Transform ray_origin;
    [SerializeField] private GameObject portal_prefab;

    private Ray rc;
    private RaycastHit rc_hit_info;
    protected GameObject portal_one;
    protected GameObject portal_two;

    private LineRenderer lr;

    private void Awake() /* Instantiates both portals and commences their setup, sets the item's location on the player */
    {
        portal_one = Instantiate(portal_prefab);
        portal_two = Instantiate(portal_prefab);

        portal_one.GetComponent<Portal_Manager>().SetUp(portal_two, 1);
        portal_two.GetComponent<Portal_Manager>().SetUp(portal_one, -1);


        Vector3 portal_spawn_pos = new Vector3(0.0f, -100.0f, 0.0f); // To hide the portals when the game starts
        portal_one.transform.position = portal_spawn_pos;
        portal_two.transform.position = portal_spawn_pos;

        lr = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FirePortal(portal_one);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            FirePortal(portal_two);
        }

    }

    private void FirePortal(GameObject portal) /* Allows the casting of the portal if it is against an object that is tagged 'Wall' */
    {
        rc = new Ray(ray_origin.position, ray_origin.forward);

        //lr.SetPosition(0, ray_origin.position);

        if (Physics.Raycast(rc, out rc_hit_info))
        {
            if (rc_hit_info.transform.tag == "Wall")
            {
                portal.GetComponent<Portal_Manager>().UpdatePortal(rc_hit_info);
            }
            //lr.SetPosition(1, rc_hit_info.point);
        }
        else
        {
            //lr.SetPosition(1, transform.forward * 5000);
        }
    }

}
