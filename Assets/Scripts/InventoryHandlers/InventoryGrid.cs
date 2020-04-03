using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using frame8.ScrollRectItemsAdapter.Classic.Examples.Common;
using Scripts.Classes.Inventory;
using Scripts.Classes.Main;
using Scripts.Weapons;
using TMPro;

namespace frame8.ScrollRectItemsAdapter.Classic.Examples
{
    public class InventoryGrid : ClassicSRIA<InvSlotViewHolder>
	{
		public RectTransform itemPrefab;
		private const string WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_weapon_info.json";
		private const string ITEM_WEAPON_INFO_MOCK_PATH = "Assets/Resources/CGD/mock_item_weapon_info.json";
		public List<ActualWeapon> Data { get; private set; }


		
		protected override void Awake()
		{
			base.Awake();
			Data = new List<ActualWeapon>();
		}

		protected override void Start()
		{
			base.Start();
			List<Weapon> weapons = WeaponGetter.getWeapons(/*WEAPON_INFO_MOCK_PATH, ITEM_WEAPON_INFO_MOCK_PATH*/);
			// simulates getting weapons from db
			User.inventory.addWeapons(weapons);
			Data = User.inventory.getWeapons();
			ResetItems(Data.Count);
		}
		
		protected override InvSlotViewHolder CreateViewsHolder(int itemIndex)
		{
			var instance = new InvSlotViewHolder();
			instance.Init(itemPrefab, itemIndex);

			return instance;
		}

		protected override void UpdateViewsHolder(InvSlotViewHolder vh)
		{
			ActualWeapon model = Data[vh.ItemIndex];
			Sprite[] icons = Resources.LoadAll<Sprite>(GameManager.WEAPON_ICONS_PATH + model.getBaseWeapon().IconFile);
			vh.itemIcon.sprite = icons[model.IconOffset];
			vh.itemName.text = model.getName();
			
			// set viewholder

		}
		#region events from DrawerCommandPanel

		
		
		#endregion

		

		}

    


	public class InvSlotViewHolder : CAbstractViewsHolder
	{
		public Image itemIcon;
		public TextMeshProUGUI itemName;
		//public Transform levelIndicator;


		public override void CollectViews()
		{
			base.CollectViews();

			itemIcon = root.Find("ItemIcon").GetComponent<Image>();
			itemName = root.Find("ItemName").GetComponent<TextMeshProUGUI>();
			//levelIndicator = root.Find("LevelIndicator").transform;
		}
	}
}
