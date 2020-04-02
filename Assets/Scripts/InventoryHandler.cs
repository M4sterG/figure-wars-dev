using System;
using System.Collections;
using System.Collections.Generic;
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
        GameObject itemIcon = newSlot.transform.Find("ItemIcon").gameObject;
        GameObject nameText = newSlot.transform.Find("ItemName").gameObject;

        Image image = itemIcon.GetComponent<Image>();
        Sprite[] icons = Resources.LoadAll<Sprite>(GameManager.WEAPON_ICONS_PATH + wep.IconFile);
        int offset = wep.IconOffset;
        if (icons == null || !(offset >= 0 && offset < icons.Length))
        {
            image.color = new Color(0, 0,0, 0);
        }
        else
        {
            image.sprite = icons[offset];
        }
        nameText.GetComponent<TextMeshProUGUI>().text = wep.Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInvSlot()
    {
        
    }

    
}
