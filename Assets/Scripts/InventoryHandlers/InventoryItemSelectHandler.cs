using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSelectHandler : MonoBehaviour
{
    
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

    private Color selectedColor = Color.yellow;/*new Color(179, 174, 99, 255);*/
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
        GameObject invContent = Instantiate(InventoryPrefabs.InvContentPrefab);
        clearScrollViewChildren(invContent);
        switch (itemClass)
        {
            case GameManager.TAB_WEAPONS_NAME:
                User.inventory.getWeapons().ForEach(w => 
                    WeaponCategoryHandler.addToInvContent(w, InventoryPrefabs.WeaponSlotPrefab, invContent));
                break;
            default:
                break;
        }
    }

    private void clearScrollViewChildren(GameObject invContent)
    {
        foreach (Transform child in invScrollView.transform)
        {
            if (!child.name.Contains("Scrollbar"))
                GameObject.Destroy(child.gameObject);
        }
        GameObject slotPrefab = Instantiate(InventoryPrefabs.WeaponSlotPrefab);
        
        ScrollRect scrollRect = invScrollView.GetComponent<ScrollRect>();
        scrollRect.content = invContent.GetComponent<RectTransform>();
        invContent.transform.SetParent(invScrollView.transform, false);
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
