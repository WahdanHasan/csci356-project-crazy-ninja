using UnityEngine;

public class Player_Camera_Controller : MonoBehaviour
{
    [SerializeField] private Transform player_view = null;
    [SerializeField] private float sensitivity = 5.0f;
    [SerializeField] private bool lock_cursor = true;
    [SerializeField][Range(0.0f, 1.0f)] private float mouse_smoothing_time = 0.03f;

    private Vector2 direction;
    private Vector2 current_mouse_delta;
    private Vector2 smoothed_mouse_delta;

    private void Start()
    {
        if(lock_cursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    private void Update()
    {
        UpdateCamera();
    }

    private void UpdateCamera()
    {
        direction = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        direction = Vector2.Scale(direction, new Vector2(sensitivity * mouse_smoothing_time, sensitivity * mouse_smoothing_time));

        smoothed_mouse_delta.x = Mathf.Lerp(smoothed_mouse_delta.x, direction.x, 1.0f / mouse_smoothing_time);
        smoothed_mouse_delta.y = Mathf.Lerp(smoothed_mouse_delta.x, direction.y, 1.0f / mouse_smoothing_time);

        lock (this)
        {
            current_mouse_delta += smoothed_mouse_delta;

            player_view.localRotation = Quaternion.AngleAxis(-current_mouse_delta.y, Vector3.right);

            transform.localRotation = Quaternion.AngleAxis(current_mouse_delta.x, transform.up);
        }
    }

    public Vector2 GetMouse()
    {
        return current_mouse_delta;
    }

    public void SetMouse(Vector3 look_delta)
    {
       lock(this)
        {
            current_mouse_delta.x += look_delta.y;
        }
    }


}
