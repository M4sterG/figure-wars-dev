using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPrefabs : MonoBehaviour
{
    public static InventoryPrefabs instance;
    public GameObject invContentPrefab;
    public GameObject weaponSlotPrefab;
    public GameObject otherSlotPrefab;
    public GameObject upgrade0Prefab;
    public GameObject upgrade1Prefab;
    public GameObject upgrade2Prefab;
    public GameObject upgrade3Prefab;
    public GameObject upgrade4Prefab;
    public GameObject upgrade5Prefab;
    public Sprite blankIcon;

    public static Sprite BlankIcon
    {
        get => instance.blankIcon;
    }

    public static GameObject InvContentPrefab
    {
        get => instance.invContentPrefab;
    }

    public static GameObject WeaponSlotPrefab
    {
        get => instance.weaponSlotPrefab;
    }

    private static Dictionary<int, GameObject> UpgradePrefabMap
    {
        get
        {
            Dictionary<int, GameObject> map = new Dictionary<int, GameObject>();
            map.Add(0, instance.upgrade0Prefab);
            map.Add(1, instance.upgrade1Prefab);
            map.Add(2, instance.upgrade2Prefab);
            map.Add(3, instance.upgrade3Prefab);
            map.Add(4, instance.upgrade4Prefab);
            map.Add(5, instance.upgrade5Prefab);
            return map;
        }
    }

    public static GameObject getUpgradePrefab(int upgrade)
    {
        if (!(upgrade >= 0 && upgrade <= GameManager.MAX_UPGRADE_LEVEL))
        {
            throw new ArgumentException("Illegal level");
        }

        return UpgradePrefabMap[upgrade];
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
