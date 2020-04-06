using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Scripts;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.Classes.Parts;
using Scripts.InventoryHandlers;
using TMPro;
using UnityEngine.EventSystems;

public class ItemEquipHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private bool IsBasic
    {
        get { return this.basicPanel.activeSelf; }
    }
    public Image statusImg;
    public Image iconImg;
    public GameObject upgradesBar;
    public GameObject basicPanel;

    public GameObject otherPanel;
    public GameObject thisPanel;


    public Sprite basicIcon;

    public Sprite idleImg;
    public Sprite hoverImg;
    public Sprite clickedImg;


    private GameManager.SlotStatus getStatus()
    {
        if (statusImg.sprite.name.Equals(clickedImg.name))
        {
            return GameManager.SlotStatus.Clicked;
        }

        return GameManager.SlotStatus.Idle;
    }
    
    

    private void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        trigger.AddListener(EventTriggerType.PointerClick, OnClicks);
    }

    void OnClicks(PointerEventData data)
    {
        int taps = data.clickCount;
        if (taps == 1)
        {
            OnClick();
        }
        if (taps > 1 && !IsBasic)
        {
            OnUnequip();
        }
    }

    public void OnHover()
    {
        if (getStatus() == GameManager.SlotStatus.Idle)
        {
            statusImg.sprite = hoverImg;
        }
    }

    public void OnUnhover()
    {
        if (getStatus() == GameManager.SlotStatus.Idle)
        {
            statusImg.sprite = idleImg;
        }
    }

    public void OnClick()
    {
        statusImg.sprite = clickedImg;
        setOthersToIdle();
        
    }

    private void setOthersToIdle()
    {
        idlePanel(thisPanel);
        idlePanel(otherPanel);
    }

    private void idlePanel(GameObject panel)
    {
        foreach (Transform child in panel.transform)
        {
            GameObject otherSlot = child.gameObject;
            if (!otherSlot.name.Equals(this.name))
                // reference equality needed
            {
                otherSlot.GetComponent<Image>().sprite = idleImg;
            }
        }
    }

    public void OnUnequip()
    {
        iconImg.sprite = basicIcon;
        upgradesBar.SetActive(false);
        basicPanel.GetComponentInChildren<TextMeshProUGUI>().text = "BASIC";
        basicPanel.SetActive(true);
        statusImg.sprite = idleImg;
        switch (InventoryHandler.activeClass)
        {
            case InventoryHandler.InventoryClass.Weapons:
                addWeaponBackToInv();
                break;
            case InventoryHandler.InventoryClass.Accessories:
                addPartBackToInv();
                break;
            case InventoryHandler.InventoryClass.Parts:
                addPartBackToInv();
                break;
            case InventoryHandler.InventoryClass.Set:
                addPartBackToInv();
                break;
                throw new NotImplementedException("Cannot unequip this item");
        }
    }
    
    

    private void addPartBackToInv()
    {
        
        foreach (var pair in GameManager.partTypeNames)
        {
            PartSlot dominantPart;
            if (this.name.Contains(pair.Value))
            {
                dominantPart = pair.Key;
                Part toUnequip = User.inventory.getEquippedParts()[dominantPart];
                setUnderslotsToBasic(toUnequip);
                User.inventory.unequipPart(dominantPart);
                break;
            }
        }
    }

    private void setUnderslotsToBasic(Part part)
    {
        // if the part is a set it sets the other slots it has to basic
        if (part.PartEquip.Count <= 1)
        {
            return;
        }
        foreach (Transform child in thisPanel.transform)
        {
            foreach (PartSlot slot in part.PartEquip)
            {
                GameObject childObj = child.gameObject;
                if (childObj.name.Contains(GameManager.partTypeNames[slot]))
                {
                    //set to basic
                   InventoryHandler.setPartSlotToPredefined(childObj, true);
                }
            }   
        }
        foreach (Transform child in otherPanel.transform)
        {
            foreach (PartSlot slot in part.PartEquip)
            {
                GameObject childObj = child.gameObject;
                if (childObj.name.Contains(GameManager.partTypeNames[slot]))
                {
                    // set to basic
                   InventoryHandler.setPartSlotToPredefined(childObj, true);
                }
            }   
        }
    }

   

    

    

    private void setOtherSlotToBasic(GameObject slot)
    {
        Image icon = slot.transform.Find("ItemIcon").gameObject.GetComponent<Image>();
        icon.sprite = basicIcon;
        upgradesBar.SetActive(false);
        GameObject otherBasicPanel = slot.transform.Find("BasicPanel").gameObject;
        otherBasicPanel.GetComponent<TextMeshProUGUI>().text = "BASIC";
        otherBasicPanel.SetActive(true);
        slot.GetComponent<Image>().sprite = idleImg;
    }
    
    

    private void addWeaponBackToInv()
    {
        
        foreach (KeyValuePair<WeaponType, string> pair in GameManager.weaponTypeNames)
        {
            WeaponType type;
            if (this.name.Contains(pair.Value))
            {
                type = pair.Key;
                User.inventory.unequipWeapon(type);
                if (InventoryHandler.getTabStatus() == InventoryHandler.TabStatus.All)
                {
                    InventoryHandler.ShowNewList(InventoryHandler.toItemList(User.inventory.getWeapons()));
                }
                else
                {
                    List<ActualWeapon> weapons = User.inventory.getWeapons()
                        .FindAll(w => w.WeaponType == type);
                    InventoryHandler.ShowNewList(InventoryHandler.toItemList(weapons));
                }
                break;
            }
        }
        
    }

    
}
