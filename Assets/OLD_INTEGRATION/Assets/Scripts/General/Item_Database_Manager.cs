using UnityEngine;

public class Item_Database_Manager : MonoBehaviour
{

    [SerializeField] private GameObject pistol;
    [SerializeField] private GameObject portal_gun;
    //[SerializeField] private GameObject katana;
    [SerializeField] private GameObject jetpack;
    [SerializeField] private GameObject doodad;

    public GameObject RequestNewItem(string tag) /* Returns a new object of the item queried */
    {
        switch(tag)
        {
            case "Gun":
                return Instantiate(pistol);
            case "PortalGun":
                return Instantiate(portal_gun);
            //case "Katana":
            //    return Instantiate(katana);
            case "Jetpack":
                return Instantiate(jetpack);
            case "Doodad":
                return Instantiate(doodad);
        }

        return null;
    }
}
