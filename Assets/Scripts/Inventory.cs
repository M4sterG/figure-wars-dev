using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Scripts.Classes.Main;
using Scripts.Classes.Parts;
using Scripts.Weapons;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    private const string MOCK_WEAPON_INFO_PATH = "Assets/Resources/CGD/mock_weapon_info.json";
    private const string MOCK_ITEM_WEAPON_INFO_PATH = "Assets/Resources/CGD/mock_item_weapon_info.json";
    void Start()
    {
    
        
        List<Weapon> /*mock*/ weapons = WeaponGetter.getWeapons(MOCK_WEAPON_INFO_PATH, MOCK_ITEM_WEAPON_INFO_PATH);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInvSlot()
    {
        
    }

    
}
