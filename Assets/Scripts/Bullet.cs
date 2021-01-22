using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void OnTriggerEnter(Collider entity)
    {
        Debug.Log("I hit something");
    }
   
    
}
