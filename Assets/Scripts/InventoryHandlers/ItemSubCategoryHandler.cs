using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.Classes.Parts;
using Scripts.InventoryHandlers;
using Scripts.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSubCategoryHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inventoryScrollView;
    public GameObject inventoryContentPrefab;
    public GameObject weaponSlotPrfab;
    public GameObject itemSlotPrefab;

    

    void Start()
    {
        
    }

    public void OnClick(string subcategory)
    {
        bool isAllCategory = false;
        switch (subcategory)
        {
            case GameManager.ALL_WEAPONS_NAME:
                isAllCategory = true;
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
            case GameManager.ALL_PARTS_NAME:
                isAllCategory = true;
                showAllParts();
                break;
            case GameManager.HAIR_NAME:
                showPartsOfCategory(PartSlot.Hair);
                break;
            case GameManager.FACE_NAME:
                showPartsOfCategory(PartSlot.Face);
                break;
            case GameManager.TOP_NAME:
                showPartsOfCategory(PartSlot.Top);
                break;
            case GameManager.BOTTOM_NAME:
                showPartsOfCategory(PartSlot.Skirt);
                break;
            case GameManager.LEGS_NAME:
                showPartsOfCategory(PartSlot.Legs);
                break;
            case GameManager.HANDS_NAME:
                showPartsOfCategory(PartSlot.Hands);
                break;
            case GameManager.SHOES_NAME:
                showPartsOfCategory(PartSlot.Shoes);
                break;
            case GameManager.ALL_ACC_NAME:
                isAllCategory = true;
                showAllAcc();
                break;
            case GameManager.HEAD_ACC_NAME:
                showPartsOfCategory(PartSlot.HeadAcc);
                break;
            case GameManager.BACK_ACC_NAME:
                showPartsOfCategory(PartSlot.BackAcc);
                break;
            case GameManager.WAIST_ACC_NAME:
                showPartsOfCategory(PartSlot.WaistAcc);
                break;
            case GameManager.ALL_MISC_NAME:
                isAllCategory = true;
                InventoryHandler.ShowNewList(User.inventory.getMisc());
                break;
            case GameManager.ALL_SETS_NAME:
                isAllCategory = true;
                showAllSets();
                break;
        }

        if (isAllCategory)
        {
            InventoryHandler.setAllStatus();
        }
        else
        {
            InventoryHandler.setPreciseStatus();
        }
        
    }

    private void showAllSets()
    {
        List<Part> sets = User.inventory.getParts().FindAll(p => p.isSet());
        InventoryHandler.ShowNewList(InventoryHandler.toItemList(sets));
    }

    private void showAllAcc()
    {
        List<Part> accs = User.inventory.getParts().FindAll(p => p.isAcc());
        InventoryHandler.ShowNewList(InventoryHandler.toItemList(accs));
    }

    private void showAllParts()
    {
        InventoryHandler.ShowNewList(InventoryHandler.toItemList(User.inventory.getParts()));
    }

    private void showPartsOfCategory(PartSlot category)
    {
        List<Part> parts = User.inventory.getParts()
            .FindAll(p => p.PartEquip.Contains(category) && !p.isSet());
        InventoryHandler.ShowNewList(InventoryHandler.toItemList(parts));
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
