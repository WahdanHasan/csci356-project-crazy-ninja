using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Controller : MonoBehaviour
{

    [SerializeField] private float walking_speed;
    [SerializeField] private float crouching_speed;
    [SerializeField] private float running_speed;
    [SerializeField] private float jump_force;
    [SerializeField] private float dash_cooldown;

    private float dash_cooldown_timer;
    private float current_speed;
    private bool is_jumping = false;
    private Rigidbody rb;
    private bool can_dash;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        current_speed = walking_speed;
    }

    public void SetCanDash(bool new_value)
    {
        can_dash = new_value;
    }

    void Update()
    {
        GetInput();

    }

    void GetInput()
    {
        dash_cooldown_timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space))
            is_jumping = true;

        if (Input.GetKeyUp(KeyCode.Space))
            is_jumping = false;

        if (Input.GetKeyDown(KeyCode.LeftShift))
            current_speed = running_speed;

        else if (Input.GetKeyUp(KeyCode.LeftShift))
            current_speed = walking_speed;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            current_speed = crouching_speed;

        else if (Input.GetKeyUp(KeyCode.LeftControl))
            current_speed = walking_speed;

        if (Input.GetKeyDown(KeyCode.V) && can_dash && dash_cooldown <=dash_cooldown_timer)
            StartCoroutine(Dash());
    }

    private IEnumerator Dash()
    {
        dash_cooldown_timer = 0;

        current_speed *= 15;
        yield return new WaitForSeconds(0.1f);
        current_speed /= 15;

    }

    void FixedUpdate()
    {
        UpdateMovement();
    }


    void UpdateMovement()
    {
        /* Positional movement */
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction.Normalize();

        //rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal") * current_speed, rb.velocity.y, Input.GetAxisRaw("Vertical") * current_speed) * Time.deltaTime;
        rb.MovePosition(transform.position + Time.deltaTime * current_speed * transform.TransformDirection(direction.x, 0.0f, direction.y));

        rb.velocity = new Vector3(rb.velocity.x * 0.0f, rb.velocity.y, rb.velocity.z * 0.0f);

        /* Jumping */
        if (is_jumping && rb.velocity.y == 0.0f)rb.AddForce(0, jump_force, 0, ForceMode.Impulse);
        

        /* Crouching */
        // Add code to reduce collider height



    }
       
    
}
