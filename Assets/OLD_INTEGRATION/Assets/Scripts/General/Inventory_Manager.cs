using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{

    public enum ItemCode /* Macros used by the script to refer to different item slot items */
    {
        Pistol = KeyCode.Alpha1,
        Portal_gun = KeyCode.Alpha2,
        Katana = KeyCode.Alpha3,
        Doodad = KeyCode.Alpha4,
        Jetpack = -1,
        Drop_Item = KeyCode.G,
        None = KeyCode.None,
    }

    public enum ItemSlot
    {
        Pistol = 0,
        Portal_gun = 1,
        Katana = 2,
        Doodad = 3,
        Jetpack = 10,
        None = -1
    }

    [SerializeField] private Transform el_right_hand;
    [SerializeField] private GameObject PISTOL;
    [SerializeField] private GameObject PORTALGUN;
    [SerializeField] private Transform el_left_hand;
    [SerializeField] private Transform el_back;
    [SerializeField] private Transform player_cam;


    private List<GameObject> inventory;
    private ItemSlot equipped_item;
    private Item_Database_Manager im;
    private Weapon gun_script;
    private DetectWeapons detectWeapons;
    [Header("Items available on spawn")]
    [SerializeField] private bool has_pistol;
    [SerializeField] private bool has_portal_gun;
    [SerializeField] private bool has_katana;
    [SerializeField] private bool has_doodad;
    [SerializeField] private bool has_jetpack;

    private void Awake()
    {
        inventory = new List<GameObject>();
    }

    private void Start()
    {
        this.detectWeapons = (DetectWeapons)base.GetComponentInChildren(typeof(DetectWeapons));
        GetItemManager();
        if (im == null)
        {
            Debug.LogError("Couldn't find item manager.");
            gameObject.SetActive(false);
            return;
        }
        GetSpawnItems();
        SetDefaultItem(ItemSlot.Pistol);
        //Test();
        //TestTwo();
    }
    GameObject go;
    private void Test()
    {
        Debug.Log("Test");

        //gun_script.PickupWeapon(true);
        
        //Debug.Log("Test1");
        go = Instantiate(PISTOL);
        Debug.Log("Test2");

        this.gun_script = (Weapon)go.GetComponent(typeof(Weapon));
        Debug.Log("Test3");

        UpdateEquipLocation(go);
        Debug.Log("Test4");
        go.SetActive(true);

        Debug.Log("Spawned and setup");
    }

    private void TestTwo()
    {
        for (int i = 0; i < 10; i++)
        {
            inventory.Add(null);
        }

        if (this.PISTOL != null)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.PISTOL, base.transform.position, Quaternion.identity);
            //GameObject go = UnityEngine.Object.Instantiate<GameObject>(this.spawnWeapon, base.transform.position, Quaternion.identity);
            this.detectWeapons.ForcePickup(gameObject);
            //inventory[0] = UnityEngine.GameObject.Instantiate<GameObject>(this.PORTALGUN, base.transform.position, Quaternion.identity);
            //this.detectWeapons.ForcePickup(inventory[0]);
            //this.detectWeapons.Throw(Vector3.forward);
            //this.detectWeapons.ForceDrop(inventory[1]);

            //inventory[1] = Instantiate(this.PORTALGUN, base.transform.position, Quaternion.identity);
            //this.detectWeapons.ForcePickup(inventory[1]);
            //this.detectWeapons.Shoot(Vector3.forward);


        }
    }

    private void SetDefaultItem(ItemSlot ic) /* Updates the initial equipped item based on parameter */
    {
        if(ItemSlot.Pistol == ic) this.detectWeapons.ForcePickup(inventory[(int)ItemSlot.Pistol]);

        GameObject item = inventory[(int)ic];
        if (item == null)
        {
            Debug.LogError("Item not found");
            return;
        }

        item.gameObject.SetActive(true);

        equipped_item = ic;
    }

    private void GetSpawnItems() /* Fills up inventory with the items set in the editor */
    {
        for(int i = 0; i < 15; i++)
        {
            inventory.Add(null);
        }

        if (has_pistol)
            inventory[(int)ItemSlot.Pistol] = im.RequestNewItem(FetchItemTag(ItemCode.Pistol));
        if (has_portal_gun)
            inventory[(int)ItemSlot.Portal_gun] = im.RequestNewItem(FetchItemTag(ItemCode.Portal_gun));
        if (has_katana)
            inventory[(int)ItemSlot.Katana] = im.RequestNewItem(FetchItemTag(ItemCode.Katana));
        if (has_doodad)
            inventory[(int)ItemSlot.Doodad] = im.RequestNewItem(FetchItemTag(ItemCode.Doodad));
        if (has_jetpack)
            inventory[(int)ItemSlot.Jetpack] = im.RequestNewItem(FetchItemTag(ItemCode.Jetpack));

        foreach (GameObject item in inventory)
            if (item != null)
            {
                Debug.Log(item.tag);
                item.SetActive(false);
                UpdateEquipLocation(item);
            }
    }

    private void GetItemManager()
    {
        im = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Item_Database_Manager>();
    }

    private void Update() /* Calls for an update to the equipped item based on the player's new choice */
    {
        //Debug.Log(GetComponent<Rigidbody>().velocity);
        ItemSlot item_choice = ItemSlot.None;

        if (Input.GetKeyDown((KeyCode)ItemCode.Pistol) && has_pistol)
            item_choice = ItemSlot.Pistol;

        else if (Input.GetKeyDown((KeyCode)ItemCode.Portal_gun) && has_portal_gun)
            item_choice = ItemSlot.Portal_gun;

        else if (Input.GetKeyDown((KeyCode)ItemCode.Katana) && has_katana)
            item_choice = ItemSlot.Katana;

        else if (Input.GetKeyDown((KeyCode)ItemCode.Doodad) && has_doodad)
            item_choice = ItemSlot.Doodad;

        UpdateEquippedItem(item_choice);

        if (Input.GetKeyDown(KeyCode.G))
            DropItem();
        if (Input.GetKeyDown(KeyCode.E))
            PickupItem();

    }

    private void UpdateEquippedItem(ItemSlot item_choice) /* Unequips the current equipped item and equips the new one */
    {
        if (item_choice == ItemSlot.None) return;
        if (equipped_item == item_choice) return;
        if (inventory[(int)item_choice] == null) return;

        //this.detectWeapons.StopUse();

        //if (item_choice == ItemSlot.Portal_gun) this.detectWeapons.ForcePickup(inventory[(int)ItemSlot.Portal_gun]);
        //if (item_choice == ItemSlot.Pistol) this.detectWeapons.ForcePickup(inventory[(int)ItemSlot.Pistol]);

        if(equipped_item == ItemSlot.Pistol) GetComponent<PlayerMovement>().can_fire = false;
    
        if (inventory[(int)equipped_item] != null)
            inventory[(int)equipped_item].SetActive(false);

        if (item_choice == ItemSlot.Pistol)
        {
            GetComponent<PlayerMovement>().can_fire = true;
            //this.detectWeapons.ForcePickup(inventory[(int)ItemSlot.Pistol]);
            //UpdateEquipLocation(inventory[(int)ItemSlot.Pistol]);
        }
        
        inventory[(int)item_choice].SetActive(true);


        equipped_item = item_choice;
    }

    private string FetchItemTag(ItemCode ic)
    {
        switch (ic)
        {
            case ItemCode.Pistol:
                return "Gun";
            case ItemCode.Portal_gun:
                return "PortalGun";
            case ItemCode.Katana:
                return "Katana";
            case ItemCode.Doodad:
                return "Doodad";
            case ItemCode.Jetpack:
                return "Jetpack";
        }

        return null;
    }

    private ItemSlot FetchItemSlot(string tag)
    {
        switch (tag)
        {
            case "Pistol":
                return ItemSlot.Pistol;
            case "PortalGun":
                return ItemSlot.Portal_gun;
            case "Katana":
                return ItemSlot.Katana;
            case "Doodad":
                return ItemSlot.Doodad;
            case "Jetpack":
                return ItemSlot.Jetpack;
        }

        return ItemSlot.None;
    }

    public void OnDeath()
    {
        if(equipped_item == ItemSlot.Pistol) GetComponent<PlayerMovement>().can_fire = false;

        inventory[(int)equipped_item].SetActive(false);
    }

    public void UpdateEquipLocation(GameObject item)
    {
        Transform pos = null;

        switch (item.tag)
        {
            case "Jetpack":
                pos = el_back;
                break;
            default:
                pos = el_right_hand;
                break;
        }

        item.transform.SetParent(pos);
        item.transform.position = pos.position;
        item.transform.rotation = pos.rotation;
    }

    private void ToggleHasItem(ItemSlot item)
    {
        switch(item)
        {
            case ItemSlot.Pistol:
                has_pistol = !has_pistol;
                break;
            case ItemSlot.Portal_gun:
                has_portal_gun = !has_portal_gun;
                break;
            case ItemSlot.Katana:
                has_katana = !has_katana;
                break;
            case ItemSlot.Doodad:
                has_doodad = !has_doodad;
                break;
        }
    }

    private void DropItem()
    {
        GameObject item = inventory[(int)equipped_item];
        inventory[(int)equipped_item] = null;

        ToggleHasItem(equipped_item);
        item.AddComponent<Rigidbody>();
        item.AddComponent<BoxCollider>();

        //Rigidbody item_rb = item.GetComponent<Rigidbody>();


        item.transform.SetParent(null);

        //item_rb.AddForce(transform.forward * 15.0f);

        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i] != null)
            {
                equipped_item = (ItemSlot)i;
                inventory[(int)equipped_item].SetActive(true);
                return;
            }
        }


        equipped_item = ItemSlot.None;
    }

    private void PickupItem()
    {
        Ray ray = new Ray(player_cam.position, player_cam.forward);
        RaycastHit rc_hit;

        if (!Physics.Raycast(ray, out rc_hit)) return;

        Debug.Log(rc_hit.transform.tag);
        ItemSlot item_slot = FetchItemSlot(rc_hit.transform.tag);
        if (item_slot == ItemSlot.None) return;

        if (inventory[(int)item_slot] != null) return;

        ToggleHasItem(item_slot);

        GameObject item = inventory[(int)item_slot] = rc_hit.transform.gameObject;

        Destroy(item.GetComponent<Rigidbody>());
        Destroy(item.GetComponent<BoxCollider>());

        UpdateEquipLocation(item);

        item.SetActive(false);

        if (equipped_item == ItemSlot.None)
        {
            item.SetActive(true);
            equipped_item = item_slot;
        }

        Debug.Log("Picked up");
    }

}
