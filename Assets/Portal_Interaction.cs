using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Interaction : MonoBehaviour
{
    public event Action<GameObject> OnEnter= delegate { };

    private void OnTriggerEnter(Collider entity)
    {
        OnEnter(gameObject);
    }
}
