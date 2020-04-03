using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Scripts;
using UnityEngine.EventSystems;

public class ItemEquipHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isBasic = false;
    public Image statusImg;
    public Image iconImg;
    public GameObject upgradesBar;
    public GameObject basicPanel;

    public string categoryName;

    public Sprite basicIcon;

    public Sprite idleImg;
    public Sprite hoverImg;
    public Sprite clickedImg;

   
    private GameManager.SlotStatus status;
    
    public void OnHover()
    {
        if (status == GameManager.SlotStatus.Idle)
        {
            statusImg.sprite = hoverImg;
        }
    }

    private void Start()
    {
        EventTrigger trigger = GetComponent<EventTrigger>();
        trigger.AddListener(EventTriggerType.PointerClick, onDoubleClick);
    }

    void onDoubleClick(PointerEventData data)
    {
        int taps = data.clickCount;
        if (taps == 1)
        {
            OnClick();
        }
        if (taps > 1)
        {
            OnUnequip(categoryName);
        }
    }

    public void OnUnhover()
    {
        if (status == GameManager.SlotStatus.Idle)
        {
            statusImg.sprite = idleImg;
        }
    }

    public void OnClick()
    {
        statusImg.sprite = clickedImg;
        status = GameManager.SlotStatus.Clicked;
    }

    public void OnUnequip(string itemClass)
    {
        iconImg.sprite = basicIcon;
        upgradesBar.SetActive(false);
        basicPanel.SetActive(true);
        statusImg.sprite = idleImg;
        isBasic = true;
    }

    
}
