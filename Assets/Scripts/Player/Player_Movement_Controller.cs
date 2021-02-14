using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Controller : MonoBehaviour
{

    [SerializeField] private float walking_speed;
    [SerializeField] private float crouching_speed;
    [SerializeField] private float running_speed;
    [SerializeField] private float movement_speed_during_jump;
    [SerializeField] private float jump_force;

    private float current_speed;
    private bool wants_jumping = false;
    private Rigidbody rb;

    Vector3 playerVelocity;

    //jetpack
   /* [SerializeField] private Camera cam;
    //[SerializeField] private GameObject jetpack;
    private bool wants_jetpacking = false;*/
    private bool has_jetpack = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        walking_speed = 5.0f;
        crouching_speed = 2.5f;
        running_speed = 10.0f;
        movement_speed_during_jump = 1.0f;
        jump_force = 5.0f;
        current_speed = walking_speed;
        //has_jetpack=false
    }

    void Update()
    {
        GetInput();

    }

    void GetInput()
    {

        // will return true if spacebar pressed down, else false.
        //wants_jumping = Input.GetKeyDown(KeyCode.Q);
        
        // will return true if spacebar held, else false.
        //!!!!!!!!!!!!!!!!!!!! TO DO: set to false when out of fuel or when touched
        //wants_jetpacking = Input.GetKey(KeyCode.Space); 
       
        if (Input.GetKeyDown(KeyCode.LeftShift))
            current_speed = running_speed;

        else if (Input.GetKeyUp(KeyCode.LeftShift))
            current_speed = walking_speed;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            current_speed = crouching_speed;

        else if (Input.GetKeyUp(KeyCode.LeftControl))
            current_speed = walking_speed;
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }


    void UpdateMovement()
    {
        rbMovePosition(current_speed);
        
        /* Jumping */
        if (wants_jumping && rb.velocity.y == 0.0f)
        {
            playerVelocity = new Vector3(0.0f, jump_force, 0.0f);
            addImpulseForce(playerVelocity);
            Debug.Log("jumping");
        }
        
        /* Jackpacking */
       /* if (has_jetpack && wants_jetpacking && rb.velocity.y == 0.0f)
        {

            //get the mouse position; convert from pixel coordinates to world units
            //Vector3 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

            //rb.MovePosition(transform.position + Time.deltaTime * );


           *//* normalize?
            rb.AddForce()*//*
        }*/


        /* Crouching */
        // Add code to reduce collider height



    }
    
    public void rbMovePosition(float n)
    {
        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction.Normalize();

        //rb.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * current_speed;
        rb.MovePosition(transform.position + Time.deltaTime * n * transform.TransformDirection(direction.x, 0.0f, direction.y));
    }

    public void addImpulseForce(Vector3 v)
    {
        rb.AddForce(v, ForceMode.VelocityChange); //change
    }

    public Vector3 getVelocity()
    {
        return rb.velocity;
    }
       
    
}
