using UnityEngine;

public class Portal_Interaction : MonoBehaviour
{
    public MeshRenderer screen;
    Portal_Manager pm;
    Portal_Manager other_pm;
    private GameObject other_portal;
    public bool portal_enabled = false;
    Matrix4x4 voyager_transform_new;
    bool teleportable = true;
    Camera player_cam;
    float half_height;
    float half_width;
    float distance_to_clip_plane_corner;
    float screen_width;
    public bool same_wall = false;
    private bool teleported = false;

    public void Setup(GameObject other_portal)
    {
        this.other_portal = other_portal;
        pm = GetComponent<Portal_Manager>();
        other_pm = other_portal.GetComponent<Portal_Manager>();
    }

    private void OnTriggerEnter(Collider entity)
    {
        if (!portal_enabled || entity.tag == "IgnorePortal") return;

        if (IsBehindPortal(entity)) teleportable = false;

        Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), true);

        if (pm.GetWallCollider() == other_pm.GetWallCollider())
        {
            same_wall = true;
        }
        else
        {
            same_wall = false;
        }
    }

    private void OnTriggerStay(Collider entity)
    {
        if (!portal_enabled || entity.tag == "IgnorePortal") return;

        if (!IsBehindPortal(entity)) teleportable = true;

        if (IsBehindPortal(entity) && teleportable)
        {
            teleported = true;
            Teleport(entity.gameObject);
        }
    }

    private void OnTriggerExit(Collider entity)
    {
        if (!portal_enabled || entity.tag == "IgnorePortal") return;

        if (!same_wall)
        {
            Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), false);
            return;
        }


        if (same_wall && teleported)
        {
            teleported = false;
            return;
        }
        else
        {
            Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), false);
        }

    }
    private bool IsBehindPortal(Collider entity)
    {
        double x_coordinate = (transform.position.x * -1) + entity.transform.position.x;
        double z_coordinate = (transform.position.z * -1) + entity.transform.position.z;

        double radian = (System.Math.Atan2(z_coordinate, x_coordinate) - (1.5708)) ;

        double degrees = RadianTo360Degree(radian);

        if (degrees > 90 && degrees < 270) return true;
        else                               return false;    
    }

    private double RadianTo360Degree(double radian)
    {
        double angle = (radian * 180) / Mathf.PI;

        if (angle < 0.0f) angle = 360 - (angle * -1);

        return (angle + transform.rotation.eulerAngles.y) % 360;
    }

    private void Teleport(GameObject voyager) /* Based on defined behavior for the entity being teleported, performs the teleport method that is appropriate for it */
    {
        Debug.Log("Teleported.");
        string voyager_tag = voyager.tag;

        if (voyager.layer == LayerMask.NameToLayer("Bullet")) voyager_tag = "Bullet";
       
        voyager_transform_new = other_pm.GetCameraHelper().transform.localToWorldMatrix * pm.GetCameraHelper().transform.worldToLocalMatrix * voyager.transform.localToWorldMatrix;


        switch (voyager_tag)
        {
            case "Player": /* In the event of a player, handles the appropriate rotation for the camera object as well */
                TeleportVoyager(voyager.transform, voyager_transform_new.GetColumn(3));
                UpdatePlayerCamera(voyager.transform);
                break;
            case "Bullet":
                ReorientBullet(voyager);
                TeleportVoyager(voyager.transform, voyager_transform_new.GetColumn(3));
                break;
            case null:
                TeleportVoyager(voyager.transform, voyager_transform_new.GetColumn(3));
                break;
        }
    }

    private void TeleportVoyager(Transform voyager, Vector3 new_position)
    {
        voyager.position = new_position;
    }

    //public void PreventTeleportViewClipping()
    //{
    //    player_cam = GetComponent<Portal_Camera>().player_camera;

    //    half_height = player_cam.nearClipPlane * Mathf.Tan(player_cam.fieldOfView * 0.5f * Mathf.Deg2Rad);

    //    half_width = half_height * player_cam.aspect;

    //    distance_to_clip_plane_corner = new Vector3(half_width, half_height, player_cam.nearClipPlane).magnitude;

    //    screen_width = distance_to_clip_plane_corner;

    //    screen.transform.localScale = new Vector3(screen.transform.localScale.x, screen.transform.localScale.y, screen_width);
    //}

    private void UpdatePlayerCamera(Transform voyager) /* Calculates the rotation difference between the two portals and sends it to the camera object to update its rotation */
    {
        Vector3 look_delta = other_portal.GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles -
            GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles;

        voyager.GetComponent<PlayerMovement>().SetMouse(look_delta);
    }

    private void ReorientBullet(GameObject bullet)
    {
        Rigidbody bullet_rb = bullet.GetComponent<Rigidbody>();
        //bullet_rb.velocity = new Vector3(0, 0, 0);

        Vector3 look_delta = other_portal.GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles -
        GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles;

        //bullet.transform.rotation = Quaternion.Euler( 90, 90, 90);
        //bullet.transform.rotation = Quaternion.Euler(0, 180, 0);

        //bullet_rb.rotation = Quaternion.Euler(90, 90, 90);

        //bullet_rb.AddRelativeForce(Vector3.up * 50.0f, ForceMode.Impulse);
        //bullet_rb.AddForce(bullet.GetComponent<Bullet>().rigidbody_velocity);


    }
}
