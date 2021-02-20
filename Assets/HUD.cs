using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI health;
    [SerializeField] private TextMeshProUGUI ammo;

    private GameObject player;
    private Health player_health;

    void Awake()
    {
        health.gameObject.SetActive(false);
        ammo.gameObject.SetActive(true);

    }

    void Update()
    {
        FindPlayer();

    }

    private void FindPlayer()
    {
        if (player != null) return;

        player = GameObject.FindGameObjectWithTag("Player");
        player_health = player.GetComponent<Health>();
        player_health.UpdateHealth += UpdateHealth;
        if (player_health.GetIsActive())
        {
            health.gameObject.SetActive(true);
            UpdateHealth(player_health.GetCurrentHealth(), player_health.GetTotalHealth());
        }
    }

    private void UpdateHealth(int current_health, int starting_health)
    {
        if (!player_health.GetIsActive()) return;
        health.text = "" + current_health + "/" + starting_health;
    }

    private void UpdateAmmo()
    {

    }

    private void ToggleHealthDisplay(bool new_status, int current_health, int starting_health)
    {
        health.gameObject.SetActive(new_status);
        health.text = "" + current_health + "/" + starting_health;
    }    
    private void ToggleAmmoDisplay()
    {
        ammo.gameObject.SetActive(!ammo.gameObject.activeSelf);
    }
}
