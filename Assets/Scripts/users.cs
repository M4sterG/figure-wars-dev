using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Game;

public class users : MonoBehaviour
{
	public Text user_1, user_2, user_3, user_4, user_5, user_6, user_7, user_8, user_9, user_10, usersText, friendsText, clanText;
	public Image user_1_level, user_2_level, user_3_level, user_4_level, user_5_level, user_6_level, user_7_level, user_8_level, user_9_level, user_10_level, list;
	public Sprite level_1_icon, level_2_icon, level_3_icon, level_4_icon, level_5_icon, level_6_icon, level_7_icon, level_8_icon, level_9_icon, level_10_icon, level_11_icon, level_12_icon, level_13_icon, level_14_icon, level_15_icon, level_16_icon, level_17_icon, level_18_icon, level_19_icon, level_20_icon, level_21_icon, level_22_icon, level_23_icon, level_24_icon, level_25_icon, level_26_icon, level_27_icon, level_28_icon, level_29_icon, level_30_icon, level_31_icon, level_32_icon, level_33_icon, level_34_icon, level_35_icon, level_36_icon, level_37_icon, level_38_icon, level_39_icon, level_40_icon, level_41_icon, level_42_icon, level_43_icon, level_44_icon, level_45_icon, level_46_icon, level_47_icon, level_48_icon, level_49_icon, level_50_icon, level_51_icon, level_52_icon, level_53_icon, level_54_icon, level_55_icon, level_56_icon, level_57_icon, level_58_icon, level_59_icon, level_60_icon, level_61_icon, level_62_icon, level_63_icon, level_64_icon, level_65_icon, level_66_icon, level_67_icon, level_68_icon, level_69_icon, level_70_icon, level_71_icon, level_72_icon, level_73_icon, level_74_icon, level_75_icon, level_76_icon, level_77_icon, level_78_icon, level_79_icon, level_80_icon, UIMask;
	public Button open, close, usersButton, friendsButton, clanButton;
	
	public class PlayerInfo {
		public int level;
		public string nickname;
	}
	
	void Start()
	{
		loadUsers();
		open.onClick.AddListener(openUsers);
		close.onClick.AddListener(closeUsers);
		
		usersButton.onClick.AddListener(loadUsers);
		friendsButton.onClick.AddListener(loadFriends);
		clanButton.onClick.AddListener(loadClan);
	}
	
	async void loadUsers() {
		reset();
		usersText.text = "<b>Lobby</b>";
		WebClient client = new WebClient();
		var users = new[] {user_1, user_2, user_3, user_4, user_5, user_6, user_7, user_8, user_9, user_10};
		var levels = new[] {user_1_level, user_2_level, user_3_level, user_4_level, user_5_level, user_6_level, user_7_level, user_8_level, user_9_level, user_10_level};
		for(int i = 0; i<10; i++) {
			string result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/users.php?key=!switkey&id="+i);
			if(result != "[]" && result != null && result != "null") {
				var json = JsonUtility.FromJson<PlayerInfo>(result);
				users[i].text = json.nickname;
				levels[i].sprite = Resources.Load<Sprite>("Game/Levels/"+json.level);
			}
		}
	}
	
	async void loadFriends() {
		reset();
		friendsText.text = "<b>Friends</b>";
		var users = new[] {user_1, user_2, user_3, user_4, user_5, user_6, user_7, user_8, user_9, user_10};
		var levels = new[] {user_1_level, user_2_level, user_3_level, user_4_level, user_5_level, user_6_level, user_7_level, user_8_level, user_9_level, user_10_level};
		WebClient client = new WebClient();
		string result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/friends.php?key=!switkey&username="+Game.Info.user);
		if(result == "0")
			return;
		string[] friends = result.Split(';');
		for(int i = 0; i < friends.Length; i++) {
			users[i].text = friends[i];
			result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/getPlayerStats.php?key=!switkey&username="+friends[i]);
			var json = JsonUtility.FromJson<PlayerInfo>(result);
			levels[i].sprite = Resources.Load<Sprite>("Game/Levels/"+json.level);
		}
		
	}
	
	async void loadClan() {
		reset();
		user_1.text = "Clan are disabled.";
		clanText.text = "<b>Clan</b>";
	}
	
	void reset() {
		usersText.text = "Lobby";
		friendsText.text = "Friends";
		clanText.text = "Clan";
		var users = new[] {user_1, user_2, user_3, user_4, user_5, user_6, user_7, user_8, user_9, user_10};
		var levels = new[] {user_1_level, user_2_level, user_3_level, user_4_level, user_5_level, user_6_level, user_7_level, user_8_level, user_9_level, user_10_level};
		for(int i = 0; i<10; i++) {
			users[i].text = null;
			levels[i].sprite = UIMask;
		}
	}
	
	void openUsers() {
		list.gameObject.SetActive(true);
	}
	
	void closeUsers() {
		list.gameObject.SetActive(false);
	}
}
