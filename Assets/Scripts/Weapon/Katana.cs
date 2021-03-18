using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : MonoBehaviour
{
    //TO-DO: ENEMY TAKE DAMAGE
    // Katana is used when number key 3 is clicked
    //when the player uses the katana, it gotta slice (slice == true)
    //enable the particle system of the sword
    //start the animation of the slice movement
    //end the slice movement animation
    //use coroutine so the player cant immediately slice again

    [SerializeField] private ParticleSystem trailParticleSystem;
    [SerializeField] private ParticleSystem glowParticleSystem;
    private Animator animatorController;
    private BoxCollider boxCollider;
    private bool canSlice=true;
    //[SerializeField] private Camera cam;
    private Rigidbody rb;

    void Start()
    {
        animatorController = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        //Physics.IgnoreCollision(player, boxCollider, true);

        this.rb = base.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && canSlice == true)
        {
            Debug.Log("KATANA SLICE");
            boxCollider.enabled = true;
            trailParticleSystem.Play();
            glowParticleSystem.Play();
            StartCoroutine(PauseParticleSystem());
            animatorController.SetBool("Slice", true);
            Invoke("EndSlice", 0.5f);
            StartCoroutine(PauseSlice());
            canSlice = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggering KATANA");
        
        Enemy enemy = (Enemy)other.transform.root.GetComponent(typeof(Enemy));
        if (!enemy)
        {
            return;
        }
        if (enemy.IsDead())
        {
            return;
        }
        if (this.canSlice == false)
        {
            RagdollController ragdollController = (RagdollController)other.transform.root.GetComponent(typeof(RagdollController));
            this.rb.AddForce(this.rb.velocity.normalized * 2f, ForceMode.Impulse);
            enemy.DropGun(this.rb.velocity.normalized * 2f);
        }

    }

    
    void EndSlice()
    {
        animatorController.SetBool("Slice", false);
        boxCollider.enabled = false;
    }

    IEnumerator PauseSlice()
    {
        yield return new WaitForSeconds(1f);
        canSlice = true;
    }

    IEnumerator PauseParticleSystem()
    {
        yield return new WaitForSeconds(1f);
        
    }
}
