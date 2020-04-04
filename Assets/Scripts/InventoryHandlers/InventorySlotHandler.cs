using System;
using System.Collections;
using System.Collections.Generic;
using Scripts.InventoryHandlers;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class InventorySlotHandler : MonoBehaviour
{
    public Sprite hoverImage;

    public Sprite clickedImage;

    public Sprite idleImage;

    public GameObject slot;
    private Image selectedStatusImage;
    private TextMeshProUGUI itemName;
    public static Color itemIdleColour = new Color32(122, 121, 149, 255);
    private int invIndex;

    private int SlotIndex
    {
        get => invIndex - InventoryHandler.getHead();
    }
    public void OnSlotClicked()
    {
        loadContext();
        if (InventoryHandler.getStatus(invIndex) == GameManager.SlotStatus.Idle)
        {
            selectedStatusImage.sprite = clickedImage;
            itemName.color = Color.white;
            InventoryHandler.setStatus(invIndex, GameManager.SlotStatus.Clicked);
            InventoryHandler.clearStatuses(invIndex, SlotIndex);
            
        }
    }

    private void loadContext()
    {
        Transform invContentTransform = slot.transform.parent;
        int index = 0;
        foreach (Transform child in invContentTransform)
        {
            if (child.gameObject.name.Equals(this.name))
            {
                invIndex = InventoryHandler.getHead() + index;
                break;
            }
            else
            {
                index++;
            }
        }
        selectedStatusImage = GetComponent<Image>();
        itemName = slot.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnHover()
    {
        loadContext();
        if (InventoryHandler.getStatus(invIndex) != GameManager.SlotStatus.Idle)
        {
            return;
        }
        selectedStatusImage.sprite = hoverImage;
        itemName.color = Color.white;
    }

    public void onUnhover()
    {
        loadContext();
        if (InventoryHandler.getStatus(invIndex) != GameManager.SlotStatus.Idle)
        {
            return;
        }
        selectedStatusImage.sprite = idleImage;
        itemName.color = itemIdleColour;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
