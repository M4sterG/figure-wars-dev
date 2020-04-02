using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.Weapons;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private const string WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_weapon_info.json";
    private const string ITEM_WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_item_weapon_info.json";
    public GameObject inventoryContent;
    public GameObject weaponInvSlotPrefab;
    public GameObject otherInvSlotPrefab;
    void Start()
    {
        List<Weapon> weapons = WeaponGetter.getWeapons(WEAPON_INFO_MOCK_PATH, ITEM_WEAPON_INFO_MOCK_PATH);
        // simulates getting weapons from db
        weapons.ForEach(w => addToInventory(w));
    }

    private void addToInventory(Weapon wep)
    {
        if (User.inventory == null)
        {
            throw new ArgumentException("Inventory is null, cannot add weapon");
        }
        User.inventory.addActualWeapon(wep);
        GameObject newSlot = Instantiate(weaponInvSlotPrefab);
        newSlot.transform.SetParent(inventoryContent.transform, false);
        Image icon = inventoryContent.GetComponentInChildren<Image>();
        icon.sprite = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInvSlot()
    {
        
    }

    
}
