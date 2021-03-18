using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Demon : MonoBehaviour
{
    enum State
    {
        Idle = 1,
        Aggro,
        Patrolling,
    }

    enum Form
    {
        Humanoid,
        Demonic,
    }

    [SerializeField] GameObject ik_parent;
    private Transform[] old_helper_transforms;
    private InverseKinematics[] ik_components;

    private Ray ray;
    private State current_state;
    private Form current_form;
    private RaycastHit rc_hit;
    private NavMeshAgent nma;
    private GameObject current_target;
    private AI_Demon_Forms form_manager;

    private float idle_duration;
    private float patrolling_duration;
    private float idle_duration_max;
    private float patrolling_duration_max;


    private void Start()
    {
        nma = GetComponent<NavMeshAgent>();
        ik_components = ik_parent.GetComponentsInChildren<InverseKinematics>();
        form_manager = GetComponent<AI_Demon_Forms>();

        foreach(InverseKinematics ik in ik_components)
            ik.enabled = false;

        Invoke("donothing", 5.0f);
    }

    private void donothing()
    {
        foreach (InverseKinematics ik in ik_components)
            ik.enabled = true;
    }

    private void Update()
    {
        switch(current_state)
        {
            case State.Idle:
                IdleBehavior();
                break;
            case State.Patrolling:
                PatrollingBehavior();
                break;
        }

        if (current_state != State.Aggro) return;

        nma.SetDestination(current_target.transform.position);        
    }

    private void OnTriggerEnter(Collider entity) /* Raycasts to the new entity that enters the collider range, makes sure its a player before raycasting, makes sure the player isnt obstructed before aggroing */
    {
        if (entity.tag != "Player") return;

        ray = new Ray(transform.position, entity.transform.position - transform.position);

        if (!Physics.Raycast(ray, out rc_hit)) return;

        if (rc_hit.transform.tag != "Player") return;

        if (current_state == State.Aggro)
            ChooseTarget(entity.gameObject);
        else
        {
            current_target = entity.gameObject;
            current_state = State.Aggro;
            TurnAggro(entity.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) /* If getting slide tackled or shot at */
    {

    }

    private void IdleBehavior()
    {
        idle_duration += Time.deltaTime;

        if(idle_duration > idle_duration_max)
        {
            idle_duration = 0.0f;
            current_state = State.Patrolling;
            return;
        }


    }

    private void PatrollingBehavior()
    {
        patrolling_duration += Time.deltaTime;

        if(patrolling_duration > patrolling_duration_max)
        {
            patrolling_duration = 0.0f;
            current_state = State.Idle;
            return;
        }
    }

    private void ChooseTarget(GameObject new_challenger)
    {
        current_target = new_challenger;
        GetComponent<ProceduralMovement>().SetTarget(current_target);
    }

    private void TurnAggro(GameObject challenger)
    {
        SwapForms(Form.Demonic);
    }

    private void SwapForms(Form form)
    {
        switch(form)
        {
            case Form.Humanoid:
                break;
            case Form.Demonic:
                form_manager.SwapFormToDemon();
                break;
        }
    }

}
