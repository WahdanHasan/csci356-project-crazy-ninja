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

    public void SetPortalEnableStatus(bool status)
    {
        GetComponent<Portal_Camera>().portal_camera.enabled = true;
        GetComponent<Portal_Interaction>().portal_enabled = status;
    }

    public void UpdatePortal(RaycastHit rc_hit) /* Updates the portals location, rotation, normal, and the reference to its wall object (the wall its cast on) */
    {
        if (PortalClipping(rc_hit)) return;

        transform.position = rc_hit.point;
        camera_helper_gameobject.transform.position = rc_hit.point;

        transform.rotation = Quaternion.LookRotation(rc_hit.normal);
        camera_helper_gameobject.transform.rotation = Quaternion.LookRotation(rc_hit.normal * camera_helper_translate_by);

        wall_collider = rc_hit.collider;

        GetComponent<Portal_Interaction>().SetPortalNormal(rc_hit.normal);
    }

    private bool PortalClipping(RaycastHit rc_hit)
    {
        Ray ray_1 = new Ray(rc_hit.point + rc_hit.normal, Vector3.up);
        Ray ray_2 = new Ray(rc_hit.point + rc_hit.normal, Vector3.down);
        Ray ray_3 = new Ray(rc_hit.point + rc_hit.normal, Vector3.left);
        Ray ray_4 = new Ray(rc_hit.point + rc_hit.normal, Vector3.right);

        RaycastHit rc_hit_one;
        RaycastHit rc_hit_two;
        RaycastHit rc_hit_three;
        RaycastHit rc_hit_four;

        if (Physics.Raycast(ray_1, out rc_hit_one) && Physics.Raycast(ray_2, out rc_hit_two) && Physics.Raycast(ray_3, out rc_hit_three) && Physics.Raycast(ray_4, out rc_hit_four))
        {
            if(rc_hit_one.distance >= (transform.localScale.y/2) && rc_hit_two.distance >= (transform.localScale.y / 2))
            {
                if (rc_hit_three.distance >= (transform.localScale.x/2) && rc_hit_four.distance >= (transform.localScale.x / 2))
                {
                    return false;
                }
            }
        }


        return true;
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
