using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ranking : MonoBehaviour
{
    public Image playerImage;
    public Text playerNickname, rankingNickname, rankingDescription, rankingClan, rankingKills, rankingDeaths, rankingScore, rankingHeadshots, rankingPlayed, rankingRKills, rankingSKills, rankingSGKills, rankingOKills;
        
    public class PlayerInfo {
		public int coins, points, kills, deaths, assists, level, score, headshots, grenadelauncher_kills, bazooka_kiils, gatling_kills, sniper_kills, shotgun_kills, rifle_kills, melee_kills;
        public float played;
        public string clan, desc, avatar;
	}
    
    void Start()
    {
        searchPlayer(playerNickname.text);
        Debug.Log(playerNickname.text);
    }

    async void searchPlayer(string player) {
        try {
            WebClient client = new WebClient();
            string result = await client.DownloadStringTaskAsync("http://remakenetwork.000webhostapp.com/api/toyshooters/getPlayerStats.php?key=!switkey&username="+player);
            var json = JsonUtility.FromJson<PlayerInfo>(result);
            rankingDescription.text = json.desc;
            rankingClan.text = json.clan;
            string uri = json.avatar;
            StartCoroutine(loadAvatar(uri));
        } catch {
            rankingDescription.text = "error";
        }
    }

    IEnumerator loadAvatar(string uri) {
        WWW www = new WWW(uri);
        yield return www;
        playerImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
    }
}
 