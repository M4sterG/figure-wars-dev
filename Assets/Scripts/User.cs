using System.Collections;
using System.Collections.Generic;
using Scripts.Classes.Inventory;
using UnityEngine;

public class User : MonoBehaviour
{
    // Start is called before the first frame update
    public static int userID;
    public static Inventory inventory = new Inventory();
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
