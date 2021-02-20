using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnim : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private Health player_health ;
    private bool is_dead;

    void Start()
    {
        player_health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        if (player_health != null) player_health.isDeadOrAlive += PlayDeathAnimation;
    }

    void Update()
    {
        if(is_dead)
        {
            anim.SetBool("isDead", false);
            return;
        }

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

    }

    private void PlayDeathAnimation(bool status)
    {
        is_dead = status;
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        anim.SetBool("isDead", true);

        yield return new WaitForSeconds(0.1f);
        //anim.SetBool("isDead", false);
    }
}
