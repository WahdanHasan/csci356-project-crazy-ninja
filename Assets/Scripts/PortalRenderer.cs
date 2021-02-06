//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PortalRenderer : MonoBehaviour
//{
//    [SerializeField] private Camera portal_cam_prefab;
//    [SerializeField] private Camera rendering_camera;
   
//    private GameObject[] portals;
//    private Camera[] portal_cameras;

//    void Start()
//    {
//        if (rendering_camera == null)
//        {
//            /* Code to automate it if i have time */
//        }

//        portals = GameObject.FindGameObjectsWithTag("Portal");

//        CreatePortalScreenCameras(portals.Length);
//        AssignPortalScreenMaterials(portals.Length);
//    }

//    private void CreatePortalScreenCameras(int amount)
//    {
//        portal_cameras = new Camera[amount];

//        for (int i = 0 ; i < amount ; i++)
//        {
//            portal_cameras[i] = Instantiate(portal_cam_prefab);
//            portal_cameras[i].gameObject.SetActive(true);

//        }
//    }

//    private void AssignPortalScreenMaterials(int amount)
//    {
        
//        for(int i = 0 ; i < amount ; i++)
//        {
//            if (portal_cameras[i].targetTexture != null) portal_cameras[i].targetTexture.Release();

//            portal_cameras[i].targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

//            portals[i].GetComponent<MeshRenderer>().material.SetTexture("_MainTex", portal_cameras[i].targetTexture);

//        }

//    }
    
//    private void Update()
//    {
//        for(int i = 0; i < portal_cameras.Length; i++)
//        {
//            Renderer r = portals[i].GetComponent<Renderer>();

//            if (PortalVisible(r))
//            {
//                GameObject primary_portal = portals[i];
//                GameObject secondary_portal = portals[i].GetComponent<Portal_Manager>().GetOtherPortal();

//                Matrix4x4 portal_transform = primary_portal.transform.localToWorldMatrix * secondary_portal.transform.worldToLocalMatrix * transform.localToWorldMatrix;


//                portal_cameras[i].transform.SetPositionAndRotation(portal_transform.GetColumn(3), portal_transform.rotation);
//                portal_cameras[i].transform.position = new Vector3(portal_cameras[i].transform.position.x, portal_cameras[i].transform.position.y + 1.5f, portal_cameras[i].transform.position.z);

//                Debug.Log(portal_transform.GetColumn(3));
//                //portal_cameras[i].gameObject.SetActive(true);
//                portal_cameras[i].enabled = true;
//            }

//        }
//    }

//    private bool PortalVisible(Renderer r)
//    {
//        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(rendering_camera);

//        return GeometryUtility.TestPlanesAABB(planes, r.bounds);
//    }
//}
