using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralPlacement : MonoBehaviour
{
    [SerializeField] private Transform look_at;
    [SerializeField] private Transform target;
    [SerializeField] private float leg_distance_before_move = 0.3f;

    private Ray ray;
    private RaycastHit rc_hit;
    private Vector3 previous_transform_position;
    private Vector3 move_to;
    private float knee_move_to_y;
    private float distance_from_target;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.3198209f, transform.position.z);
        ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out rc_hit, 100);
        look_at.position = rc_hit.point;
        move_to = look_at.position;
    }


    void Update()
    {
        if (look_at.position != move_to)
        {
            look_at.position = Vector3.Lerp(look_at.position, move_to, Time.deltaTime * 30.0f);
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, knee_move_to_y, transform.position.z), Time.deltaTime * (30.0f / 2.0f));
            knee_move_to_y = transform.position.y;
        }

        if (previous_transform_position == transform.position) return; /* Optimization for when there is no movement */
        previous_transform_position = transform.position;

        ray = new Ray(transform.position, Vector3.down);

        if (!Physics.Raycast(ray, out rc_hit, 100)) return; /* Fix the length here to only check the max height of the leg or something */
       
        target.position = rc_hit.point;

        distance_from_target = Vector3.Distance(target.position, look_at.position);

        if(distance_from_target >= leg_distance_before_move)
        {
            //look_at.position = target.position;
            move_to = target.position;
            knee_move_to_y = move_to.y + transform.position.y; // fix this turd
        }

    }
}
