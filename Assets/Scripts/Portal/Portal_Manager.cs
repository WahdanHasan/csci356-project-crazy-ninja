using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal_Manager : MonoBehaviour
{
    static bool link_cameras = true;
    static bool setup_portals = true;

    private GameObject other_portal;
    private Portal_Camera portal_camera;
    private Portal_Interaction portal_interaction;
    private Transform portal_transform;
    private GameObject camera_helper_gameobject;
    private int camera_helper_translate_by;

    public void SetUp (GameObject other_portal, int value)
    {
        this.other_portal = other_portal;
        camera_helper_translate_by = value;

        portal_camera = GetComponent<Portal_Camera>();

        if (setup_portals)
        {
            setup_portals = false;
            SetPortalReferencesInScripts();
        }

        SetObjForCamera();

        if(link_cameras)
        {
            link_cameras = false;
            LinkCameras();
        }

    }

    private void SetPortalReferencesInScripts()
    {
        GetComponent<Portal_Camera>().Setup(other_portal);
        GetComponent<Portal_Interaction>().Setup(other_portal);

        other_portal.GetComponent<Portal_Camera>().Setup(this.gameObject);
        other_portal.GetComponent<Portal_Interaction>().Setup(this.gameObject);
    }

    private void SetObjForCamera()
    {
        camera_helper_gameobject = new GameObject();
    }

    private void LinkCameras()
    {
        Portal_Camera other_portal_camera = other_portal.GetComponent<Portal_Camera>();

        portal_camera.AssignPortalScreenTexture(other_portal_camera.GetCamera());
        other_portal_camera.AssignPortalScreenTexture(portal_camera.GetCamera());
    }

    public void UpdatePortal(RaycastHit rc_hit)
    {
        transform.position = rc_hit.point;
        camera_helper_gameobject.transform.position = rc_hit.point;

        transform.rotation = Quaternion.LookRotation(rc_hit.normal);
        camera_helper_gameobject.transform.rotation = Quaternion.LookRotation(rc_hit.normal * camera_helper_translate_by);

    }

    public GameObject GetCameraHelper()
    {
        return camera_helper_gameobject;
    }

}
