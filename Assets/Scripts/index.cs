using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Game {
	
	public static class Info {
		public static string user = "";
		public static string nick = "";
		public static string version = "BETA_1.0.0";
		public static int isUpdated = 0;
		public static int isFirstLoad = 0;
	}
	
	public static class API {
		public static string API_ADDR = "http://remake-network.cf";
		public static string API_PORT = "80";
		public static string API_KEY = "!switkey";
	}
	
	public class index : MonoBehaviour
	{
		public InputField usernameInput, passwordInput;
		public Image blackScreen, levelImage, connectionImage, levelBarImage, dialogBox;
		public Button registerButton, loginButton, acceptButton, refuseButton;
		public Text nicknameText, pointsText, coinsText, statsText, connectionText, levelBarText, versionText, dialogText;
		public Sprite connectionGBar, connectionOBar, connectionRBar;
		
		public class PlayerStats {
			public static string nickname, points, coins, level, xp, stats, percentage;
		}

		public class PlayerInfo {
			public int coins, points, kills, deaths, assists, level, xp;
			public string nickname;
		}
		
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				Scene currentScene = SceneManager.GetActiveScene();
				if(currentScene.name == "login")
					login();
			}
		}
		
		async void Start()
		{		
			Screen.SetResolution(1024, 768, false);
		
			Scene currentScene = SceneManager.GetActiveScene();
			
			if(currentScene.name == "login") 
			{				
				loginButton.onClick.AddListener(login);
				refuseButton.onClick.AddListener(delegate{dialogBox.gameObject.SetActive(false);});
				refuseButton.onClick.AddListener(delegate{blackScreen.gameObject.SetActive(false);});
				
				versionText.text = "VERSION "+Info.version;
				WebClient client = new WebClient();
				string v = await client.DownloadStringTaskAsync(API.API_ADDR+":"+API.API_PORT+"/api/toyshooters/version");		
				if(v != Info.version) {
					Info.isUpdated = 0;
					return;
				}
				Info.isUpdated = 1;
			}
			
			if(currentScene.name == "menu" && Info.isFirstLoad == 0) 
			{
				Info.isFirstLoad = 1;
				setPlayerStats();
				
			}
			
			if(currentScene.name == "menu" || currentScene.name == "channels" || currentScene.name == "inventory" || currentScene.name == "shop" || currentScene.name == "capsule" || currentScene.name == "ranking") {
				if(Info.isFirstLoad == 0) {
					return;
				}
				if(currentScene.name == "channels" || currentScene.name == "inventory" || currentScene.name == "shop" || currentScene.name == "capsule" || currentScene.name == "ranking")
					loadPlayerStats();
				setPlayerStats();
			}
			
			if(currentScene.name == "menu") {
				WebClient client = new WebClient();
				string p = await client.DownloadStringTaskAsync(API.API_ADDR+":"+API.API_PORT+"/api/getPing.php");
				if(Int32.Parse(p) <= 100) {
					connectionImage.sprite = connectionGBar;
				} else if(Int32.Parse(p) < 200) {
					connectionImage.sprite = connectionOBar;				
				} else {
					connectionImage.sprite = connectionRBar;
				}
				connectionText.text = "Server : Europe ("+p+"ms)";
			}
		}
		
		async void OnApplicationQuit() {
			WebClient client = new WebClient();
			await client.DownloadStringTaskAsync(API.API_ADDR+":"+API.API_PORT+"/api/toyshooters/connected.php?key="+API.API_KEY+"&username="+Info.user+"&disconnect");
		}

		async void login() 
		{
			try {
				WebClient client = new WebClient();
				blackScreen.gameObject.SetActive(true);
				dialogBox.gameObject.SetActive(true);
				refuseButton.gameObject.SetActive(false);
				acceptButton.gameObject.SetActive(false);
				dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/white");
				dialogText.text = "Connecting to the server...";
				string username = usernameInput.text;
				string password = passwordInput.text;
				string result = await client.DownloadStringTaskAsync(API.API_ADDR+":"+API.API_PORT+"/api/toyshooters/api.php?key="+API.API_KEY+"&username="+username+"&password="+password);
				if (result == "ALLOWED" && Info.isUpdated != 0) {
					result = await client.DownloadStringTaskAsync(API.API_ADDR+":"+API.API_PORT+"/api/toyshooters/connected.php?key="+API.API_KEY+"&username="+username+"&check");
					if(result == "1") {
						blackScreen.gameObject.SetActive(true);
						dialogBox.gameObject.SetActive(true);
						dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
						dialogText.text = "You are already connected to the server.";
						refuseButton.gameObject.SetActive(true);
					} else {
						connect(username);
						await client.DownloadStringTaskAsync(API.API_ADDR+":"+API.API_PORT+"/api/toyshooters/connected.php?key="+API.API_KEY+"&username="+Info.user+"&connect");
					}
				}	
				else {
					blackScreen.gameObject.SetActive(true);
					dialogBox.gameObject.SetActive(true);
					dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
					dialogText.text = "Error, your username or your password is invalid.";
					if(Info.isUpdated == 0)
						dialogText.text = "Error, your game is not updated.";
					refuseButton.gameObject.SetActive(true);
				}
			} catch {
				blackScreen.gameObject.SetActive(true);
				dialogBox.gameObject.SetActive(true);
				dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
				dialogText.text = "Can't connect to the server, please try again.";
				acceptButton.gameObject.SetActive(false);
				refuseButton.gameObject.SetActive(true);
			}
		}
		
		void connect(string username) {
			Info.user = username;
			SceneManager.LoadScene("channels");
		}
		
		async void setPlayerStats() {
			WebClient client = new WebClient();
			string r = await client.DownloadStringTaskAsync(API.API_ADDR+":"+API.API_PORT+"/api/toyshooters/getPlayerStats.php?key="+API.API_KEY+"&username="+Info.user);
			var json = JsonUtility.FromJson<PlayerInfo>(r);
			string p = await client.DownloadStringTaskAsync(API.API_ADDR+":"+API.API_PORT+"/api/toyshooters/getPercentageofLevel.php?key="+API.API_KEY+"&xp="+json.xp+"&level="+json.level);
			float kda = 0;
			if(json.deaths != 0)
				kda = (json.kills / json.deaths);
			if(json.deaths == 0 || json.kills == 0)
				kda = 0;
			PlayerStats.stats = "KILLS "+json.kills.ToString()+" / DEATHS "+json.deaths.ToString()+" / ASSISTS "+ json.assists.ToString() + " / KD "+kda;
			PlayerStats.percentage = p;
			PlayerStats.nickname = json.nickname;
			PlayerStats.level = json.level.ToString();
			PlayerStats.points = json.points.ToString("#,##0");
			PlayerStats.coins = json.coins.ToString("#,##0");
			
			Info.nick = json.nickname;
			
			loadPlayerStats();
		}
		
		void loadPlayerStats() {
			try {
				levelImage.sprite = Resources.Load<Sprite>("Game/Levels/"+PlayerStats.level);
				statsText.text = PlayerStats.stats;
				levelBarText.text = PlayerStats.percentage+"%";
				nicknameText.text = PlayerStats.nickname;
				pointsText.text = PlayerStats.points;
				coinsText.text = PlayerStats.coins;
				
				var rect = levelBarImage.transform as RectTransform;
				rect.sizeDelta = new Vector2 (Int32.Parse(PlayerStats.percentage) * 500 / 100, rect.sizeDelta.y);
			} catch {}
		}

	}
}
