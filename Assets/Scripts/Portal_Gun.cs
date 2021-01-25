using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Gun : MonoBehaviour
{
    [SerializeField] private Transform ray_origin;
    [SerializeField] private GameObject portal_prefab;

    private Ray rc;
    private RaycastHit rc_hit_info;
    private GameObject primary_portal;
    private GameObject secondary_portal;
    private Portal_Interaction primary_portal_event;
    private Portal_Interaction secondary_portal_event;

    private void OnEnable()
    {
        primary_portal = Instantiate(portal_prefab);
        secondary_portal = Instantiate(portal_prefab);

        Vector3 duct_tape = new Vector3(0.0f, -100.0f, 0.0f); // To hide the portals when the game starts
        primary_portal.transform.position = duct_tape;
        secondary_portal.transform.position = duct_tape;
        //primary_portal.SetActive(false);
        //secondary_portal.SetActive(false);

        primary_portal_event = primary_portal.GetComponent<Portal_Interaction>();
        secondary_portal_event = secondary_portal.GetComponent<Portal_Interaction>();

        primary_portal_event.OnEnter += TeleportEntity;
        secondary_portal_event.OnEnter += TeleportEntity;
    }

    private void TeleportEntity(GameObject portal)
    {

    }

    private void OnDisable()
    {
        Destroy(primary_portal);
        Destroy(secondary_portal);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            FirePortal(primary_portal);
        else if (Input.GetMouseButtonDown(1))
            FirePortal(secondary_portal);

    }

    private void FirePortal(GameObject portal)
    {
        rc = new Ray(ray_origin.position, ray_origin.forward);
        
        if(Physics.Raycast(rc, out rc_hit_info))
        {
            if (rc_hit_info.transform.tag == "Wall")
            {
                portal.transform.position = rc_hit_info.point;

                portal.transform.rotation = Quaternion.LookRotation(rc_hit_info.normal);
            }
        }
    }


}
