using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class capsule : MonoBehaviour
{
	public Button capsuleButton, closeButton, acceptButton, refuseButton, leftButton, rightButton, listButton, closeListButton;
    public Image itemsListImage, capsulesImage, typeImage, blackScreen, resultImage, capsuleResultImage, capsuleItemImage, dialogBox, leftImage, rightImage, rareImage1, rareImage2, rareImage3, rareImage4, commonImage1, commonImage2, commonImage3, commonImage4, commonImage5, commonImage6, commonImage7, commonImage8, commonImage9, commonImage10, commonImage11, commonImage12, commonImage13, commonImage14, commonImage15;
	public Sprite luckyBlue, luckyYellow, weapons;
	public Text jackpotText, pointsText, coinsText, luckText, dialogText, priceText, itemText;
	public Slider luckySlider;
	
	static int id = 0;
	static string type = "c";
	static int price;
	string itemId;
	string itemRarity;
	string itemName;
	
	public class PlayerInfo
	{
		public int coins, points;
	}
	
    async void Start() {
		blackScreen.enabled = true;
		resultImage.enabled = false;
		capsuleResultImage.enabled = false;
		capsuleItemImage.enabled = false;
		rightImage.enabled = false;
		rightButton.gameObject.SetActive(false);
		WebClient client = new WebClient();
		string result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/capsuleInfo.php?id="+id);
		string[] r = result.Split(';');
		if(type == "c") {
			priceText.text = r[0]+" Coins";
			price = Int32.Parse(r[1]);
		}
		if(type == "p") {
			priceText.text = r[1]+" Points";
			price = Int32.Parse(r[1]);
		}
        capsuleButton.onClick.AddListener(startCapsule);
		closeButton.onClick.AddListener(closeResult);
		listButton.onClick.AddListener(loadItemsList);
		refuseButton.onClick.AddListener(delegate{dialogBox.gameObject.SetActive(false);});
		refuseButton.onClick.AddListener(delegate{blackScreen.enabled = false;});
		closeListButton.onClick.AddListener(delegate{itemsListImage.gameObject.SetActive(false);});
		closeListButton.onClick.AddListener(delegate{blackScreen.enabled = false;});
		leftButton.onClick.AddListener(delegate{changeId("left");});		
		rightButton.onClick.AddListener(delegate{changeId("right");});	
		reloadCapsule();
    }
	
	async void reloadCapsule() {
		try {			
			WebClient client = new WebClient();
			string result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/getPlayerStats.php?key=!switkey&username="+Game.Info.user);
			var json = JsonUtility.FromJson<PlayerInfo>(result);
			pointsText.text = json.points.ToString("#,##0");
			coinsText.text = json.coins.ToString("#,##0");
			
			result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/luck.php?key=!switkey&check&username="+Game.Info.user);
			luckText.text = result+"%";
			luckySlider.value = float.Parse(result)/100f;
			
			result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/jackpot.php?key=!switkey");
			jackpotText.text = "Coins Jackpot: "+result;
			blackScreen.enabled = false;
		} catch {
			blackScreen.enabled = true;
			dialogBox.gameObject.SetActive(true);
			refuseButton.gameObject.SetActive(true);
			acceptButton.gameObject.SetActive(false);
			dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
			dialogText.text = "Error while initializing the capsule machine, please try again or ignore it.";
		}
	}
	
    async void startCapsule()
	{
		WebClient client = new WebClient();
		blackScreen.enabled = true;
		dialogBox.gameObject.SetActive(true);
		refuseButton.gameObject.SetActive(false);
		acceptButton.gameObject.SetActive(false);
		dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/white");
		dialogText.text = "Loading capsules, please wait...";
		try {
			string result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/capsule.php?key=!switkey&id="+id+"&username="+Game.Info.user+"&type="+type);
			if(result == "ERR_NO_ENOUGH_MONEY") {
				blackScreen.enabled = true;
				dialogBox.gameObject.SetActive(true);
				refuseButton.gameObject.SetActive(true);
				acceptButton.gameObject.SetActive(false);
				dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
				if(type == "c")
					dialogText.text = "Can't start the capsule machine, you don't have enough coins.";
				if(type == "p")
					dialogText.text = "Can't start the capsule machine, you don't have enough points.";
			} else {
				string[] r = result.Split(';');
				itemId = r[0];
				itemRarity = r[1];
				await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/luck.php?key=!switkey&add&username="+Game.Info.user);
				StartCoroutine(animCapsules());
				string json = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/items.json");
				json = fixItemListJson(json);
				ItemList list = JsonUtility.FromJson<ItemList>(json);
				List<Item> items = list.itemList;
				foreach (Item item in items)
				{
					if(item.id.ToString() == itemId) 
						itemName = item.name;
				}
			}
		} catch {
			blackScreen.enabled = true;
			dialogBox.gameObject.SetActive(true);
			refuseButton.gameObject.SetActive(true);
			acceptButton.gameObject.SetActive(false);
			dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
			dialogText.text = "Error while starting the capsule machine, please try again.";
		}
	}
	
	IEnumerator animCapsules() 
	{
		blackScreen.enabled = false;
		dialogBox.gameObject.SetActive(false);
		refuseButton.gameObject.SetActive(false);
		acceptButton.gameObject.SetActive(false);
		for (int i = 1; i <= 8; i++)
		{
			capsulesImage.sprite = Resources.Load<Sprite>("Game/Capsules/Capsules/"+i);
			yield return new WaitForSeconds(0.04f);
		}
		yield return new WaitForSeconds(0.5f);
		StartCoroutine(animResult());
		if(itemRarity == "4" || itemRarity == "5")
			StartCoroutine(animLucky());
	}
	
	IEnumerator animLucky() 
	{
		for (int i = 1; i <= 10; i++)
		{
			typeImage.sprite = luckyBlue;
			yield return new WaitForSeconds(0.2f);
			typeImage.sprite = luckyYellow;
			yield return new WaitForSeconds(0.2f);
		}
		if(id == 0)
			typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/weapons");
		if(id == 1)
			typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/parts");
		if(id == 2)
			typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/items");
		if(id == 3)
			typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/accessories");
	}
	
	IEnumerator animResult() 
	{
		blackScreen.enabled = true;
		resultImage.enabled = true;
		capsuleResultImage.enabled = true;
		for (int i = 1; i <= 28; i++)
		{
			capsuleResultImage.sprite = Resources.Load<Sprite>("Game/Capsules/Result/"+i);
			yield return new WaitForSeconds(0.06f);	
		}
		capsuleResultImage.enabled = false;
		capsuleItemImage.enabled = false;
		closeButton.gameObject.SetActive(true);
		capsuleItemImage.sprite = Resources.Load<Sprite>("Game/Items/"+itemId);
		itemText.text = "["+itemName+"]";
		
		capsuleItemImage.enabled = true;
		Debug.Log("Item: "+itemId+" "+itemRarity);
	}
	
	void closeResult() {
		blackScreen.enabled = false;
		resultImage.enabled = false;
		capsuleResultImage.enabled = false;
		capsuleItemImage.enabled = false;
		closeButton.gameObject.SetActive(false);
		itemText.text = null;
		reloadCapsule();
	}
	
	void changeId(string d) {
		reloadCapsule();
		leftButton.gameObject.SetActive(true);
		rightButton.gameObject.SetActive(true);
		leftImage.enabled = true;
		rightImage.enabled = true;
		Debug.Log("old id: "+id);
		switch(id) {
			case 0:
				if(d == "left") {
					leftImage.sprite = Resources.Load<Sprite>("Game/Capsules/parts-left-a");
					rightImage.sprite = Resources.Load<Sprite>("Game/Capsules/weapons-right");
					typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/parts");
					id = 1;
				}
				Debug.Log("new id: "+id);
				break;
			case 1:
				if(d == "left") {
					leftImage.sprite = Resources.Load<Sprite>("Game/Capsules/items-left-a");
					rightImage.sprite = Resources.Load<Sprite>("Game/Capsules/parts-right");
					typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/items");
					id = 1;
				}
				if(d == "right") {
					leftImage.sprite = Resources.Load<Sprite>("Game/Capsules/parts-left");
					rightImage.sprite = Resources.Load<Sprite>("Game/Capsules/weapons-right-a");
					typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/weapons");
					id = 0;
				}
				Debug.Log("new id: "+id);
				break;
			case 2:
				if(d == "left") {
					leftImage.sprite = Resources.Load<Sprite>("Game/Capsules/accessories-left-a");
					rightImage.sprite = Resources.Load<Sprite>("Game/Capsules/items-right");
					typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/accessories");
					id = 2;
				}
				if(d == "right") {
					leftImage.sprite = Resources.Load<Sprite>("Game/Capsules/parts-left-a");
					rightImage.sprite = Resources.Load<Sprite>("Game/Capsules/items-right");
					typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/perts");
					id = 1;
				}
				Debug.Log("new id: "+id);
				break;
			case 3:
				if(d == "left") {
					//leftButton.gameObject.SetActive(false);
					//leftImage.enabled = false;
					rightImage.sprite = Resources.Load<Sprite>("Game/Capsules/accessories-right-a");
					typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/items");
					id = 3;
				}
				if(d == "right") {
					leftImage.sprite = Resources.Load<Sprite>("Game/Capsules/accessories-left");
					rightImage.sprite = Resources.Load<Sprite>("Game/Capsules/items-right-a");
					typeImage.sprite = Resources.Load<Sprite>("Game/Capsules/items");
					id = 2;
				}
				Debug.Log("new id: "+id);
				break;
		}
	}
	
	async void loadItemsList() {
		itemsListImage.gameObject.SetActive(true);
		blackScreen.enabled = true;
		WebClient client = new WebClient();
		string result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/capsuleItems.php?id="+id);
		string[] a = result.Split(';');
		string i = a[0];
		string r = a[1];
		
		string[] items = i.Split(',');
		string[] rarity = r.Split(',');
		
		int ri = 1;
		int ci = 1;
		for(int x = 0; x < items.Length; x++) {
			try {
				if(rarity[x] == "4" || rarity[x] == "5") {
					GameObject rareObject = GameObject.Find("Rare Item "+ri); 
					Image rareImage = rareObject.GetComponent<Image>();
					rareImage.sprite = Resources.Load<Sprite>("Game/Items/"+items[x]);
					ri++;
				} else {
					GameObject commonObject = GameObject.Find("Common Item "+ci); 
					Image commonImage = commonObject.GetComponent<Image>();
					commonImage.sprite = Resources.Load<Sprite>("Game/Items/"+items[x]);
					ci++;
				}
			} catch {}
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
    }
}
