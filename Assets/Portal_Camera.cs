using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Camera : MonoBehaviour
{
    [SerializeField] private Camera portal_camera_prefab;

    private GameObject player;
    private Camera player_camera;
    private GameObject other_portal;
    private Camera portal_camera;
    private Camera other_portal_camera;


    private void Awake()
    {
        other_portal_camera = Instantiate(portal_camera_prefab);
        player = GameObject.FindGameObjectWithTag("Player");
        player_camera = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<Camera>();

    }


    public void AssignPortalScreenTexture()
    {
        if (portal_camera.targetTexture != null) portal_camera.targetTexture.Release();
        
        portal_camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);

        portal_camera.enabled = true;

        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", portal_camera.targetTexture);

    }

    public void Setup(GameObject other_portal)
    {
        this.other_portal = other_portal;

        other_portal.GetComponent<Portal_Camera>().SetCamera(other_portal_camera);
    }

    public void SetCamera(Camera camera_at_other_portal)
    {
        portal_camera = camera_at_other_portal;
        AssignPortalScreenTexture();

    }

    private bool PortalVisible(Renderer r)
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(player_camera);

        return GeometryUtility.TestPlanesAABB(planes, r.bounds);
    }

    private void LateUpdate()
    {
        AssignPortalScreenTexture();
        if (!PortalVisible(other_portal.GetComponent<Renderer>())) return;

        Matrix4x4 portal_transform = transform.localToWorldMatrix * other_portal.transform.worldToLocalMatrix * player.transform.localToWorldMatrix;

        portal_camera.transform.SetPositionAndRotation(portal_transform.GetColumn(3), portal_transform.rotation);

        portal_camera.transform.position = new Vector3(portal_camera.transform.position.x, portal_camera.transform.position.y + 1.5f, portal_camera.transform.position.z);

    }



}
