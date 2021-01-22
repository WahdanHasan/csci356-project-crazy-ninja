using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{

    [SerializeField] private Transform player_view;
    [SerializeField] private Transform this_portal;
    [SerializeField] private Transform other_portal;

    private Vector3 player_offset_from_portal;
    private float angle_difference_between_portals;
    private Quaternion portal_rotation_difference;
    private Vector3 camera_direction;

    void Update()
    {
        player_offset_from_portal = player_view.position - other_portal.position;
        transform.position = this_portal.position + player_offset_from_portal;

        angle_difference_between_portals = Quaternion.Angle(this_portal.rotation, other_portal.rotation);

        portal_rotation_difference = Quaternion.AngleAxis(angle_difference_between_portals, Vector3.up);

        camera_direction = portal_rotation_difference * player_view.forward;

        transform.rotation = Quaternion.LookRotation(camera_direction, Vector3.up);
    }
}
