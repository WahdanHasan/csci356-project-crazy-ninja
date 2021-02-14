using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera_Controller : MonoBehaviour
{
    [SerializeField] private Transform player_view = null;
    [SerializeField] private float sensitivity = 5.0f;
    [SerializeField] private bool lock_cursor = true;
    [SerializeField][Range(0.0f, 1.0f)] private float mouse_smoothing_time = 0.03f;

    private float camera_pitch = 0.0f;
    private Vector2 direction;
    private Vector2 current_mouse_delta;
    private Vector2 smoothed_mouse_delta;
    
    void Start()
    {
        if(lock_cursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }

    }

    
    void Update()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        //current_mouse_delta = Vector2.SmoothDamp(current_mouse_delta, new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")), ref current_mouse_delta_velocity, mouse_smoothing_time);

        ///* Horizontal Movement */
        //transform.Rotate(Vector3.up * current_mouse_delta.x * sensitivity);

        ///* Vertical Movement */
        //camera_pitch -= current_mouse_delta.y * sensitivity;

        //if (camera_pitch < -90.0f) camera_pitch = -90.0f;
        //else if (camera_pitch > 90.0f) camera_pitch = 90.0f;

        //player_view.localEulerAngles = Vector3.right * camera_pitch;

        direction = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        direction = Vector2.Scale(direction, new Vector2(sensitivity * mouse_smoothing_time, sensitivity * mouse_smoothing_time));

        smoothed_mouse_delta.x = Mathf.Lerp(smoothed_mouse_delta.x, direction.x, 1.0f / mouse_smoothing_time);
        smoothed_mouse_delta.y = Mathf.Lerp(smoothed_mouse_delta.x, direction.y, 1.0f / mouse_smoothing_time);

        current_mouse_delta += smoothed_mouse_delta;

        player_view.localRotation = Quaternion.AngleAxis(-current_mouse_delta.y, Vector3.right);

        transform.localRotation = Quaternion.AngleAxis(current_mouse_delta.x, transform.up);
    }


}
