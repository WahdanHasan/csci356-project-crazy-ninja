using UnityEngine;

public class Portal_Camera : MonoBehaviour
{
    [SerializeField] private Camera portal_camera_prefab;

    private GameObject other_portal;
    private Camera player_camera;
    private Camera portal_camera;

    private void Awake()
    {
        portal_camera = Instantiate(portal_camera_prefab);
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

        Matrix4x4 portal_transform = pm.GetCameraHelper().transform.localToWorldMatrix * other_pm.GetCameraHelper().transform.worldToLocalMatrix * player_camera.transform.localToWorldMatrix;

        portal_camera.transform.SetPositionAndRotation(portal_transform.GetColumn(3), portal_transform.rotation);

        SetClipPlane();
    }

    private void SetClipPlane()
    {
        int dot = System.Math.Sign(Vector3.Dot(transform.forward, transform.position - portal_camera.transform.position));

        Vector3 camera_position = portal_camera.worldToCameraMatrix.MultiplyPoint(transform.position);

        Vector3 camera_position_normal = portal_camera.worldToCameraMatrix.MultiplyVector(transform.forward) * dot;

        portal_camera.projectionMatrix = player_camera.CalculateObliqueMatrix(new Vector4(camera_position_normal.x, camera_position_normal.y, camera_position_normal.z, -Vector3.Dot(camera_position, camera_position_normal)));
    }


}
