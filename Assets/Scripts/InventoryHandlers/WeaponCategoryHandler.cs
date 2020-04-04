using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.InventoryHandlers;
using Scripts.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponCategoryHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventoryScrollView;
    public GameObject inventoryContentPrefab;
    public GameObject weaponSlotPrfab;
    public GameObject itemSlotPrefab;

    void Start()
    {
        
    }

    public void OnClick(string wepType)
    {
        switch (wepType)
        {
            case "All":
                showAllWeapons();
                break;
            case GameManager.MELEE_NAME:
                showOnlyCategory(WeaponType.Melee);
                break;
            case GameManager.RIFLE_NAME:
                showOnlyCategory(WeaponType.Rifle);
                break;
            case GameManager.SHOTGUN_NAME:
                showOnlyCategory(WeaponType.Shotgun);
                break;
            case GameManager.SNIPER_NAME:
                showOnlyCategory(WeaponType.Sniper);
                break;
            case GameManager.MINIGUN_NAME:
                showOnlyCategory(WeaponType.Minigun);
                break;
            case GameManager.BAZOOKA_NAME:
                showOnlyCategory(WeaponType.Bazooka);
                break;
            case GameManager.GRENADE_NAME:
                showOnlyCategory(WeaponType.Grenade);
                break;
        }
    }

    private void showAllWeapons()
    {
        InventoryHandler.ShowNewList(InventoryHandler.toItemList(User.inventory.getWeapons()));
    }
    

    private void showOnlyCategory(WeaponType category)
    {
        List<ActualWeapon> onlySome = User.inventory.getWeapons().FindAll(w => w.getType() == category);
        InventoryHandler.ShowNewList(InventoryHandler.toItemList(onlySome));
    }

   
    
    
}
