using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
    enum Item
    {
        Pistol =  KeyCode.Alpha1,
        Portal_gun = KeyCode.Alpha2,
        Katana = KeyCode.Alpha3,
        None = KeyCode.None,
    }

    private Gun pistol;
    private Portal_Gun portal_gun;
    private Item equipped_item;

    private void Start()
    {
        pistol = GetComponent<Gun>();
        portal_gun = GetComponent<Portal_Gun>();

    }

    private void Update()
    {
        Item item_choice = Item.None;

        if (Input.GetKeyDown((KeyCode)Item.Pistol))
            item_choice = Item.Pistol;
        if (Input.GetKeyDown((KeyCode)Item.Portal_gun))
            item_choice = Item.Portal_gun;


        UpdateEquippedItem(item_choice);
    }

    private void UpdateEquippedItem(Item item_choice)
    {
        if (item_choice == Item.None) return;


        ChangeItemEquipStatus(equipped_item);
        equipped_item = item_choice;
        ChangeItemEquipStatus(equipped_item);

    }

    private void ChangeItemEquipStatus(Item item)
    {
        switch(item)
        {
            case Item.Pistol:
                pistol.enabled = !pistol.enabled;
                break;
            case Item.Portal_gun:
                portal_gun.enabled = !portal_gun.enabled;
                break;
            case Item.Katana:
                break;
            default:
                Debug.LogError("Invalid item request.");
                break;
        }
    }

}
