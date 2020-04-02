using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPrefabs : MonoBehaviour
{
    public static InventoryPrefabs instance;
    public GameObject invContentPrefab;
    public GameObject weaponSlotPrefab;
    public GameObject otherSlotPrefab;

    public static GameObject InvContentPrefab
    {
        get => instance.invContentPrefab;
    }

    public static GameObject WeaponSlotPrefab
    {
        get => instance.weaponSlotPrefab;
    }

    public GameObject OtherSlotPrefab
    {
        get => instance.otherSlotPrefab;
    }

    void Awake()
    {
        instance = this;
    }
}
