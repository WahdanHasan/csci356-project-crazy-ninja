using System.Collections;
using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] private float cooldown=1.5f;
    [SerializeField] private float dash_speed;

    [SerializeField] private ParticleSystem trailParticleSystem;
    [SerializeField] private ParticleSystem glowParticleSystem;
    private bool canDash = true;

    private GameObject player;

    private float timer;

    private void Awake()
    {
        trailParticleSystem.Stop();
        glowParticleSystem.Stop();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(Input.GetKey(KeyCode.LeftShift) && canDash==true && timer >= cooldown)
        {
            trailParticleSystem.Play();
            glowParticleSystem.Play();
            StartCoroutine(PauseParticleSystem());
            timer = 0.0f;
            canDash = false;
        }
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    IEnumerator PauseParticleSystem()
    {
        yield return new WaitForSeconds(1f);
        trailParticleSystem.Stop();
        glowParticleSystem.Stop();
        canDash = true;
    }

}
