using System.Collections;
using System.Collections.Generic;
using Scripts.Classes.Main;
using Scripts.Weapons;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    private const string WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_weapon_info.json";
    private const string ITEM_WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_item_weapon_info.json";
    void Start()
    {
        List<Weapon> weapons = WeaponGetter.getWeapons(WEAPON_INFO_MOCK_PATH, ITEM_WEAPON_INFO_MOCK_PATH);
        User.inventory = new Inventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInvSlot()
    {
        
    }

    
}
