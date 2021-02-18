using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private Vector3 previous_transform_position;
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isRunningForward", true);
            anim.SetBool("isRunningBackward", false);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isRunningBackward", true);
            anim.SetBool("isRunningForward", false);
        }
        else
        {
            anim.SetBool("isRunningForward", false);
            anim.SetBool("isRunningBackward", false);
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetBool("isJumping", true);
        }
        else
        {
            anim.SetBool("isJumping", false);
            
        }

        previous_transform_position = transform.position;
    }
}
