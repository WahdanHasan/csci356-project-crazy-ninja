using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doodad : MonoBehaviour
{

    [SerializeField] private float desiredTimeScale = 0.35f;
    [SerializeField] private float desiredDuration = 0.5f;
    [SerializeField] private float cooldown = 2.0f;

    private float time_elapsed;
    private bool ready_to_use = true;        

    void Start()
    {
    }

    void Update()
    {

        if (Input.GetButton("Fire1") && ready_to_use)
        {
            ready_to_use = false;      
            PlayerMovement.Instance.Slowmo(0.35f, 0.5f);
            base.Invoke("ResetDoodad", this.cooldown);
        }
    }

    private void ResetDoodad()
    {
        this.ready_to_use = true;
    }
}
