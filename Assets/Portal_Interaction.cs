using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Interaction : MonoBehaviour
{
    private GameObject voyager;
    private GameObject other_portal;
    private bool entity_teleportable = true;


    public void PreventBackTeleport()
    {
        entity_teleportable = false;
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
                Debug.Log("Portal: Behavior for the entity has not been defined");
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
