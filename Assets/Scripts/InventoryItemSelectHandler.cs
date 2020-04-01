using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSelectHandler : MonoBehaviour
{
    
    private ObjectStatus status;

    public GameObject itemClassUIObject;

    public Sprite idleImg;

    public Sprite hoveringImg;

    public Sprite clickedImg;
    private static Color blank = new Color(25, 255, 255, 0);
    private static Color fullColor = new Color(25, 255, 255, 255);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    private void setImageByOperationType(OperationType opType)
    {
        switch (opType)
        {
            case OperationType.Hovering:
                if (status == ObjectStatus.Idle)
                {
                    setImage(itemClassUIObject, hoveringImg);
                }
                return;
            case OperationType.Clicking:
                setImage(itemClassUIObject, clickedImg);
                status = ObjectStatus.Clicked;
                return;
            case OperationType.Unhovering:
                if (status == ObjectStatus.Idle)
                {
                    itemClassUIObject.GetComponent<Image>().color = blank;
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
            img.color = fullColor;

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
