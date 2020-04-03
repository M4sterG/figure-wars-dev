using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public const string WEAPON_ICONS_PATH = "Game/Icons/Weapon/";
    public const string MELEE_NAME = "Melee";
    public const string RIFLE_NAME = "Rifle";
    public const string SHOTGUN_NAME = "Shotgun";
    public const string SNIPER_NAME = "Sniper";
    public const string MINIGUN_NAME = "Minigun";
    public const string BAZOOKA_NAME = "Bazooka";
    public const string GRENADE_NAME = "Grenade";
    public static Color TRANSPARENT = new Color(0, 0,0, 0);
    public static Color FULL_COLOR = new Color(25, 255, 255, 255);

    public const int MAX_UPGRADE_LEVEL = 5;
    
    public const string TAB_WEAPONS_NAME = "Weapons";
    public const string TAB_SET_NAME = "Set";
    public const string TAB_PARTS_NAME = "Parts";
    public const string TAB_ACCESORIES_NAME = "Accessories";
    public const string TAB_MISC_NAME = "Items";
    
    public enum SlotStatus {Idle, Clicked}
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
