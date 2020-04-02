using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
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
        addWeaponsToContent(User.inventory.getWeapons());
    }

    private void addWeaponsToContent(List<ActualWeapon> toAdd)
    {
        foreach (Transform child in inventoryScrollView.transform)
        {
            if (!child.name.Contains("Scrollbar"))
            GameObject.Destroy(child.gameObject);
        }
        GameObject content = Instantiate(inventoryContentPrefab);
        GameObject slotPrefab = Instantiate(weaponSlotPrfab);
        
        ScrollRect scrollRect = inventoryScrollView.GetComponent<ScrollRect>();
        scrollRect.content = content.GetComponent<RectTransform>();
        content.transform.SetParent(inventoryScrollView.transform, false);
        
        toAdd.ForEach(w => addToInvContent(w, slotPrefab, content));
    }

    private void showOnlyCategory(WeaponType category)
    {
        List<ActualWeapon> onlySome = User.inventory.getWeapons().FindAll(w => w.getType() == category);
        addWeaponsToContent(onlySome);
        
    }

    public static void addToInvContent(ActualWeapon actualWeapon, GameObject slotPrefab, GameObject contentHolder)
    {
        addToInvContent(actualWeapon.getBaseWeapon(), slotPrefab, contentHolder);
    }

    public static void addToInvContent(Weapon wep, GameObject slotPrefab, GameObject contentHolder)
    {
        if (User.inventory == null)
        {
            throw new ArgumentException("Inventory is null, cannot add weapon");
        }
        GameObject newSlot = Instantiate(slotPrefab);
        newSlot.transform.SetParent(contentHolder.transform, false);
        GameObject itemIcon = newSlot.transform.Find("ItemIcon").gameObject;
        GameObject nameText = newSlot.transform.Find("ItemName").gameObject;

        Image image = itemIcon.GetComponent<Image>();
        Sprite[] icons = Resources.LoadAll<Sprite>(GameManager.WEAPON_ICONS_PATH + wep.IconFile);
        int offset = wep.IconOffset;
        if (icons == null || !(offset >= 0 && offset < icons.Length))
        {
            image.color = GameManager.TRANSPARENT;
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
}
