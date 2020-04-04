using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using frame8.ScrollRectItemsAdapter.Classic.Examples.Common;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.Weapons;
using TMPro;
using UnityEditor;

namespace Scripts.InventoryHandlers
{
    public class InventoryGrid : MonoBehaviour
	{
		private const string WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_weapon_info.json";
		private const string ITEM_WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_item_weapon_info.json";

		public static InventoryGrid instance;
		public GameObject inventoryContent;
		public static List<Item> itemList = new List<Item>();
		private static int head = 0;
		private static int tail = 0;

		private static bool lastRowDown = false;

		private static int DataLength
		{
			get => ItemList.Count;
		}
		
		private const int rowSize = 4;
		private const int viewable = 8;
		
		// MUST HAVE EXACTLY 8 SLOTS
		public static GameObject InventoryContent
		{
			get => instance.inventoryContent;
		}

		private static void clearAfter(int index)
		{
			for (int i = index + 1; i < viewable; i++)
			{
				SlotArray[i].SetActive(false);
			}
		}

		private static void show()
		{
			if (DataLength <= viewable)
			{
				tail = DataLength;
				showViewable(DataLength);
			}
			else
			{
				tail = viewable - 1;
				for (int i = 0; i < viewable; i++)
				{
					setUpObject(SlotArray[i], itemList.ElementAt(head + i));
					SlotArray[i].SetActive(true);
				}
			}
		}

		public static void scrollDown()
		{
			if (tail + 1 >= DataLength)
			{
				return;
			}

			head += rowSize;
			if (tail + rowSize >= DataLength)
			{
				tail = DataLength - 1;
			}
			else
			{
				tail += rowSize;
			}

			for (int i = 0; head + i <= tail; i++)
			{
				setUpObject(SlotArray[i], itemList.ElementAt(head + i));
				SlotArray[i].SetActive(true);
				if (i + head == tail)
				{
					clearAfter(i);
				}
			}
		}

		public static void ScrollUp()
		{
			int rows = viewable / rowSize;
			if (head - rowSize < 0)
			{
				return;
			}
			head = head - rowSize;
			int diff = tail - head;
			tail = tail/rowSize * rowSize - 1;
			for (int i = 0; i + head <= tail; i++)
			{
				
				setUpObject(SlotArray[i], itemList.ElementAt(head + i));
				SlotArray[i].SetActive(true);
				if (i + head == tail)
				{
					clearAfter(i);
				}
			}
		}

		private static void getSlotChildren(GameObject slot, Image iconImg, Image statusImg, TextMeshProUGUI itemName)
		{
			iconImg = slot.transform.Find("ItemIcon").gameObject.GetComponent<Image>();
			statusImg = slot.transform.GetComponent<Image>();
			itemName = slot.transform.Find("Itemname").gameObject.GetComponent<TextMeshProUGUI>();
		}

		private static void setUpObject(GameObject slot, Item item)
		{
			Image iconImg, statusImg;
			TextMeshProUGUI itemName;
			if (item.GetType() == typeof(ActualWeapon))
			{
				iconImg = slot.transform.Find("ItemIcon").gameObject.GetComponent<Image>();
				statusImg = slot.transform.GetComponent<Image>();
				itemName = slot.transform.Find("ItemName").gameObject.GetComponent<TextMeshProUGUI>();
				itemName.text = item.Name;
				setIcon(iconImg, item);
			}
		}

		private static void setIcon(Image image, Item item)
		{
			Sprite[] icons = Resources.LoadAll<Sprite>(GameManager.WEAPON_ICONS_PATH + item.IconFile);
			int offset = item.IconOffset;
			if (icons == null || !(offset >= 0 && offset < icons.Length))
			{
				image.color = GameManager.TRANSPARENT;
			}
			else
			{
				image.sprite = icons[offset];
			}
		}

		private static void showViewable(int length)
		{
			head = 0;
			tail = length;
			for (int i = 0; i < length; i++)
			{
				setUpObject(SlotArray[i], itemList.ElementAt(i));
				SlotArray[i].SetActive(true);
			}
		}

		public static List<Item> ItemList
		{
			get { return itemList; }
			set { itemList = value; }
		}

		public static GameObject[] SlotArray
		{
			get
			{
				GameObject[] slots = new GameObject[8];
				int index = 0;
				foreach (Transform child in InventoryContent.transform)
				{
					slots[index] = child.gameObject;
					index++;
				}
				return slots;
			}
		}

		public static List<T> toSpecificList<T>(List<Item> data, T reference) where T : Item
		{
			// data must have actual type T
			return data.Select(it => (T) it).ToList();
		}

		public static List<Item> toItemList<T>(List<T> data) where T : Item
		{
			return data.Select(it => (Item) it).ToList();
			show();
		}
		

		void Awake()
		{
			instance = this;
			List<Weapon> weps = WeaponGetter.getWeapons(WEAPON_INFO_MOCK_PATH, ITEM_WEAPON_INFO_MOCK_PATH).Take(16).ToList();
			User.inventory.addWeapons(weps);
			ItemList = toItemList(User.inventory.getWeapons());
			show();
		}



	}
}
