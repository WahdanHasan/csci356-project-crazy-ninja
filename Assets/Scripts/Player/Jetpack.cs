using UnityEngine;

public class Jetpack : MonoBehaviour
{
    [SerializeField] private float cooldown;
    [SerializeField] private float dash_speed;

    private GameObject player;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && timer >= cooldown)
        {
            timer = 0.0f;

        }
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }
}
