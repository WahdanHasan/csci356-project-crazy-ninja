using System.Collections;
using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{

    //public enum ItemCode /* Macros used by the script to refer to different item slot items */
    //{
    //    Pistol = KeyCode.Alpha1,
    //    Portal_gun = KeyCode.Alpha2,
    //    Katana = KeyCode.Alpha3,
    //    Doodad = KeyCode.Alpha4,
    //    Jetpack = -1,
    //    Drop_Item = KeyCode.G,
    //    None = KeyCode.None,
    //}

    //[SerializeField] private Transform el_right_hand;
    //[SerializeField] private Transform el_left_hand;
    //[SerializeField] private Transform el_back;

    //private ArrayList inventory;
    //private ItemCode equipped_item;
    //private Item_Database_Manager im;
    //private DetectWeapons detectWeapons;
    //private Pickup gun_handle;
    //[Header("Items available on spawn")]
    //[SerializeField] private bool has_pistol;
    //[SerializeField] private bool has_portal_gun;
    //[SerializeField] private bool has_katana;
    //[SerializeField] private bool has_doodad;
    //[SerializeField] private bool has_jetpack;

    //private void Awake()
    //{
    //    inventory = new ArrayList();
    //}

    //private void Start()
    //{
    //    detectWeapons = (DetectWeapons)base.GetComponentInChildren(typeof(DetectWeapons));
    //    //gun_handle = (Pickup)
    //    GetItemManager();
    //    if(im == null)
    //    {
    //        Debug.LogError("Couldn't find item manager.");
    //        gameObject.SetActive(false);
    //        return;
    //    }
    //    GetSpawnItems();
    //    SetDefaultItem(ItemCode.Portal_gun);
    //}

    //private void SetDefaultItem(ItemCode ic) /* Updates the initial equipped item based on parameter */
    //{
    //    ItemHandler item = FetchItemGameObject(ic).GetComponent<ItemHandler>();
    //    if(item == null)
    //    {
    //        Debug.LogError("Item does not inherit from handler.");
    //        return;
    //    }

    //    item.ChangeEquipStatus();
    //    UpdateEquipLocation(FetchItemGameObject(ic));
    //    item.gameObject.SetActive(true);


    //    equipped_item = ic;

    //    detectWeapons.ForcePickup(FetchItemGameObject(ItemCode.Portal_gun));
    //}

    //private void GetSpawnItems() /* Fills up inventory with the items set in the editor */
    //{
    //    if (has_pistol)
    //        inventory.Add(im.RequestNewItem(FetchItemTag(ItemCode.Pistol)));
    //    if (has_portal_gun)
    //        inventory.Add(im.RequestNewItem(FetchItemTag(ItemCode.Portal_gun)));
    //    if (has_katana)
    //        inventory.Add(im.RequestNewItem(FetchItemTag(ItemCode.Katana)));
    //    if (has_doodad)
    //        inventory.Add(im.RequestNewItem(FetchItemTag(ItemCode.Doodad)));
    //    if (has_jetpack)
    //        inventory.Add(im.RequestNewItem(FetchItemTag(ItemCode.Jetpack)));

    //    foreach (GameObject item in inventory)
    //        if (item != null)
    //        {
    //            Debug.Log(item.tag);
    //            item.SetActive(false);
    //            UpdateEquipLocation(item);
    //        }

    //}

    //private void GetItemManager()
    //{
    //    im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Item_Database_Manager>();
    //}

    //private void Update() /* Calls for an update to the equipped item based on the player's new choice */
    //{

    //    ItemCode item_choice = ItemCode.None;

    //    if (Input.GetKeyDown((KeyCode)ItemCode.Pistol) && has_pistol)
    //        item_choice = ItemCode.Pistol;

    //    else if (Input.GetKeyDown((KeyCode)ItemCode.Portal_gun) && has_portal_gun)
    //        item_choice = ItemCode.Portal_gun;

    //    else if (Input.GetKeyDown((KeyCode)ItemCode.Katana) && has_katana)
    //        item_choice = ItemCode.Katana;

    //    else if (Input.GetKeyDown((KeyCode)ItemCode.Doodad) && has_doodad)
    //        item_choice = ItemCode.Doodad;

    //    if (Input.GetKeyDown(KeyCode.K))
    //        DropItem();
        

    //    UpdateEquippedItem(item_choice);
    //}

    //private void UpdateEquippedItem(ItemCode item_choice) /* Unequips the current equipped item and equips the new one */
    //{
    //    if (item_choice == ItemCode.None) return;
    //    if (equipped_item == item_choice) return;
    //    if (FetchItemGameObject(item_choice) == null) return;

    //    if (FetchItemGameObject(equipped_item) != null)
    //        FetchItemGameObject(equipped_item).SetActive(false);

    //    detectWeapons.StopUse();

    //    detectWeapons.ForcePickup(FetchItemGameObject(item_choice));



    //    FetchItemGameObject(item_choice).SetActive(true);

    //    equipped_item = item_choice;

    //}

    //private void UpdateEquippedItem(GameObject item_choice)
    //{
    //    if (FetchItemGameObject(equipped_item) == null)
    //    {
    //        item_choice.SetActive(false);
    //        SetDefaultItem(FetchItemGameObjectCode(item_choice));
    //        return;
    //    }



    //    ItemHandler new_item = item_choice.GetComponent<ItemHandler>();

    //    new_item.ChangeEquipStatus();
    //    UpdateEquipLocation(item_choice);

    //    equipped_item = FetchItemGameObjectCode(item_choice);

    //}

    //private GameObject FetchItemGameObject(ItemCode ic) /* Fetches the item's reference based on the itemcode specified */
    //{
    //    string retrieve_tag = "";

    //    switch(ic)
    //    {
    //        case ItemCode.Pistol:
    //            retrieve_tag = "Gun";
    //            break;
    //        case ItemCode.Portal_gun:
    //            retrieve_tag = "PortalGun";
    //            break;
    //        case ItemCode.Katana:
    //            retrieve_tag = "Katana";
    //            break;
    //        case ItemCode.Doodad:
    //            retrieve_tag = "Doodad";
    //            break;
    //    }

    //    foreach (GameObject item in inventory)
    //        if (item.tag == retrieve_tag) return item;

    //    return null;
    //}

    //private string FetchItemTag(ItemCode ic)
    //{
    //    switch (ic)
    //    {
    //        case ItemCode.Pistol:
    //            return "Gun";
    //        case ItemCode.Portal_gun:
    //            return "PortalGun";
    //        case ItemCode.Katana:
    //            return "Katana";
    //        case ItemCode.Doodad:
    //            return "Doodad";
    //        case ItemCode.Jetpack:
    //            return "Jetpack";
    //    }

    //    return null;
    //}

    //private ItemCode FetchItemGameObjectCode(GameObject item)
    //{
    //    switch (item.tag)
    //    {
    //        case "Gun":
    //            return ItemCode.Pistol;
    //        case "PortalGun":
    //            return ItemCode.Portal_gun;
    //        case "Katana":
    //            return ItemCode.Katana;
    //        case "Doodad":
    //            return ItemCode.Doodad;
    //    }

    //    return ItemCode.None;
    //}

    //public void OnDeath()
    //{

    //}

    //public void DropItem()
    //{
    //    GameObject item = FetchItemGameObject(equipped_item);

    //    inventory.Remove(item);


    //    Drop(item);

    //    foreach (GameObject it in inventory)
    //    {
    //        if (it != null)
    //        {
    //            UpdateEquippedItem(it);
    //            return;
    //        }
    //    }

    //    Debug.LogError("No Items in Inventory");
    //}

    //public void Drop(GameObject item)
    //{
    //    item.AddComponent<Rigidbody>();
    //    item.transform.SetParent(null);


    //    item.gameObject.AddComponent<BoxCollider>();
    //}

    //public void AddItem(GameObject item)
    //{
    //    Destroy(item.GetComponent<Rigidbody>());
    //    Destroy(item.GetComponent<BoxCollider>());

    //    inventory.Add(item);


    //    UpdateEquippedItem(item);

    //    item.transform.rotation = new Quaternion(0, 0, 0, 0);
    //}

    ////private void EquipJetpack(GameObject jetpack)
    ////{
    ////    //GetComponent<Player_Movement_Controller>().SetCanDash(true);
    ////}

    //public void UpdateEquipLocation(GameObject item)
    //{
    //    Transform pos = null;

    //    switch (item.tag)
    //    {
    //        //case "Gun":
    //        //    pos = el_right_hand;
    //        //    break;
    //        default:
    //            pos = el_right_hand;
    //            break;
    //    }

    //    item.transform.SetParent(pos);
    //    item.transform.position = pos.position;
    //    item.transform.rotation = new Quaternion(0, 0, 0, 0);


    //}
}
