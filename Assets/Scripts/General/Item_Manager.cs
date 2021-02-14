using UnityEngine;

public class Item_Manager : Item
{

    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject portal_gun;
    //[SerializeField] private GameObject katana;
    //[SerializeField] private GameObject jetpack;
    //[SerializeField] private GameObject doodad;

    void Start()
    {
        
    }

    public GameObject RequestNewItem(ItemCode item)
    {
        switch(item)
        {
            case ItemCode.Pistol:
                return Instantiate(pistol);
            case ItemCode.Portal_gun:
                return Instantiate(portal_gun);
            //case ItemCode.Katana:
            //    return Instantiate(katana);
            //case ItemCode.Jetpack:
            //    return Instantiate(jetpack);
            //case ItemCode.Doodad:
            //    return Instantiate(doodad);
        }

        return null;
    }
}
