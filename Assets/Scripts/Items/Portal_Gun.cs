using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Gun : Item
{
    [SerializeField] private Transform ray_origin;
    [SerializeField] private GameObject portal_prefab;

    private Ray rc;
    private RaycastHit rc_hit_info;
    protected GameObject portal_one;
    protected GameObject portal_two;    

    private void Start()
    {
        portal_one = Instantiate(portal_prefab);
        portal_two = Instantiate(portal_prefab);

        portal_one.GetComponent<Portal_Manager>().SetUp(portal_two, 1);
        portal_two.GetComponent<Portal_Manager>().SetUp(portal_one, -1);

        Vector3 duct_tape = new Vector3(0.0f, -100.0f, 0.0f); // To hide the portals when the game starts
        portal_one.transform.position = duct_tape;
        portal_two.transform.position = duct_tape;

        el = EquipLocation.Right_hand;

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            FirePortal(portal_one);
        else if (Input.GetMouseButtonDown(1))
            FirePortal(portal_two);

    }

    private void FirePortal(GameObject portal)
    {
        rc = new Ray(ray_origin.position, ray_origin.forward);

        if (Physics.Raycast(rc, out rc_hit_info))
            if (rc_hit_info.transform.tag == "Wall")
                portal.GetComponent<Portal_Manager>().UpdatePortal(rc_hit_info);


    }


}
