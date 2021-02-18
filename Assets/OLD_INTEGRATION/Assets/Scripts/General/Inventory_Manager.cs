using System.Collections;
using UnityEngine;

public class Inventory_Manager : ItemHandler
{
    [SerializeField] private Transform el_right_hand;
    [SerializeField] private Transform el_left_hand;
    [SerializeField] private Transform el_back;

    private ArrayList inventory;
    private ItemCode equipped_item;
    private Item_Database_Manager im;
    [Header("Items available on spawn")]
    [SerializeField] private bool has_pistol;
    [SerializeField] private bool has_portal_gun;
    [SerializeField] private bool has_katana;
    [SerializeField] private bool has_doodad;
    [SerializeField] private bool has_jetpack;


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

    private void SetDefaultItem(ItemCode ic) /* Updates the initial equipped item based on parameter */
    {
        ItemHandler item = FetchItem(ic).GetComponent<ItemHandler>();

        item.ChangeEquipStatus();
        UpdateEquipLocation(FetchItem(ic));

        equipped_item = ic;
    }

    private void GetSpawnItems() /* Fills up inventory with the items set in the editor */
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
                if (item.tag != "Jetpack") item.SetActive(false);
                else
                {
                    EquipJetpack(item);
                }
                UpdateEquipLocation(item);
            }

    }

    private void GetItemManager()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Item_Database_Manager>();
    }

    private void Update() /* Calls for an update to the equipped item based on the player's new choice */
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
        else if (Input.GetKeyDown((KeyCode)ItemCode.Drop_Item))
            DropItem();


        UpdateEquippedItem(item_choice);
    }

    private void UpdateEquippedItem(ItemCode item_choice) /* Unequips the current equipped item and equips the new one */
    {
        if (item_choice == ItemCode.None) return;
        if (equipped_item == item_choice) return;
        if (FetchItem(equipped_item) == null) return;
        if (FetchItem(item_choice) == null) return;

        ItemHandler current_item = FetchItem(equipped_item).GetComponent<ItemHandler>();

        current_item.ChangeEquipStatus();

        ItemHandler new_item = FetchItem(item_choice).GetComponent<ItemHandler>();

        new_item.ChangeEquipStatus();

        UpdateEquipLocation(FetchItem(item_choice));

        equipped_item = item_choice;

    }

    private void UpdateEquippedItem(GameObject item_choice)
    {
        if (FetchItem(equipped_item) == null)
        {
            item_choice.SetActive(false);
            SetDefaultItem(FetchItemCode(item_choice));
            return;
        }



        ItemHandler new_item = item_choice.GetComponent<ItemHandler>();

        new_item.ChangeEquipStatus();
        UpdateEquipLocation(item_choice);

        equipped_item = FetchItemCode(item_choice);
        
        
    }
    
    private GameObject FetchItem(ItemCode ic) /* Fetches the item's reference based on the itemcode specified */
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

    private ItemCode FetchItemCode(GameObject item)
    {
        switch(item.tag)
        {
            case "Pistol":
                return ItemCode.Pistol;
            case "PortalGun":
                return ItemCode.Portal_gun;
            case "Katana":
                return ItemCode.Katana;
            case "Doodad":
                return ItemCode.Doodad;
        }

        return ItemCode.None;
    }

    public void OnDeath()
    {

    }

    public void DropItem()
    {
        GameObject item = FetchItem(equipped_item);

        inventory.Remove(item);

        item.GetComponent<ItemHandler>().Drop();

        foreach (GameObject it in inventory)
        {
            if (it != null) 
            { 
                UpdateEquippedItem(it);
                return;
            }
        }

        Debug.LogError("No Items in Inventory");
    }

    public void AddItem(GameObject item)
    {
        Destroy(item.GetComponent<Rigidbody>());
        Destroy(item.GetComponent<BoxCollider>());

        inventory.Add(item);

        if(item.tag == "Jetpack")
        {
            EquipJetpack(item);
            UpdateEquipLocation(item);
            item.SetActive(true);
            return;
        }

        UpdateEquippedItem(item);

        item.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    private void EquipJetpack(GameObject jetpack)
    {
        //GetComponent<Player_Movement_Controller>().SetCanDash(true);
    }

    public void UpdateEquipLocation(GameObject item)
    {
        Transform pos = null;

        switch (item.GetComponent<ItemHandler>().GetEl())
        {
            case EquipLocation.Right_hand:
                pos = el_right_hand;
                break;
            case EquipLocation.Left_Hand:
                pos = el_left_hand;
                break;
            case EquipLocation.Back:
                pos = el_back;
                break;
        }
        item.transform.SetParent(pos);
        item.transform.position = pos.position;
        item.transform.rotation = new Quaternion(0, 0, 0, 0);


    }
}
