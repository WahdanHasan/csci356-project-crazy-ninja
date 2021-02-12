using UnityEngine;

public class Portal_Interaction : MonoBehaviour
{
    private GameObject voyager;
    private GameObject other_portal;
    private bool entity_teleportable = true;
    Vector3 portal_normal;
   

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

    //private void OnTriggerEnter(Collider entity)
    //{

    //    return; // AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA


    //    voyager = entity.gameObject;

    //    if (entity_teleportable)
    //        Teleport(voyager);
    //    else
    //        entity_teleportable = true;
    //}

    private void OnTriggerEnter(Collider entity)
    {
        voyager = entity.gameObject;

        Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), true);
        CalculateLinearDistance(transform.position, entity.transform.position);
    }

    private void OnTriggerStay(Collider entity)
    {
        voyager = entity.gameObject;

        //float distance_from_portal = Vector3.Distance(transform.position, voyager.transform.position);

        //Vector3 diff = new Vector3(transform.position.x - entity.transform.position.x, transform.position.y - entity.transform.position.y, transform.position.z - entity.transform.position.z);
        //float distance = Mathf.Sqrt(Mathf.Pow(diff.x,2f) + Mathf.Pow(diff.y , 2f) + Mathf.Pow( diff.z, 2f));

        //float voyager_distance_from_portal = CalculateLinearDistance(transform.position, entity.transform.position);

        //Debug.Log(voyager_distance_from_portal);

        //if (voyager_distance_from_portal == 0.0f) Debug.Log("AAAAAAAAAAAAAA");

    }

    private void CalculateLinearDistance(Vector3 portal_position, Vector3 voyager_position)
    {
        //while (true)
        {
            float x = (portal_position.x - voyager_position.x) * portal_normal.x;
            float z = (portal_position.z - voyager_position.z) * portal_normal.z;
            if (x-z == 0.0f) Debug.Log("AAAAAAAAAAAAAA");
        }

        //return x - z;
    }

    private void OnTriggerExit(Collider entity)
    {
        voyager = entity.gameObject;

        Physics.IgnoreCollision(entity, GetComponent<Portal_Manager>().GetWallCollider(), false);
    }

    private void Teleport(GameObject voyager)
    {
        string voyager_tag = voyager.tag;
        
        other_portal.GetComponent<Portal_Interaction>().PreventBackTeleport();

        Matrix4x4 voyager_transform_new = other_portal.transform.localToWorldMatrix * transform.worldToLocalMatrix * voyager.transform.localToWorldMatrix;

        switch (voyager_tag)
        {
            case "Player":
                TeleportVoyager(voyager.transform, voyager_transform_new.GetColumn(3));
                UpdatePlayerCamera(voyager.transform, voyager_transform_new.rotation.eulerAngles);
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

    private void UpdatePlayerCamera(Transform voyager, Vector3 camera_euler)
    {
        Vector3 look_delta = other_portal.GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles - GetComponent<Portal_Manager>().GetCameraHelper().transform.eulerAngles;

        voyager.GetComponent<Player_Camera_Controller>().SetMouse(look_delta);
    }

}
