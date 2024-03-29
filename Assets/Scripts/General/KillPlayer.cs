using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Health h = other.GetComponent<Health>();

        if ( h != null)
        {
            h.TakeDamage(100000);
        }
    }
}
