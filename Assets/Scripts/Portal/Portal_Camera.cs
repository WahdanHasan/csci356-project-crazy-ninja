using UnityEngine;

public class Portal_Camera : MonoBehaviour
{
    [SerializeField] private Camera portal_camera_prefab;

    private GameObject other_portal;
    private Camera player_camera;
    public Camera portal_camera;

    private void Awake() /* Instantiates a camera object from a prefab and fetches a reference to the player camera object */
    {
        portal_camera = Instantiate(portal_camera_prefab);
        player_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();  
    }

    public Camera GetCamera()
    {
        return portal_camera;
    }

    public void AssignPortalScreenTexture(Camera other_portal_camera) /* Assigns the camera to the portal texture, so that it is drawn on the texture */
    {
        //portal_camera.enabled = true;

        GetComponent<MeshRenderer>().material.SetTexture("_MainTex", other_portal_camera.targetTexture);
    }

    public void Setup(GameObject other_portal) /* Creates a reference to the other portal and sets the rendertexture for this portal */
    {
        this.other_portal = other_portal;

        if (portal_camera.targetTexture != null) portal_camera.targetTexture.Release();

        portal_camera.targetTexture = new RenderTexture(Screen.width, Screen.height, 24); ;
    }

    private bool PortalVisible(Renderer r) /* Checks whether or not the portal is within the player camera's view frustum */
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(player_camera);

        return GeometryUtility.TestPlanesAABB(planes, r.bounds);
    }

    private void LateUpdate() /* If the portal is not visible, updates the rotation and position of the portal camera from the portal its at, with respect to the player's position from the portal he's at */
    {
        if (!PortalVisible(other_portal.GetComponent<Renderer>())) return;

        Portal_Manager pm = GetComponent<Portal_Manager>();
        Portal_Manager other_pm = other_portal.GetComponent<Portal_Manager>();

        Matrix4x4 portal_transform = pm.GetCameraHelper().transform.localToWorldMatrix * other_pm.GetCameraHelper().transform.worldToLocalMatrix * player_camera.transform.localToWorldMatrix;

        portal_camera.transform.SetPositionAndRotation(portal_transform.GetColumn(3), portal_transform.rotation);

        SetClipPlane();
    }

    private void SetClipPlane() /* Dynamically adjusts the near clipping plane of the camera in respect to the portal it's looking at, ensures that no object is blocking the camera's view to and through the portal */
    {
        int dot = System.Math.Sign(Vector3.Dot(transform.forward, transform.position - portal_camera.transform.position));

        Vector3 camera_position = portal_camera.worldToCameraMatrix.MultiplyPoint(transform.position);

        Vector3 camera_position_normal = portal_camera.worldToCameraMatrix.MultiplyVector(transform.forward) * dot;

        portal_camera.projectionMatrix = player_camera.CalculateObliqueMatrix(new Vector4(camera_position_normal.x, camera_position_normal.y, camera_position_normal.z, -Vector3.Dot(camera_position, camera_position_normal)));
    }


}
 