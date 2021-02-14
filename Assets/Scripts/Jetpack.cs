using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour
{

    //access to character controller --> get current velocity and change it to vroom

    Player_Movement_Controller playerController;

    [SerializeField] private float power;
    [SerializeField] private float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<Player_Movement_Controller>(); //when scipt starts, get the controller
    }

    private void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.V)) /*transform.parent.gameObject.CompareTag("Player")*/
        {
            jetpackWork();
        }

        //colliding with player. this would be beneficial if the jetpack then falls on the floor, but is still in the scene.
        /*
         * try transform.parent = null
         * can have a public bool detachChild to know when to do so
         */

    }

    private void OnTriggerEnter(Collider other)
    {

        //if collided with player and he got no jetpack, then become it's child
        /*if (other.gameObject.CompareTag("Player") && transform.parent==null)
        {
            transform.parent = other.transform;
        }
        */
    }

    private void jetpackWork()
    {
        for (int count = 0; count < 5; count++)
        {
            Vector3 jet = new Vector3(Input.GetAxisRaw("Horizontal"), playerController.getVelocity().y, Input.GetAxisRaw("Vertical")) * power;
            playerController.addImpulseForce(jet);
            Debug.Log(jet);
        }
    }

    /* Questions:
     * - How will you determine the place of the jetpack when attaching it? pre-set a Jetpack empty game object as a child of player. 
     *      Then when set the jetpack item as the child of that Jetpack empty game object? or set it as it's transform position?
     * 
    */
}
