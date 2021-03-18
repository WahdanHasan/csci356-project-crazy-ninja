using UnityEngine;

public class Portal_Interaction : MonoBehaviour
{
    private GameObject voyager;
    private GameObject other_portal;
    public bool portal_enabled = false;
    private Vector3 portal_normal;
    private int portal_forward_sign;

    public void PreventBackTeleport()
    {
        //entity_teleportable = false;
    }

    public void SetPortalNormal(Vector3 new_normal)
    {
        portal_normal = new_normal;
    }

    public void Setup(GameObject other_portal)
    {
        this.other_portal = other_portal;
    }

    //private void OnTriggerEnter(Collider entity) /* On entering, if teleports if the entity is allowed to be teleported, else, sets it to be allowed to be teleported */
    //{
    //    if (entity.gameObject.tag == "IgnorePortal") return;

    //    voyager = entity.gameObject;

    //    if (entity_teleportable)
    //        Teleport(voyager);
    //    else
    //        entity_teleportable = true;
    //}


    //private void OnTriggerEnter(Collider entity)
    //{

    //}

    /* -------------------------------------------------EXPERIMENTAL TELEPORTATION CODE KINDLY IGNORE------------------------------------------------------------------*/
    private void OnTriggerEnter(Collider entity)
    {
        if (!portal_enabled) return;
        //if(entity.GetComponent<Health>().is_teleportable)
        //{

        //}

        voyager = entity.gameObject;

        Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), true);

        portal_forward_sign = System.Math.Sign(CalculateLinearDistance(transform.position, voyager.transform.position));
    }

    private void OnTriggerStay(Collider entity)
    {
        if (!portal_enabled) return;

        if (ShouldTeleportEntity(entity)) Teleport(entity.gameObject);
        voyager = entity.gameObject;
        float voyager_portal_side_sign = System.Math.Sign(CalculateLinearDistance(transform.position, entity.transform.position));

        if (voyager_portal_side_sign != portal_forward_sign) Teleport(voyager);
    }
    
    private bool ShouldTeleportEntity(Collider entity)
    {
        double x_coordinate = (entity.transform.position.x * -1) + transform.position.x;
        double z_coordinate = (entity.transform.position.z * -1) + transform.position.z;

        double radian = (System.Math.Atan2(z_coordinate, x_coordinate) - (1.5708 * GetComponent<Portal_Manager>().camera_helper_translate_by)) * GetComponent<Portal_Manager>().camera_helper_translate_by ;

        double degrees = RadianTo360Degree(radian);

        Debug.Log(degrees);

        if (degrees > 90 || degrees < -90) return true;
        else return false;
        return true;
    }

    private double RadianTo360Degree(double radian)
    {
        double angle = (radian * 180) / Mathf.PI;

        ///if (angle < 0.0f) angle = 360 - (angle * -1);

        return angle;
    }
    private float CalculateLinearDistance(Vector3 portal_position, Vector3 voyager_position)
    {
        float x = (portal_position.x - voyager_position.x) * portal_normal.x;
        float z = (portal_position.z - voyager_position.z) * portal_normal.z;

        return x - z;
    }

    private void OnTriggerExit(Collider entity)
    {
        if (!portal_enabled) return;

        voyager = entity.gameObject;

        //Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), false);
        //other_portal.GetComponent<Portal_Interaction>().PreventBackTeleport();
    }
    /* ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  */

    private void Teleport(GameObject voyager) /* Based on defined behavior for the entity being teleported, performs the teleport method that is appropriate for it */
    {
        string voyager_tag = voyager.tag;

        if (voyager.layer == LayerMask.NameToLayer("Bullet")) voyager_tag = "Bullet";

        other_portal.GetComponent<Portal_Interaction>().PreventBackTeleport();

        Matrix4x4 voyager_transform_new = other_portal.transform.localToWorldMatrix * transform.worldToLocalMatrix * voyager.transform.localToWorldMatrix;

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
            default:
                //Debug.LogError("Portal: Behavior for the entity has not been defined. Tag: " + voyager_tag);
                break;
        }
    }

    private void TeleportVoyager(Transform voyager, Vector3 new_position)
    {        
        voyager.position = new_position;
    }

    private void UpdatePlayerCamera(Transform voyager) /* Calculates the rotation difference between the two portals and sends it to the camera object to update its rotation */
    {
        Vector3 look_delta = other_portal.GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles -
            GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles;

        voyager.GetComponent<PlayerMovement>().SetMouse(look_delta);
    }

    private void ReorientBullet(GameObject bullet)
    {
        Rigidbody bullet_rb = bullet.GetComponent<Rigidbody>();
        bullet_rb.velocity = new Vector3(0, 0, 0);

        Vector3 look_delta = other_portal.GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles -
        GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles;

        bullet.transform.localRotation = Quaternion.Euler(bullet.transform.rotation.x + look_delta.x, bullet.transform.rotation.y + look_delta.y, bullet.transform.rotation.z + look_delta.z);
        //bullet.transform.rotation = Quaternion.Euler(0, 180, 0);

        bullet_rb.AddForce(bullet.GetComponent<Bullet>().rigidbody_velocity);


    }
}
