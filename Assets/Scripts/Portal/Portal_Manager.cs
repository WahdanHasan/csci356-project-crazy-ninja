using UnityEngine;


public class Portal_Manager : MonoBehaviour
{
    static bool link_cameras = true;
    static bool setup_portals = true;

    private Collider wall_collider;
    private GameObject other_portal;
    private Portal_Camera portal_camera;
    private GameObject camera_helper_gameobject;
    private int camera_helper_translate_by;

    public void SetUp (GameObject other_portal, int value) /* Sets up the portals, only one of the class objects manages linking to its partner class */
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

    private void SetPortalReferencesInScripts() /* Assigns references for the portals to each other */
    {
        GetComponent<Portal_Camera>().Setup(other_portal);
        GetComponent<Portal_Interaction>().Setup(other_portal);

        other_portal.GetComponent<Portal_Camera>().Setup(this.gameObject);
        other_portal.GetComponent<Portal_Interaction>().Setup(this.gameObject);
    }

    private void SetObjForCamera() /* Instantiates a helper gameobject for the camera, the camera utilizes the helpers transform to determine its position */
    {
        camera_helper_gameobject = new GameObject();
    }

    private void LinkCameras() /* Links the render textures on the portal screens to their respective cameras */
    {
        Portal_Camera other_portal_camera = other_portal.GetComponent<Portal_Camera>();

        portal_camera.AssignPortalScreenTexture(other_portal_camera.GetCamera());
        other_portal_camera.AssignPortalScreenTexture(portal_camera.GetCamera());
    }

    public void UpdatePortal(RaycastHit rc_hit) /* Updates the portals location, rotation, normal, and the reference to its wall object (the wall its cast on) */
    {
        transform.position = rc_hit.point;
        camera_helper_gameobject.transform.position = rc_hit.point;

        transform.rotation = Quaternion.LookRotation(rc_hit.normal);
        camera_helper_gameobject.transform.rotation = Quaternion.LookRotation(rc_hit.normal * camera_helper_translate_by);

        wall_collider = rc_hit.collider;

        GetComponent<Portal_Interaction>().SetPortalNormal(rc_hit.normal);
    }

    public GameObject GetCameraHelper()
    {
        return camera_helper_gameobject;
    }

    public Collider GetWallCollider()
    {
        return wall_collider;
    }

}