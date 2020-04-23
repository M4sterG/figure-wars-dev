using System.Collections;
using System.Collections.Generic;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.Classes.Parts;
using Scripts.Weapons;
using UnityEngine;
using UnityEngine.UI;

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
    public const string HEAD_ACC_NAME = "HeadAcc";
    public const string BACK_ACC_NAME = "BackAcc";
    public const string WAIST_ACC_NAME = "WaistAcc";


    public const string ALL_WEAPONS_NAME = "AllWeapons";
    public const string ALL_PARTS_NAME = "AllParts";
    public const string ALL_SETS_NAME = "AllSets";
    public const string ALL_MISC_NAME = "AllMisc";
    public const string ALL_ACC_NAME = "AllAcc";


    public static Dictionary<WeaponType, string> weaponTypeNames = new Dictionary<WeaponType, string>
    {
        {WeaponType.Melee, MELEE_NAME},
        {WeaponType.Rifle, RIFLE_NAME},
        {WeaponType.Shotgun, SHOTGUN_NAME},
        {WeaponType.Sniper, SNIPER_NAME},
        {WeaponType.Minigun, MINIGUN_NAME},
        {WeaponType.Bazooka, BAZOOKA_NAME},
        {WeaponType.Grenade, GRENADE_NAME}
    };
    
    public static Dictionary<PartSlot, string> partTypeNames = new Dictionary<PartSlot, string>
    {
        {PartSlot.HeadAcc, HEAD_ACC_NAME},
        {PartSlot.BackAcc, BACK_ACC_NAME},
        {PartSlot.WaistAcc, WAIST_ACC_NAME},
        {PartSlot.Hair, HAIR_NAME},
        {PartSlot.Face, FACE_NAME},
        {PartSlot.Top, TOP_NAME},
        {PartSlot.Skirt, BOTTOM_NAME},
        {PartSlot.Legs, LEGS_NAME},
        {PartSlot.Shoes, SHOES_NAME},
        {PartSlot.Hands, HANDS_NAME}
    };
    
    public static Color TRANSPARENT = new Color(0, 0,0, 0);
    public static Color FULL_COLOR = new Color(25, 255, 255, 255);

    public const int MAX_UPGRADE_LEVEL = 5;
    
    public const string TAB_WEAPONS_NAME = "Weapons";
    public const string TAB_SET_NAME = "Set";
    public const string TAB_PARTS_NAME = "Parts";
    public const string TAB_ACCESORIES_NAME = "Accessories";
    public const string TAB_MISC_NAME = "Items";
    
    public static Dictionary<WeaponType, int> weaponPositions = new Dictionary<WeaponType, int>
    {
        {WeaponType.Melee, 1},
        {WeaponType.Rifle, 2},
        {WeaponType.Shotgun, 3},
        {WeaponType.Sniper, 4},
        {WeaponType.Minigun, 5},
        {WeaponType.Bazooka, 6},
        {WeaponType.Grenade, 7}
    };
    
    public enum SlotStatus {Idle, Clicked}
    void Start()
    {
        
    }

    public static void setIcon(Image iconImg, Item item)
    {
        switch (item.ItemType)
        {
            case ItemType.Weapon:
                setImageIcon(WEAPON_ICONS_PATH, iconImg, item);
                break;
            case ItemType.Part:
                setImageIcon(OTHERS_ICON_PATH, iconImg, item);
                break;
            case ItemType.Accessory:
                setImageIcon(OTHERS_ICON_PATH, iconImg, item);
                break;
        }
    }

    private static void setImageIcon(string originPath, Image image, Item item)
    {
        Sprite[] icons = Resources.LoadAll<Sprite>(originPath + item.IconFile);
        int offset = item.IconOffset;
        if (icons == null || !(offset >= 0 && offset < icons.Length))
        {
            image.color = GameManager.TRANSPARENT;
        }
        else
        {
            image.color = GameManager.FULL_COLOR;
            image.sprite = icons[offset];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
