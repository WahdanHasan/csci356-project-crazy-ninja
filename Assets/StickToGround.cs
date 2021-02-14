using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickToGround : MonoBehaviour
{
    [SerializeField] private Transform rc_origin;
    private Ray ray;
    private RaycastHit rc_hit;

    void Update()
    {
        ray = new Ray(rc_origin.position, Vector3.down);

        if (Physics.Raycast(ray, out rc_hit, 100))
        {
            transform.position = rc_hit.point;
        }
    }
}
