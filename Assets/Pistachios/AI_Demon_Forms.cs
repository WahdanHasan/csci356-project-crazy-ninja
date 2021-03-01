using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Demon_Forms : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject animated_demon_body;
    [SerializeField] GameObject procedural_demon_body;
    [SerializeField] GameObject animated_hip_joint;
    [SerializeField] GameObject animated_r_hand_joint;
    [SerializeField] GameObject animated_l_hand_joint;
    [SerializeField] GameObject procedural_hip_joint;
    [SerializeField] GameObject procedural_r_hand_look;
    [SerializeField] GameObject procedural_l_hand_look;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SwapFormToDemon()
    {
        anim.SetBool("isIdle", false);
        return;
        StartCoroutine(ChangeToDemon());
    }

    private IEnumerator ChangeToDemon()
    {

        yield return new WaitForSeconds(2.0f);

        procedural_hip_joint.transform.position = animated_hip_joint.transform.position;
        procedural_r_hand_look.transform.position = animated_r_hand_joint.transform.position;
        procedural_l_hand_look.transform.position = animated_l_hand_joint.transform.position;

       // procedural_demon_body.GetComponent<SkinnedMeshRenderer>().enabled = true;
        //animated_demon_body.GetComponent<SkinnedMeshRenderer>().enabled = false;


        Debug.Log("Transformed");
    }
}
