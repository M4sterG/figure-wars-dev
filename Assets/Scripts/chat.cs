﻿using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Game;

public class chat : MonoBehaviour {

	public Text message_text_6, message_text_5, message_text_4, message_text_3, message_text_2, message_text_1, nicknameText, toggleText, globalButtonText, clanButtonText, announcementButtonText;
	public InputField chat_input;
	public Button sendButton, toogleButton, globalButton, clanButton, announcementButton;
	public Image chatBox, chatHeader;

	static string cMessage_1, cMessage_2, cMessage_3, cMessage_4, cMessage_5, cMessage_6;
	static int chatType = 0;
	
    void Start()
    {
	    /*
		InvokeRepeating("loopChat", 5f, 5f);
		loadChat("global",1);
			
		toogleButton.onClick.AddListener(toogleChat);
		sendButton.onClick.AddListener(sendChat);
		
		globalButton.onClick.AddListener(delegate{switchChat("global");});
		clanButton.onClick.AddListener(delegate{switchChat("clan");});
		announcementButton.onClick.AddListener(delegate{switchChat("announcements");}); */
    }
	
	public class MessageInfo {
		public string message, user, date;
	}

	public class NewsInfo {
		public string message, date;
	}
	
	void loopChat() {
		if(chatType == 0) {
			loadChat("global",0);
		}
	}
	
	async void loadChat(string type, int firstLoad) 
	{
		try {
			clanButtonText.color = new Color(183,178,186);
			globalButtonText.color = new Color(183,178,186);
			announcementButtonText.color = new Color(183,178,186);
			
			if(firstLoad == 1) {
				message_text_1.text = null;
				message_text_2.text = null;
				message_text_3.text = null;
				message_text_4.text = null;
				message_text_5.text = null;
				message_text_6.text = null;
			}
			
			WebClient client = new WebClient();
			var arr = new[] {message_text_1, message_text_2, message_text_3, message_text_4, message_text_5, message_text_6};
			var msgs = new[] {cMessage_1, cMessage_2, cMessage_3, cMessage_4, cMessage_5, cMessage_6};
			if(type == "global") {
				if(firstLoad == 1) message_text_6.text = "Loading the chat, please wait...";
				globalButtonText.color = Color.yellow;
				for(int i = 1; i <= 6; i++) {
					string result = await client.DownloadStringTaskAsync("http://remakenetwork.000webhostapp.com/api/toyshooters/chat.php?key=!switkey&func=get&id="+i);
					var json = JsonUtility.FromJson<MessageInfo>(result);
					msgs[i-1] = "["+json.date+"]"+" "+json.user+": "+json.message;
					if(json.user.Contains("[GM]") || json.user.Contains("[ADMIN]")) msgs[i-1] = "["+json.date+"]"+" <color=#E55451ff>"+json.user+"</color>: "+json.message;
					if(json.user.Contains("[MOD]")) msgs[i-1] = "["+json.date+"]"+" <color=#ffa500ff>"+json.user+"</color>: "+json.message;
					//if(firstLoad == 1) message_text_6.text = "Loading the chat, please wait... ("+i+"/6)";
				}
				for(int i = 0; i <= 5; i++) {
					try {
						arr[i].text = msgs[i];		
					} catch {
						Debug.Log("Error");
					}
				}
			}
			else if(type == "clan") {
				clanButtonText.color = Color.yellow;
				message_text_6.text = "Hello "+nicknameText.text+", the clan chat is not enabled yet.";			
			}
			else if(type == "announcements") {
				if(firstLoad == 1) message_text_6.text = "Loading news, please wait...";
				announcementButtonText.color = Color.yellow;
				for(int i = 1; i < 7; i++) {
					string result = await client.DownloadStringTaskAsync("http://remakenetwork.000webhostapp.com/api/toyshooters/news.php?key=!switkey&id="+i);
					var json = JsonUtility.FromJson<MessageInfo>(result);
					arr[i-1].text = "["+json.date+"] "+json.message;
				}		
			}
		} catch {
			message_text_6.text = "Error while loading, please try later.";
		}
	}
	
	void sendChat() 
	{
		string message = chat_input.text;
		chat_input.text = "";
		WebClient client = new WebClient();
		client.DownloadStringAsync(new Uri("http://remakenetwork.000webhostapp.com/api/toyshooters/chat.php?key=!switkey&func=post&user="+Game.Info.nick+"&message="+message));
		loadChat("global",0);
	}
	
	void switchChat(string type)
	{
		switch(type) {
			case "global":
				chatType = 0;
				loadChat("global",1);
				break;
			case "clan":
				chatType = 1;
				loadChat("clan",1);
				break;
			case "announcements":
				chatType = 2;
				loadChat("announcements",1);
				break;				
		}
	}
	
	int s = 0;
	void toogleChat() {
		var texts = new[] {message_text_1, message_text_2, message_text_3, message_text_4, message_text_5, message_text_6,};
		var buttons = new[] {sendButton, globalButton, clanButton, announcementButton}; 
		if(s == 0) {
			s = 1;
			for(int i = 0; i < texts.Length; i++) {
				texts[i].gameObject.SetActive(false);
			}
			for(int i = 0; i < buttons.Length; i++) {
				buttons[i].gameObject.SetActive(false);
			}
			chat_input.gameObject.SetActive(false);
			chatBox.gameObject.SetActive(false);
			chatHeader.gameObject.SetActive(false);
			toggleText.color = Color.green;
		}
		else {
			s = 0;
			for(int i = 0; i < texts.Length; i++) {
				texts[i].gameObject.SetActive(true);
			}
			for(int i = 0; i < buttons.Length; i++) {
				buttons[i].gameObject.SetActive(true);
			}
			chat_input.gameObject.SetActive(true);
			chatBox.gameObject.SetActive(true);
			chatHeader.gameObject.SetActive(true);
			toggleText.color = Color.red;
		}
	}
}
