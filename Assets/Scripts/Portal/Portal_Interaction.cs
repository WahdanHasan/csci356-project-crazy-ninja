using UnityEngine;

public class Portal_Interaction : MonoBehaviour
{
    private GameObject voyager;
    private GameObject other_portal;
    private Vector3 portal_normal;
    private bool entity_teleportable = true;
    private int portal_forward_sign;
    private int portal_backward_sign;

    public void PreventBackTeleport()
    {
        entity_teleportable = false;
    }

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
        voyager = entity.gameObject;

        if (entity_teleportable)
            Teleport(voyager);
        else
            entity_teleportable = true;
    }

/* EXPERIMENTAL TELEPORTATION CODE  */
    //private void OnTriggerEnter(Collider entity)
    //{
    //    voyager = entity.gameObject;

    //    Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), true);

    //    portal_forward_sign = System.Math.Sign(CalculateLinearDistance(transform.position, voyager.transform.position));

    //}

    //private void OnTriggerStay(Collider entity)
    //{
    //    voyager = entity.gameObject;
    //    float voyager_portal_side_sign = System.Math.Sign(CalculateLinearDistance(transform.position, entity.transform.position));

    //    if (voyager_portal_side_sign != portal_forward_sign) Teleport(voyager);
    //}

    //private float CalculateLinearDistance(Vector3 portal_position, Vector3 voyager_position)
    //{   
    //    float x = (portal_position.x - voyager_position.x) * portal_normal.x;
    //    float z = (portal_position.z - voyager_position.z) * portal_normal.z;

    //    return x - z;
    //}

    //private void OnTriggerExit(Collider entity)
    //{
    //    voyager = entity.gameObject;

    //    Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), false);

    //    other_portal.GetComponent<Portal_Interaction>().PreventBackTeleport();
    //}
    /*                  */

    private void Teleport(GameObject voyager)
    {
        string voyager_tag = voyager.tag;
        
        other_portal.GetComponent<Portal_Interaction>().PreventBackTeleport();

        Matrix4x4 voyager_transform_new = other_portal.transform.localToWorldMatrix * transform.worldToLocalMatrix * voyager.transform.localToWorldMatrix;

        switch (voyager_tag)
        {
            case "Player":
                TeleportVoyager(voyager.transform, voyager_transform_new.GetColumn(3));
                UpdatePlayerCamera(voyager.transform);
                break;
            case "Untagged":
                TeleportVoyager(voyager.transform, voyager_transform_new.GetColumn(3));
                break;
            default:
                Debug.LogError("Portal: Behavior for the entity has not been defined");
                break;
        }
    }

    private void TeleportVoyager(Transform voyager, Vector3 new_position)
    {        
        voyager.position = new_position;
    }

    private void UpdatePlayerCamera(Transform voyager)
    {
        Vector3 look_delta = other_portal.GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles - GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles;

        voyager.GetComponent<Player_Camera_Controller>().SetMouse(look_delta);
    }

}
