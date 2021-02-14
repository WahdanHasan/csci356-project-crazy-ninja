using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager : Item
{

    private ArrayList inventory;
    private ItemCode equipped_item;
    private Item_Manager im;
    [Header("Items available on spawn")]
    [SerializeField] private bool has_pistol;
    [SerializeField] private bool has_portal_gun;
    [SerializeField] private bool has_katana;
    [SerializeField] private bool has_doodad;
    [SerializeField] private bool has_jetpack;

    [SerializeField] private Transform equip_location;


    private void Awake()
    {
        inventory = new ArrayList();
    }

    private void Start()
    {
        GetItemManager();
        GetSpawnItems();
        SetDefaultItem(ItemCode.Pistol);
    }

    private void SetDefaultItem(ItemCode ic)
    {
        Item item = FetchItem(ic).GetComponent<Item>();

        item.ChangeEquipStatus();
        item.UpdateEquipLocation(equip_location);

        equipped_item = ic;
    }

    private void GetSpawnItems()
    {
        if (has_pistol)
            inventory.Add(im.RequestNewItem(ItemCode.Pistol));
        if (has_portal_gun)
            inventory.Add(im.RequestNewItem(ItemCode.Portal_gun));
        if (has_katana)
            inventory.Add(im.RequestNewItem(ItemCode.Katana));
        if (has_doodad)
            inventory.Add(im.RequestNewItem(ItemCode.Doodad));
        if (has_jetpack)
            inventory.Add(im.RequestNewItem(ItemCode.Jetpack));

        foreach (GameObject item in inventory)
            if (item != null)
            {
                item.SetActive(false);
                item.GetComponent<Item>().UpdateEquipLocation(equip_location);
            }

    }

    private void GetItemManager()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Item_Manager>();
    }

    private void Update()
    {
        ItemCode item_choice = ItemCode.None;

        if (Input.GetKeyDown((KeyCode)ItemCode.Pistol) && has_pistol)
            item_choice = ItemCode.Pistol;

        else if (Input.GetKeyDown((KeyCode)ItemCode.Portal_gun) && has_portal_gun)
            item_choice = ItemCode.Portal_gun;

        else if (Input.GetKeyDown((KeyCode)ItemCode.Katana) && has_katana)
            item_choice = ItemCode.Katana;

        else if (Input.GetKeyDown((KeyCode)ItemCode.Doodad) && has_doodad)
            item_choice = ItemCode.Doodad;


        UpdateEquippedItem(item_choice);
    }

    private void UpdateEquippedItem(ItemCode item_choice)
    {
        if (item_choice == ItemCode.None) return;
        if (equipped_item == item_choice) return;

        Item current_item = FetchItem(equipped_item).GetComponent<Item>();

        current_item.ChangeEquipStatus();

        Item new_item = FetchItem(item_choice).GetComponent<Item>();

        new_item.ChangeEquipStatus();

        new_item.UpdateEquipLocation(equip_location);

        equipped_item = item_choice;

    }
    
    private GameObject FetchItem(ItemCode ic)
    {
        string retrieve_tag = "";

        switch(ic)
        {
            case ItemCode.Pistol:
                retrieve_tag = "Pistol";
                break;
            case ItemCode.Portal_gun:
                retrieve_tag = "PortalGun";
                break;
            case ItemCode.Katana:
                retrieve_tag = "Katana";
                break;
            case ItemCode.Doodad:
                retrieve_tag = "Doodad";
                break;
        }

        foreach (GameObject item in inventory)
            if (item.tag == retrieve_tag) return item;

        return null;

    }

    public void OnDeath()
    {

    }

}
