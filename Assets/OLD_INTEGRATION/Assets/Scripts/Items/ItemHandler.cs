using UnityEngine;

public class ItemHandler : MonoBehaviour
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

    public enum EquipLocation
    {
        Right_hand,
        Left_Hand,
        Back,
    }

    protected EquipLocation el;

    public EquipLocation GetEl()
    {
        return el;
    }

    public void ChangeEquipStatus() /* Changes an items equip by inversing its current active setting */
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }


}
