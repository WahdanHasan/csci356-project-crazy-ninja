using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPlacement : MonoBehaviour
{
    [SerializeField] private Transform ray_origin;
    [SerializeField] private Transform look_at;
    [SerializeField] private Transform target;
    [SerializeField] private float leg_distance_before_move = 0.3f;
    [SerializeField] private float leg_move_speed = 10.0f;
    [SerializeField] private float leg_zigzag_offset = 0.15f;
    [SerializeField] private bool zigzag;

    private Ray ray;
    private RaycastHit rc_hit;
    private Vector3 previous_transform_position;
    private Vector3 move_to;
    private float distance_from_target;

    void Start()
    {
        ray = new Ray(ray_origin.transform.position, Vector3.down);
        Physics.Raycast(ray, out rc_hit, 100);
        if (!zigzag)
        {
            look_at.position = rc_hit.point;
            target.position = look_at.position;
        }
        else
        {
            look_at.position = rc_hit.point;
            target.position = look_at.position + (transform.forward * leg_zigzag_offset);
        }

        move_to = look_at.position;
    }


    void Update()
    {
        if (look_at.position != move_to)
        {
            look_at.position = Vector3.Lerp(look_at.position, move_to, Time.deltaTime * leg_move_speed);
        }

        if (previous_transform_position == ray_origin.transform.position) return; /* Optimization for when there is no movement */
        previous_transform_position = ray_origin.transform.position;

        ray = new Ray(ray_origin.transform.position, Vector3.down);

        if (!Physics.Raycast(ray, out rc_hit, 100)) return; /* Fix the length here to only check the max height of the leg or something */

        if (!zigzag) target.position = rc_hit.point;
        else target.position = rc_hit.point + (transform.forward * leg_zigzag_offset);

        distance_from_target = Vector3.Distance(target.position, look_at.position);

        if (distance_from_target >= leg_distance_before_move)
        {
            move_to = target.position;
        }

    }
}
