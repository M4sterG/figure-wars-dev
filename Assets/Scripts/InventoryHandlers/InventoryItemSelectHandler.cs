using System;
using System.Collections;
using System.Collections.Generic;
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

    // it is assumed the 3 images are different
    public Sprite idleImg;
    public Sprite hoveringImg;
    public Sprite clickedImg;
    
    
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
                GameObject tab = child.gameObject;
                Image statusImage = tab.GetComponent<Image>();
                statusImage.color = GameManager.TRANSPARENT;
                statusImage.sprite = idleImg;
            }
        }
    }
    

    private void setImageByOperationType(OperationType opType)
    {
        switch (opType)
        {
            case OperationType.Hovering:
                if (Status == ObjectStatus.Idle)
                {
                    setImage(tab, hoveringImg);
                }
                return;
            case OperationType.Clicking:
                setImage(tab, clickedImg);
                setOthersToIdle();
                return;
            case OperationType.Unhovering:
                if (Status == ObjectStatus.Idle)
                {
                    tab.GetComponent<Image>().color = GameManager.TRANSPARENT;
                }
                return;
        }
    }

    public void OnHover()
    {
        setImageByOperationType(OperationType.Hovering);
    }

    public void OnClick()
    {
        setImageByOperationType(OperationType.Clicking);
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
