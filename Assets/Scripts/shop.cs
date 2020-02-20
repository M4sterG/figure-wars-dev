using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class shop : MonoBehaviour
{
	public Image itemSlot1,itemSlot2,itemSlot3,itemSlot4,itemSlot5,itemSlot6,itemSlot7,itemSlot8,itemSlot9,itemSlot10,itemSlot11,itemSlot12,itemSlot13,itemSlot14,itemSlot15,itemSlot16,itemSlot17,itemSlot18,itemSlot19,itemSlot20,itemSlot21,itemSlot22,itemSlot23,itemSlot24;
	public Image item1,item2,item3,item4,item5,item6,item7,item8,item9,item10,item11,item12,item13,item14,item15,item16,item17,item18,item19,item20,item21,item22,item23,item24;
	public Text item_name1,item_name2,item_name3,item_name4,item_name5,item_name6,item_name7,item_name8,item_name9,item_name10,item_name11,item_name12,item_name13,item_name14,item_name15,item_name16,item_name17,item_name18,item_name19,item_name20,item_name21,item_name22,item_name23,item_name24;
    
	static string type = "Weapons";
	async void Start()
    {
		var slotsArr = new[] {itemSlot1,itemSlot2,itemSlot3,itemSlot4,itemSlot5,itemSlot6,itemSlot7,itemSlot8,itemSlot9,itemSlot10,itemSlot11,itemSlot12,itemSlot13,itemSlot14,itemSlot15,itemSlot16,itemSlot17,itemSlot18,itemSlot19,itemSlot20,itemSlot21,itemSlot22,itemSlot23,itemSlot24};
		var itemsArr = new[] {item1,item2,item3,item4,item5,item6,item7,item8,item9,item10,item11,item12,item13,item14,item15,item16,item17,item18,item19,item20,item21,item22,item23,item24};
		var namesArr = new[] {item_name1,item_name2,item_name3,item_name4,item_name5,item_name6,item_name7,item_name8,item_name9,item_name10,item_name11,item_name12,item_name13,item_name14,item_name15,item_name16,item_name17,item_name18,item_name19,item_name20,item_name21,item_name22,item_name23,item_name24};
		
		var item1Arr = new[] {""};
		var item2Arr = new[] {""};
		var item3Arr = new[] {""};
		var item4Arr = new[] {""};
		var item5Arr = new[] {""};
		var item6Arr = new[] {""};
		var item7Arr = new[] {""};
		var item8Arr = new[] {""};
		var item9Arr = new[] {""};
		var item10Arr = new[] {""};
		var item11Arr = new[] {""};
		var item12Arr = new[] {""};
		var item13Arr = new[] {""};
		var item14Arr = new[] {""};
		var item15Arr = new[] {""};
		var item16Arr = new[] {""};
		var item17Arr = new[] {""};
		var item18Arr = new[] {""};
		var item19Arr = new[] {""};
		var item20Arr = new[] {""};
		var item21Arr = new[] {""};
		var item22Arr = new[] {""};
		var item23Arr = new[] {""};
		var item24Arr = new[] {""};
		
		for(int i = 0; i<slotsArr.Length; i++) {
			slotsArr[i].enabled = false;
			itemsArr[i].enabled = false;
		}
        try
        {
			WebClient client = new WebClient();
			string json = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/items.json");
            json = fixItemListJson(json);
            Debug.Log(json);
            ItemList list = JsonUtility.FromJson<ItemList>(json);
            List<Item> items = list.itemList;
			int i = 0;
            foreach (Item item in items)
            {
				if(item.type == "Bazooka" || item.type == "Melee" || item.type == "Sniper" || item.type == "Rifle" || item.type == "Shotgun" || item.type == "Minigun" || item.type == "Grenade") {
					slotsArr[i].enabled = true;
					itemsArr[i].enabled = true;
					string price;
					if(item.price_c != -1) {
						price = item.price_c.ToString()+"C";
					} else {
						price = item.price_p.ToString()+"P";
					}
					namesArr[i].text = price+"\n"+item.name;
					itemsArr[i].sprite = Resources.Load<Sprite>("Game/Items/"+item.id);
					i++;
				}
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public string fixItemListJson(string json)
    {
        return "{\"itemList\":"+json+"}";
    }

    public class ItemList
    {
        public List<Item> itemList;
    }

    [Serializable]
    public class Item
    {
        public int id;
        public string name;
        public int price_c;
        public int price_p;
        public string type;
        public string description;
        public int power;
        public int range; // melee
        public int run_speed; //melee
        public int secondary_power; // melee
        public int firing_rate; //sg, sniper, zooka etc
        public int accuracy;
        public int reload_speed;
        public int weapon_bullets;
        public int bullets_max;
        public int warm_up_time; // mg
        public int blast_radius; // bazooka
        public int bullet_speed; // bazooka
        public double explosion_armor; //explosion armor
        public double bullet_armor; // bullet armor
        public int shotgun_bullets; // extra from accesory
        public int rifle_bullets; // extra from accesory
        public int bazooka_bullets; // extra from accesory
        public int grenade_bullets; // extra from accesory
        public int gatling_bullets; // extra from accesory
        public int sniper_bullets; // extra from accesory
        public int tank; // sets
        public double move_speed; // sets
        public int rarity; // 1-5

        public Item(int id, string name, int price_c, int price_p, string type, string description,
					int power, int range, int run_speed, int secondary_power, int firing_rate, int accuracy,
					int reload_speed, int weapon_bullets, int bullets_max, int warm_up_time, int blast_radius, int bullet_speed,
					double explosion_armor, double bullet_armor, int shotgun_bullets, int rifle_bullets, int bazooka_bullets,
					int grenade_bullets, int gatling_bullets, int sniper_bullets, int tank, double move_speed, int rarity)
        {
            this.id = id;
            this.name = name;
            this.price_c = price_c;
            this.price_p = price_p;
            this.type = type;
            this.description = description;
            this.power = power;
            this.range = range;
            this.run_speed = run_speed;
            this.secondary_power = secondary_power;
            this.firing_rate = firing_rate;
            this.accuracy = accuracy;
            this.reload_speed = reload_speed;
            this.weapon_bullets = weapon_bullets;
            this.bullets_max = bullets_max;
            this.warm_up_time = warm_up_time;
            this.blast_radius = blast_radius;
            this.bullet_speed = bullet_speed;
            this.explosion_armor = explosion_armor;
            this.bullet_armor = bullet_armor;
            this.shotgun_bullets = shotgun_bullets;
            this.rifle_bullets = rifle_bullets;
            this.bazooka_bullets = bazooka_bullets;
            this.grenade_bullets = grenade_bullets;
            this.gatling_bullets = gatling_bullets;
            this.sniper_bullets = sniper_bullets;
            this.tank = tank;
            this.move_speed = move_speed;
            this.rarity = rarity;
        }

        public enum ItemType
        {
            Melee,
            Rifle,
            Shotgun,
            Sniper,
            Minigun,
            Bazooka,
            Grenade,
            Head,
            Face,
            Accesories,
            Top,
            Pants,
            Shoes,
            Set
        }
    }
}
