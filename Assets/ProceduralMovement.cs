using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralMovement : MonoBehaviour
{
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject player_cam;
    [Header("Put each reference in a 1:1 index fashion, with pairs of limbs being sequentially placed")]
    [SerializeField] private Transform[] looks;
    [SerializeField] private Transform[] targets;
    [SerializeField] private Transform[] ray_origins;

    [SerializeField] private float distance_before_move;
    [SerializeField] private float limb_travel_speed;
    [SerializeField] private float turn_speed;
    [SerializeField] private float step_height;
    

    private Ray ray;
    private RaycastHit[] rc_hits;
    private Vector3[] intermediate_positions;
    private Vector3[] new_positions;
    private Vector3[] directions;
    private Vector3 look_direction;
    private Quaternion look_rotation;
    private float[] lerps;
    private float original_scale = 3.9f;
    private bool[] is_moving;
    private Transform look;
    private Transform target;
    private Transform ray_origin;
    private GameObject enemy;
    private Vector3 average_looks_position;
    private Vector3 body_position;

    void Start()
    {
        intermediate_positions = new Vector3[ray_origins.Length];
        directions = new Vector3[ray_origins.Length];
        lerps = new float[ray_origins.Length];
        new_positions = new Vector3[ray_origins.Length];
        is_moving = new bool[ray_origins.Length];
        rc_hits = new RaycastHit[ray_origins.Length];

        for (int i = 0; i < ray_origins.Length; i++)
        {
            intermediate_positions[i] = looks[i].position;
            directions[i] = looks[i].position;
        }

        distance_before_move *= (distance_before_move % original_scale); /* Experimental */

        for(int limb_index = 0; limb_index < ray_origins.Length; limb_index++)
        {
            ray = new Ray(ray_origins[limb_index].position, Vector3.down);

            if (!Physics.Raycast(ray, out rc_hits[limb_index])) return;

            intermediate_positions[limb_index] = rc_hits[limb_index].point;
            new_positions[limb_index] = rc_hits[limb_index].point;

            looks[limb_index].position = intermediate_positions[limb_index];
        }

    }

    void Update()
    {
        if (enemy == null) return;

        for(int i = 0; i < looks.Length; i++)
        {
            UpdateLimb(i);
        }

        UpdateBody();
    }

    private void UpdateBody()
    {
        look_direction = (player_cam.transform.position - body.transform.position).normalized;
        look_rotation = Quaternion.LookRotation(look_direction);

        body.transform.rotation = Quaternion.Slerp(body.transform.rotation, look_rotation, Time.deltaTime * turn_speed);

        int i;

        for (i = 0; i < ray_origins.Length; i++)
        {
            average_looks_position.y += looks[i].position.y;
        }

        //average_looks_position.x = body.transform.position.x;
        //average_looks_position.y /= i;
        //average_looks_position.z /= body.transform.position.z;
        //// AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        //body.transform.position = average_looks_position;

        //average_looks_position.x = 0.0f;
        //average_looks_position.y = 0.0f;
        //average_looks_position.z = 0.0f;
    }

    private void UpdateLimb(int limb_index) // Balls come back with delay, rest gucci
    {

        if (IsPartnerLimbMoving(limb_index)) return;

        look = looks[limb_index];
        target = targets[limb_index];
        ray_origin = ray_origins[limb_index];
        is_moving[limb_index] = true;


        lerps[limb_index] += Time.deltaTime * limb_travel_speed;

        intermediate_positions[limb_index] = Vector3.Lerp(look.position, new_positions[limb_index], lerps[limb_index]);

        if (intermediate_positions[limb_index] == new_positions[limb_index]) is_moving[limb_index] = false;

        look.position = intermediate_positions[limb_index];


        float distance = CalculateHorizontalDistance(look.position, target.position);

        if (distance < distance_before_move || intermediate_positions[limb_index] != new_positions[limb_index])      return;

        look.position = new_positions[limb_index];

        ray = new Ray(ray_origin.position, Vector3.down);

        if (!Physics.Raycast(ray, out rc_hits[limb_index])) return;

        new_positions[limb_index] = rc_hits[limb_index].point;

        lerps[limb_index] = 0.0f;

        look.rotation = Quaternion.FromToRotation(Vector3.up, rc_hits[limb_index].normal);
        //if (look.position != new_positions[limb_index]) lerps[limb_index] = 0.0f;
    }

    private float CalculateHorizontalDistance(Vector3 pos1, Vector3 pos2)
    {
        return System.Math.Abs((pos1.x - pos2.x) - (pos1.z - pos2.z));
    }

    private bool IsPartnerLimbMoving(int index)
    {
        int other_index;

        if (index % 2 == 0) other_index = index + 1;
        else other_index = index - 1;

        if (is_moving[other_index])
            return true;
        else
            return false;
    }

    public void SetTarget(GameObject current_target)
    {
        enemy = current_target;
    }
}
