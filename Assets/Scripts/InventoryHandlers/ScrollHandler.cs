using System.Collections;
using System.Collections.Generic;
using Scripts.InventoryHandlers;
using UnityEngine;

public class ScrollHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnScrollUp()
    {
        InventoryHandler.ScrollUp();
    }

    public void OnScrollDown()
    {
        InventoryHandler.scrollDown();
    }
}
