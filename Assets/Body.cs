using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    [SerializeField] private Transform orienter;
    [SerializeField] private Transform left_side_look_at;
    [SerializeField] private Transform right_side_look_at;

    private Vector3 previous_transform_position;
    private Ray ray;
    private RaycastHit rc_hit;

    void Start()
    {
        
    }

    void Update()
    {
        if (transform.position == previous_transform_position) return;
        previous_transform_position = transform.position;

        orienter.transform.position = new Vector3(transform.position.x, (left_side_look_at.position.y + right_side_look_at.position.y)/-2.0f , transform.position.z);
        //transform.position = new Vector3(transform.position.x, , transform.position.z);

        ray = new Ray(transform.position, Vector3.down);

        if (!Physics.Raycast(ray, out rc_hit, 100)) return;

        Quaternion.LookRotation(rc_hit.normal);
        
    }
}
