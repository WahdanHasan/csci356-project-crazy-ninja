using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammo_text;
    private Gun gun_event;

    void Start()
    {
        gun_event.OnFire += UpdateGunHUD;
    }

    private void UpdateGunHUD(int current_clip_size, int magazine_size)
    {
        ammo_text.text = "" + current_clip_size + "/" + magazine_size;
    }


    void Update()
    {
        
    }

    public void UpdateGunInfo()
    {

    }
}
