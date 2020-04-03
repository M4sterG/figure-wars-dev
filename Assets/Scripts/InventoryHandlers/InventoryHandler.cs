using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class InventoryHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private const string WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_weapon_info.json";
    private const string ITEM_WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_item_weapon_info.json";
    private static int rng = 500;
    public GameObject inventoryContent;
    public GameObject weaponInvSlotPrefab;
    public GameObject otherInvSlotPrefab;
    void Start()
    {
       
       /* ShowAllWeapons();
       // weapons.ForEach(w => addToInventory(w));
    }

    private void ShowAllWeapons()
    {
        User.inventory.getWeapons().ForEach(w => 
            WeaponCategoryHandler.addToInvContent(w, weaponInvSlotPrefab, inventoryContent));*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInvSlot()
    {
        
    }

    
}
