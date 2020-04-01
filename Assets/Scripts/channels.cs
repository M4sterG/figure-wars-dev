﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Game;

public class channels : MonoBehaviour
{
	public Image serverLabel1, serverLabel2, serverStatus1, serverStatus2, channelLabel, blackScreen, dialogBox, newsImage;
	public Text serverName1, serverName2, channelName, dialogText, welcomeText;
	public Sprite online, offline, busy, full, selected, unselected;
	public Button serverButton1, serverButton2, acceptButton, refuseButton, joinButton, cancelButton, channelButton;
	
	static string channel1, channel2;
	static int currentId, isChannelSelected, channelStatus1, channelStatus2;
	
	async void Start()
    {
		loadChannels();
    }
	
	async void loadChannels() {
		welcomeText.text = "Welcome on ToyShooters "+Game.Info.user+",\nPlease select a channel to play.";
		serverButton1.onClick.AddListener(delegate{chooseServer(1);});
		serverButton2.onClick.AddListener(delegate{chooseServer(2);});
		channelButton.onClick.AddListener(selectChannel);
		
		refuseButton.onClick.AddListener(delegate{dialogBox.gameObject.SetActive(false);});
		refuseButton.onClick.AddListener(delegate{blackScreen.gameObject.SetActive(false);});
		
		joinButton.onClick.AddListener(join);
		cancelButton.onClick.AddListener(cancel);
        try
        {
			WebClient client = new WebClient();
			string json = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/channels.php?key=!switkey");
            json = fixItemListJson(json);
            Debug.Log(json);
            ItemList list = JsonUtility.FromJson<ItemList>(json);
            List<Item> items = list.itemList;
			int i = 1;
			var labels = new[] {serverLabel1, serverLabel2};
			var names = new[] {serverName1, serverName2};
			var status = new[] {serverStatus1, serverStatus2};
			var cstatus = new[] {channelStatus1, channelStatus2};
            foreach (Item item in items)
            {
				int players = 0;
				try {
					if(i == 1) {
						string result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/connectedPlayers.php?key=!switkey");
						players = Int32.Parse(result);
					}
					labels[i-1].gameObject.SetActive(true);
					names[i-1].text = item.server+"\n"+players+"/"+item.max_players;
					status[i-1].sprite = online;
					if(i == 1) cstatus[i-1] = 0;
					if(i == 1) channel1 = item.channel;
					if(i == 2) channel2 = item.channel;
					Debug.Log(item.channel);
					if(item.maintenance == 1) 
						status[i-1].sprite = busy;
						if(i == 1) channelStatus1 = 1;
					if(players >= item.max_players)
						status[i-1].sprite = full;
						if(i == 1) channelStatus1 = 2;
					//cstatus[i-1] = 2;
					i++;
				} catch {
					status[i-1].sprite = offline;
					if(i == 1) channelStatus1 = 3;
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
        public string channel;
        public string server;
        public int max_players;
		public int maintenance;

        public Item(int id, string channel, string server, int max_players, int maintenance)
        {
            this.id = id;
            this.channel = channel;
            this.server = server;
            this.max_players = max_players;
			this.maintenance = maintenance;
        }
    }
	
	void chooseServer(int id) {
		isChannelSelected = 0;
		channelLabel.gameObject.SetActive(true);
		serverLabel1.sprite = unselected;
		serverLabel2.sprite = unselected;
		channelLabel.sprite = unselected;
		serverName1.color = Color.white;
		serverName2.color = Color.white;
		channelName.color = Color.white;
		if(id == 1) {
			serverLabel1.sprite = selected;
			serverName1.color = Color.yellow;
			channelName.text = channel1;
			currentId = 1;
		}
		if(id == 2) {
			serverLabel2.sprite = selected;
			serverName2.color = Color.yellow;
			channelName.text = channel2;
			currentId = 2;
		}
	}
	
	void selectChannel() {
		isChannelSelected = 1;
		channelLabel.sprite = selected;
		channelName.color = Color.yellow;
	}
	
	async void join() {
		WebClient client = new WebClient();
		if(isChannelSelected == 0) {
			blackScreen.gameObject.SetActive(true);
			dialogBox.gameObject.SetActive(true);
			refuseButton.gameObject.SetActive(true);
			acceptButton.gameObject.SetActive(false);
			dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
			dialogText.text = "Please select a channel before joining a server.";
		} else {
			blackScreen.gameObject.SetActive(true);
			dialogBox.gameObject.SetActive(true);
			refuseButton.gameObject.SetActive(false);
			acceptButton.gameObject.SetActive(false);
			dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/white");
			dialogText.text = "Connecting to the server...";
			string r = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/getPlayerRank.php?key=!switkey&username="+Game.Info.user);
			if(currentId == 1) {
				if(channelStatus1 == 1) {
					blackScreen.gameObject.SetActive(true);
					dialogBox.gameObject.SetActive(true);
					refuseButton.gameObject.SetActive(true);
					acceptButton.gameObject.SetActive(false);
					dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
					dialogText.text = "This server is in maintenance, you can't join.";	
					if(r == "1") {
						blackScreen.gameObject.SetActive(true);
						dialogBox.gameObject.SetActive(true);
						refuseButton.gameObject.SetActive(false);
						acceptButton.gameObject.SetActive(false);
						dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/white");
						dialogText.text = "You are developer, bypassing the maintenance...";
						SceneManager.LoadScene("menu");
					}	
				}
				else if(channelStatus1 == 2) {
					blackScreen.gameObject.SetActive(true);
					dialogBox.gameObject.SetActive(true);
					refuseButton.gameObject.SetActive(true);
					acceptButton.gameObject.SetActive(false);
					dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
					dialogText.text = "This server is full, you can't join.";	
					if(r == "1") {
						blackScreen.gameObject.SetActive(true);
						dialogBox.gameObject.SetActive(true);
						refuseButton.gameObject.SetActive(false);
						acceptButton.gameObject.SetActive(false);
						dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/white");
						dialogText.text = "You are developer, bypassing the full limit...";
						SceneManager.LoadScene("menu");
					}	
				}
				else if(channelStatus1 == 3) {
					blackScreen.gameObject.SetActive(true);
					dialogBox.gameObject.SetActive(true);
					refuseButton.gameObject.SetActive(true);
					acceptButton.gameObject.SetActive(false);
					dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
					dialogText.text = "This server is offline, you can't join.";	
				} else {
					SceneManager.LoadScene("menu");
				}
			} else if(currentId == 2) {
				if(r == "1") {
					SceneManager.LoadScene("map_test");
				} else {
					blackScreen.gameObject.SetActive(true);
					dialogBox.gameObject.SetActive(true);
					refuseButton.gameObject.SetActive(true);
					acceptButton.gameObject.SetActive(false);
					dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
					dialogText.text = "This server is only accessible to ToyShooters developers.";					
				}
			}
			
		}
	}
	
	void cancel() {
		channelLabel.gameObject.SetActive(false);
		isChannelSelected = 0;
		serverLabel1.sprite = unselected;
		serverLabel2.sprite = unselected;
		channelLabel.sprite = unselected;
		serverName1.color = Color.white;
		serverName2.color = Color.white;
		channelName.color = Color.white;
	}
}
