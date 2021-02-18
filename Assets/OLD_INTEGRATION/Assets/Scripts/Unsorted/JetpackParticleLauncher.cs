using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackParticleLauncher : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleLauncher;
    [SerializeField] private Gradient particleColorGradient;
    //ParticleSystem.enableEmmission = false;


    //private int minSpeed=
    private int emitCount=1;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            //set color of emmission
            ParticleSystem.MainModule psMain = particleLauncher.main; //needs to be inside scope
            psMain.startColor = particleColorGradient.Evaluate(Random.Range(0.25f, 0.75f));

            particleLauncher.Emit(emitCount);   //to specify the number of particles emitted every frame
        }
    }
}
