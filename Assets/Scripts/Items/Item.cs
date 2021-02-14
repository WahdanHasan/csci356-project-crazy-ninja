using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemCode
    {
        Pistol = KeyCode.Alpha1,
        Portal_gun = KeyCode.Alpha2,
        Katana = KeyCode.Alpha3,
        Doodad = KeyCode.Alpha4,
        Jetpack = -1,
        None = KeyCode.None,
    }

    public enum EquipLocation
    {
        Right_hand,
        Left_Hand,
        Back,
    }

    protected bool can_pickup;
    protected EquipLocation el;
    

    public void SetCanPickup(bool can_pickup)
    {
        this.can_pickup = can_pickup;
    }

    public void ChangeEquipStatus()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void UpdateEquipLocation(Transform t)
    { //Set this to the item default equip location once everything is done, atm its a location i set manually
        transform.SetParent(t);
        transform.position = t.position;
    }
}
