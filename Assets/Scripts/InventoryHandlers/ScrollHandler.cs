using System.Collections;
using System.Collections.Generic;
using Scripts.InventoryHandlers;
using UnityEngine;

public class ScrollHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnScrollUp()
    {
        InventoryGrid.ScrollUp();
    }

    public void OnScrollDown()
    {
        InventoryGrid.scrollDown();
    }
}
