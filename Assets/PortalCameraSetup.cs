using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCameraSetup : MonoBehaviour
{

    [SerializeField] private Camera camera_a;
    [SerializeField] private Camera camera_b;
    [SerializeField] private Material camera_a_mat;
    [SerializeField] private Material camera_b_mat;
    

    void Start()
    {
        if (camera_a.targetTexture != null) camera_a.targetTexture.Release();    
        if (camera_b.targetTexture != null) camera_b.targetTexture.Release();

        camera_a.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        camera_b.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        camera_a_mat.mainTexture = camera_a.targetTexture;
        camera_b_mat.mainTexture = camera_b.targetTexture;
    }

}
