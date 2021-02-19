using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doodad : MonoBehaviour
{

    [SerializeField] private float desiredTimeScale = 1.0f;
    [SerializeField] private float desiredDuration = 1.0f;

    private PlayerMovement pm;

    void Start()
    {
        pm = base.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            PlayerMovement.Instance.Slowmo(0.35f, 0.5f);
            //Slowmo(desiredTimeScale, desiredDuration);
        }
    }

    private void ResetSlowmo()
    {
        this.desiredTimeScale = 1f;
    }

    public void Slowmo(float timescale, float length)
    {
        base.CancelInvoke("Slowmo");
        this.desiredTimeScale = timescale;
        base.Invoke("ResetSlowmo", length);
    }
}
