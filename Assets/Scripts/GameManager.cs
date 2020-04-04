using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public const string WEAPON_ICONS_PATH = "Game/Icons/Weapon/";
    public const string OTHERS_ICON_PATH = "Game/Icons/Nonweapons/";
    
    public const string MELEE_NAME = "Melee";
    public const string RIFLE_NAME = "Rifle";
    public const string SHOTGUN_NAME = "Shotgun";
    public const string SNIPER_NAME = "Sniper";
    public const string MINIGUN_NAME = "Minigun";
    public const string BAZOOKA_NAME = "Bazooka";
    public const string GRENADE_NAME = "Grenade";
    public const string FACE_NAME = "Face";
    public const string HAIR_NAME = "Hair";
    public const string TOP_NAME = "Top";
    public const string BOTTOM_NAME = "Bottom";
    public const string LEGS_NAME = "Legs";
    public const string SHOES_NAME = "Shoes";
    public const string HANDS_NAME = "Hands";
    public const string ALL_WEAPONS_NAME = "AllWeapons";
    public const string ALL_PARTS_NAME = "AllParts";
    public const string ALL_SETS_NAME = "AllSets";
    public const string ALL_MISC_NAME = "AllMisc";
    public const string ALL_ACC_NAME = "AllAcc";
    public const string HEAD_ACC_NAME = "HeadAcc";
    public const string BACK_ACC_NAME = "BackAcc";
    public const string WAIST_ACC_NAME = "WaistAcc";
    
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
