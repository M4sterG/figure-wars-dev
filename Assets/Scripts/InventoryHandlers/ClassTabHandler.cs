using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.InventoryHandlers;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ClassTabHandler : MonoBehaviour
{

    public GameObject bottomBar;
    private const string BAR_NAME = "Bar";
    public GameObject associatedPanel;
    
    private ObjectStatus Status
    {
        get
        {
            Sprite sprite = tab.GetComponent<Image>().sprite;
            if (sprite == null)
            {
                return ObjectStatus.Idle;
            }
            string imgName = sprite.name;
            if (imgName != null && imgName.Equals(clickedImg.name))
            {
                return ObjectStatus.Clicked;
            }
            return ObjectStatus.Idle;
        }
    }

    public GameObject tab;
    public GameObject navBar;
    public GameObject invScrollView;

    // it is assumed the 3 images are different
    public Sprite idleImg;
    public Sprite hoveringImg;
    public Sprite clickedImg;

    private Color selectedColor = Color.yellow;
    private Color baseColor = new Color(85/255f, 91/255f, 142/255f);


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setOthersToIdle()
    {
        foreach (Transform child in navBar.transform)
        {
            if (!child.gameObject.name.Equals(tab.name))
            {
                GameObject currTab = child.gameObject;
                Image statusImage = currTab.GetComponent<Image>();
                if (statusImage == null)
                {
                    continue;
                }
                statusImage.sprite = idleImg;
                statusImage.color = GameManager.TRANSPARENT;
                TextMeshProUGUI textObj = currTab.GetComponentInChildren<TextMeshProUGUI>();
                textObj.color = baseColor;
            }
        }
        loadCurrentBottomTab();
        showEquippedTab();
    }

    private void showEquippedTab()
    {
        if (associatedPanel == null)
        {
            return;
        }

        foreach (Transform child in associatedPanel.transform.parent)
        {
            GameObject childObj = child.gameObject;
            if (!childObj.name.Equals(associatedPanel.name))
            {
                childObj.SetActive(false);
            }
        }
        associatedPanel.SetActive(true);
    }

    private void loadCurrentBottomTab()
    {
        foreach (Transform child in this.transform.parent)
        {
            string barName = child.gameObject.name;
            if (barName.Contains(BAR_NAME) && !barName.Equals(this.name))
            {
                child.gameObject.SetActive(false);
            }
        }
        bottomBar.SetActive(true);
    }
    

    private void setImageByOperationType(OperationType opType)
    {
        TextMeshProUGUI textObj = tab.GetComponentInChildren<TextMeshProUGUI>();
        switch (opType)
        {
            case OperationType.Hovering:
                if (Status == ObjectStatus.Idle)
                {
                    setImage(tab, hoveringImg);
                    textObj.color = baseColor;
                }
                return;
            case OperationType.Clicking:
                setImage(tab, clickedImg);
                textObj.color = selectedColor;
                setOthersToIdle();
                return;
            case OperationType.Unhovering:
                if (Status == ObjectStatus.Idle)
                {
                    setImage(tab, idleImg);
                    tab.GetComponent<Image>().color = GameManager.TRANSPARENT;
                    textObj.color = baseColor;
                }
                return;
        }
    }

    public void OnHover()
    {
        setImageByOperationType(OperationType.Hovering);
    }

    public void OnClick(string itemClass)
    {
        setImageByOperationType(OperationType.Clicking);
        List<Item> itemList;
        switch (itemClass)
        {
            case GameManager.TAB_WEAPONS_NAME:
                itemList = InventoryHandler.toItemList(User.inventory.getWeapons());
                InventoryHandler.ShowNewList(itemList);
                InventoryHandler.activeClass = InventoryHandler.InventoryClass.Weapons;
                break;
            case GameManager.TAB_SET_NAME:
                itemList = InventoryHandler.toItemList(User.inventory.getParts()
                                                        .FindAll(p => p.isSet()));
                InventoryHandler.ShowNewList(itemList);
                InventoryHandler.activeClass = InventoryHandler.InventoryClass.Set;
                break;
            case GameManager.TAB_PARTS_NAME:
                itemList = InventoryHandler.toItemList(User.inventory.getParts());
                InventoryHandler.ShowNewList(itemList);
                InventoryHandler.activeClass = InventoryHandler.InventoryClass.Parts;
                break;
            case GameManager.TAB_ACCESORIES_NAME:
                itemList = InventoryHandler.toItemList(User.inventory.getParts()
                                                            .FindAll(p => p.isAcc()));
                InventoryHandler.ShowNewList(itemList);
                InventoryHandler.activeClass = InventoryHandler.InventoryClass.Accessories;
                break;
            case GameManager.TAB_MISC_NAME:
                itemList = InventoryHandler.toItemList(User.inventory.getMisc());
                InventoryHandler.ShowNewList(itemList);
                InventoryHandler.activeClass = InventoryHandler.InventoryClass.Items;
                break;
            
        }
        InventoryHandler.setAllStatus();
    }
    
    

    public void OnUnhover()
    {
        setImageByOperationType(OperationType.Unhovering);
    }

    

    private void setImage(GameObject UIElem, Sprite newImage)
    {
        try
        {
            Image img = UIElem.GetComponent<Image>();
            img.sprite = newImage;
            img.color = GameManager.FULL_COLOR;
        }
        catch (NullReferenceException nullEx)
        {     
            Debug.Log(nullEx.Message);
        }
    }

    

    private enum OperationType
    {
        Hovering,
        Unhovering,
        Clicking
    }

    private enum ObjectStatus
    {
        Idle, Clicked
    }
    
}
