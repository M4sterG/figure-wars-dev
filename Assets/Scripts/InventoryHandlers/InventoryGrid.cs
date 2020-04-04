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
		private static Dictionary<int, GameManager.SlotStatus> statusMap
			= new Dictionary<int, GameManager.SlotStatus>();
		private static int head = 0;
		private static int tail = 0;

		public Sprite idleImage;
		public Sprite clickedImage;

		private static int DataLength
		{
			get => ItemList.Count;
		}

		public static int getHead()
		{
			return head;
		}

		public static Sprite IdleImage
		{
			get => instance.idleImage;
		}

		public static Sprite ClickedImage
		{
			get => instance.clickedImage;
			
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
					setUpObject(SlotArray[i], itemList.ElementAt(head + i), head + i);
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
				setUpObject(SlotArray[i], itemList.ElementAt(head + i), head + i);
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
				
				setUpObject(SlotArray[i], itemList.ElementAt(head + i), head + i);
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

		private static void setUpObject(GameObject slot, Item item, int index)
		{
			Image iconImg, statusImg;
			TextMeshProUGUI itemName;
			if (item.GetType() == typeof(ActualWeapon))
			{
				iconImg = slot.transform.Find("ItemIcon").gameObject.GetComponent<Image>();
				statusImg = slot.transform.GetComponent<Image>();
				itemName = slot.transform.Find("ItemName").gameObject.GetComponent<TextMeshProUGUI>();
				itemName.text = item.Name;
				if (statusMap[index] == GameManager.SlotStatus.Clicked)
				{
					statusImg.sprite = ClickedImage;
				}
				else
				{
					statusImg.sprite = IdleImage;
				}
				setIcon(iconImg, item);
			}
		}
		

		public static void clearStatuses(int except, int localIndex)
		{
			for (int i = 0; i < viewable; i++)
			{
				if (i != localIndex)
				{
					SlotArray[i].GetComponent<Image>().sprite = IdleImage;
				}
			}
			for (int i = 0; i < DataLength; i++)
			{
				if (statusMap[i] == GameManager.SlotStatus.Clicked && i != except)
				{
					statusMap[i] = GameManager.SlotStatus.Idle;
					return;
				}
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
				setUpObject(SlotArray[i], itemList.ElementAt(i), i);
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
			List<Weapon> weps = WeaponGetter.getWeapons(WEAPON_INFO_MOCK_PATH, ITEM_WEAPON_INFO_MOCK_PATH);
			User.inventory.addWeapons(weps);
			ItemList = toItemList(User.inventory.getWeapons());
			defineStatusMap();
			show();
		}

		private static void defineStatusMap()
		{
			statusMap.Clear();
			for (int i = 0; i < DataLength; i++)
			{
				
				statusMap.Add(i, GameManager.SlotStatus.Idle);
			}
		}

		private static bool checkIndex(int index)
		{
			bool good = index >= 0 && index < statusMap.Count;
			if (!good)
			{
				throw new ArgumentException("Illegal index");
			}

			return true;
		}

		public static void setStatus(int index, GameManager.SlotStatus status)
		{
			checkIndex(index);
			statusMap[index] = status;
		}

		public static GameManager.SlotStatus getStatus(int index)
		{
			checkIndex(index);
			return statusMap[index];
		}



	}
}
