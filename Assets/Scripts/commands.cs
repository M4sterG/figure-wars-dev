using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.IO;
using System.Threading;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Game;

public class commands : MonoBehaviour
{
	public Image dialogBox, blackScreen;
	public Button acceptButton, refuseButton;
	public Text dialogText;
	
	static int isKicked;
	
	void Start() {
		InvokeRepeating("checkKick", 2.0f, 10f);
		Scene currentScene = SceneManager.GetActiveScene();
		if(currentScene.name == "login" && isKicked == 1) {
			refuseButton.onClick.AddListener(delegate{dialogBox.gameObject.SetActive(false);});
			refuseButton.onClick.AddListener(delegate{blackScreen.gameObject.SetActive(false);});
			blackScreen.gameObject.SetActive(true);
			dialogBox.gameObject.SetActive(true);
			refuseButton.gameObject.SetActive(true);
			acceptButton.gameObject.SetActive(false);
			dialogBox.sprite = Resources.Load<Sprite>("Game/DialogBoxs/red");
			dialogText.text = "You have been kicked from the server.";
			isKicked = 0;
		}
	}
	async void checkKick() {
		Scene currentScene = SceneManager.GetActiveScene();
		if(currentScene.name == "login") {
			return;
		}
		WebClient client = new WebClient();
		string result = await client.DownloadStringTaskAsync("http://remake-network.cf/api/toyshooters/punish.php?key="+Game.API.API_KEY+"&username="+Game.Info.user+"&check");		
		if(result == "1") {
			isKicked = 1;
			StartCoroutine(kickPlayer());
			return;
		}		
	}
	
	IEnumerator kickPlayer()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("login");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
