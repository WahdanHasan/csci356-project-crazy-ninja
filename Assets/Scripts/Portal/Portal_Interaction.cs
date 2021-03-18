using UnityEngine;

public class Portal_Interaction : MonoBehaviour
{
    private GameObject other_portal;
    public bool portal_enabled = false;
    private Vector3 portal_normal;
    Matrix4x4 voyager_transform_new;
    bool teleportable = true;

    public void SetPortalNormal(Vector3 new_normal)
    {
        portal_normal = new_normal;
    }

    public void Setup(GameObject other_portal)
    {
        this.other_portal = other_portal;
    }

    private void OnTriggerEnter(Collider entity)
    {
        if (!portal_enabled || entity.tag == "IgnorePortal") return;

        if (ShouldTeleportEntity(entity)) teleportable = false;

        Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), true);
    }

    private void OnTriggerStay(Collider entity)
    {
        if (!portal_enabled || entity.tag == "IgnorePortal") return;

        if (!ShouldTeleportEntity(entity)) teleportable = true;

        if (ShouldTeleportEntity(entity) && teleportable) Teleport(entity.gameObject);
    }

    private void OnTriggerExit(Collider entity)
    {
        if (!portal_enabled || entity.tag == "IgnorePortal") return;


        Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), false);
    }

    private bool ShouldTeleportEntity(Collider entity)
    {
        double x_coordinate = (transform.position.x * -1) + entity.transform.position.x;
        double z_coordinate = (transform.position.z * -1) + entity.transform.position.z;

        double radian = (System.Math.Atan2(z_coordinate, x_coordinate) - (1.5708)) ;

        double degrees = RadianTo360Degree(radian);

        if (degrees > 90 && degrees < 270) return true;
        else 
        {
            voyager_transform_new = other_portal.transform.localToWorldMatrix * transform.worldToLocalMatrix * entity.transform.localToWorldMatrix;
            return false;
        }

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
        Portal_Manager pm = GetComponent<Portal_Manager>();
        Portal_Manager other_pm = other_portal.GetComponent<Portal_Manager>();
        Matrix4x4 portal_transform = other_pm.GetCameraHelper().transform.localToWorldMatrix * pm.GetCameraHelper().transform.worldToLocalMatrix * GetComponent<Portal_Camera>().player_camera.transform.localToWorldMatrix;
        //voyager.position = new_position;
        //Vector3 pos = portal_transform.GetColumn(3);
        //pos += (portal_normal*2);
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
