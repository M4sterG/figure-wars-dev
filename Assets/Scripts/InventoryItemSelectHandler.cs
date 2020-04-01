using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSelectHandler : MonoBehaviour
{
    private const string WEAPON_CLASS_NAME = "Weapons";

    private const string SETS_CLASS_NAME = "Set";

    private const string PARTS_CLASS_NAME = "Parts";

    private const string ACCESSORIES_CLASS_NAME = "Accessories";

    private const string MISC_CLASS_NAME = "Items";

    public Sprite weaponsIdleImg;
    public Sprite weaponsHoverImg;
    public Sprite weaponsClickedImg;

    public Sprite setsIdleImg;
    public Sprite setsHoverImg;
    public Sprite setsClickedImg;
    
    public Sprite partsIdleImg;
    public Sprite partsHoverImg;
    public Sprite partsClickedImg;

    public Sprite accIdleImg;
    public Sprite accHoverImg;
    public Sprite accClickedImg;

    public Sprite miscIdleImg;
    public Sprite miscHoverImg;
    public Sprite miscClickedImg;
    private ObjectStatus status;

    public GameObject itemClassUIObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void onInteract(string itemClass, OperationType opType){
        switch (itemClass)
        {
            case WEAPON_CLASS_NAME:
                setImageByOperationType(opType, weaponsIdleImg, weaponsHoverImg, weaponsClickedImg);
                return;
            case SETS_CLASS_NAME:
                setImageByOperationType(opType, setsIdleImg, setsHoverImg, setsClickedImg);
                break;
            case PARTS_CLASS_NAME:
                setImageByOperationType(opType, partsIdleImg, partsHoverImg, partsClickedImg);
                break;
            case ACCESSORIES_CLASS_NAME:
                setImageByOperationType(opType, accIdleImg, accHoverImg, accClickedImg);
                break;
            case MISC_CLASS_NAME:
                setImageByOperationType(opType, miscIdleImg, miscHoverImg, miscClickedImg);
                break;
            default:
                throw new ArgumentException("Illegal item class name");
        }
    }

    private void setImageByOperationType(OperationType opType, Sprite idleImg, Sprite hoverImg, Sprite clickedImg)
    {
        switch (opType)
        {
            case OperationType.Hovering:
                if (status == ObjectStatus.Idle)
                {
                    setImage(itemClassUIObject, hoverImg);
                }
                return;
            case OperationType.Clicking:
                setImage(itemClassUIObject, clickedImg);
                status = ObjectStatus.Clicked;
                return;
            case OperationType.Unhovering:
                if (status == ObjectStatus.Idle)
                {
                    setImage(itemClassUIObject, idleImg);
                }
                return;
        }
    }

    public void OnHover(string itemClass)
    {
        onInteract(itemClass, OperationType.Hovering);
    }

    public void OnClick(string itemClass)
    {
        onInteract(itemClass, OperationType.Clicking);
    }

    public void OnUnhover(string itemClass)
    {
        onInteract(itemClass, OperationType.Unhovering);
    }

    

    private void setImage(GameObject UIElem, Sprite img)
    {
        try
        {
            UIElem.GetComponent<Image>().sprite = img;
        }
        catch (NullReferenceException nullEx)
        {
            Debug.Log(UIElem + "has no Image componenet");
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
