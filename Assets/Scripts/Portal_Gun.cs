using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Gun : MonoBehaviour
{
    [SerializeField] private Transform ray_origin;
    [SerializeField] private GameObject portal_prefab;

    private Ray rc;
    private RaycastHit rc_hit_info;
    protected GameObject portal_one;
    protected GameObject portal_two;    

    private void OnEnable()
    {
        portal_one = Instantiate(portal_prefab);
        portal_two = Instantiate(portal_prefab);

        portal_two.GetComponent<Portal_Interaction>().SetOtherPortal(portal_one);
        portal_one.GetComponent<Portal_Interaction>().SetOtherPortal(portal_two);

        Vector3 duct_tape = new Vector3(0.0f, -100.0f, 0.0f); // To hide the portals when the game starts
        portal_one.transform.position = duct_tape;
        portal_two.transform.position = duct_tape;

    }

    private void OnDisable() // placeholder
    {
        Destroy(portal_one);
        Destroy(portal_two);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            FirePortal(portal_one, 0);
        else if (Input.GetMouseButtonDown(1))
            FirePortal(portal_two, 1);

    }

    private void FirePortal(GameObject portal, int trigger)
    {
        rc = new Ray(ray_origin.position, ray_origin.forward);
        
        if(Physics.Raycast(rc, out rc_hit_info))
        {
            if (rc_hit_info.transform.tag == "Wall")
            {
                portal.transform.position = rc_hit_info.point;

                if (trigger == 0 )portal.transform.rotation = Quaternion.LookRotation(rc_hit_info.normal * -1);
                else portal.transform.rotation = Quaternion.LookRotation(rc_hit_info.normal);
            }
        }
    }


}
