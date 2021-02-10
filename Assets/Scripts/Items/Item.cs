using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemCode
    {
        Pistol = KeyCode.Alpha1,
        Portal_gun = KeyCode.Alpha2,
        Katana = KeyCode.Alpha3,
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

    public void DropItem()
    {

    }

    public void ChangeEquipStatus()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void UpdateEquipLocation(Transform t)
    {
        transform.SetParent(t);
        transform.position = t.position;
    }
}
