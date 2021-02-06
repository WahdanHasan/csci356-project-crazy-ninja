using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Camera : MonoBehaviour
{
    [SerializeField] private Camera portal_camera_prefab;

    private GameObject player;
    private GameObject other_portal;
    private Camera player_camera;
    private Camera portal_camera;
    private RenderTexture camera_rt;

    private void Awake()
    {
        portal_camera = Instantiate(portal_camera_prefab);
        player = GameObject.FindGameObjectWithTag("Player");
        player_camera = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<Camera>();
    }

    public Camera GetCamera()
    {
        return portal_camera;
    }

    public void AssignPortalScreenTexture(Camera other_portal_camera)
    {
        portal_camera.enabled = true;

        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", other_portal_camera.targetTexture);
    }

    public void Setup(GameObject other_portal)
    {
        this.other_portal = other_portal;

        if (portal_camera.targetTexture != null) portal_camera.targetTexture.Release();

        portal_camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24); ;
    }

    private bool PortalVisible(Renderer r)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(player_camera);

        return GeometryUtility.TestPlanesAABB(planes, r.bounds);
    }

    private void LateUpdate()
    {
        if (!PortalVisible(other_portal.GetComponent<Renderer>())) return;

        Portal_Manager pm = GetComponent<Portal_Manager>();
        Portal_Manager other_pm = other_portal.GetComponent<Portal_Manager>();

        Matrix4x4 portal_transform = pm.GetCameraHelper().transform.localToWorldMatrix * other_pm.GetCameraHelper().transform.worldToLocalMatrix * player.transform.localToWorldMatrix;

        portal_camera.transform.SetPositionAndRotation(portal_transform.GetColumn(3), portal_transform.rotation);
        portal_camera.transform.position = new Vector3(portal_camera.transform.position.x, portal_camera.transform.position.y + 1.5f, portal_camera.transform.position.z);
    }

}
