using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    public Sprite hoverImage;

    public Sprite clickedImage;

    public Sprite idleImage;

    public GameObject slot;
    private Image selectedStatusImage;
    private TextMeshProUGUI itemName;
    private static Color itemIdleColour = new Color32(122, 121, 149, 255);
    private SlotStatus status = SlotStatus.Idle;
    public void OnSlotClicked()
    {
        loadContext();
        if (status == SlotStatus.Idle)
        {
            selectedStatusImage.sprite = clickedImage;
            itemName.color = Color.white;
            status = SlotStatus.Clicked;
        }
    }

    private void loadContext()
    {
        selectedStatusImage = GetComponent<Image>();
        itemName = slot.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnHover()
    {
        if (status != SlotStatus.Idle)
        {
            return;
        }
        loadContext();
        selectedStatusImage.sprite = hoverImage;
        itemName.color = Color.white;
    }

    public void onUnhover()
    {
        if (status != SlotStatus.Idle)
        {
            return;
        }
        loadContext();
        selectedStatusImage.sprite = idleImage;
        itemName.color = itemIdleColour;
    }

    public enum SlotStatus
    {
        Idle,
        Clicked
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
