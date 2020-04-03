using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts;

public class ItemEquipHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Image statusImg;
    public Image iconImg;
    public GameObject upgradesBar;

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
    
    
}
