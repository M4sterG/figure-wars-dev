using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class navbar : MonoBehaviour
{
	public Button LobbyButton, InventoryButton, ShopButton, CapsuleButton, RankingButton;
    void Start()
    {
        LobbyButton.onClick.AddListener(delegate{SceneManager.LoadScene("menu");});
		InventoryButton.onClick.AddListener(delegate{SceneManager.LoadScene("inventory");});
		ShopButton.onClick.AddListener(delegate{SceneManager.LoadScene("shop");});
		CapsuleButton.onClick.AddListener(delegate{SceneManager.LoadScene("capsule");});
		RankingButton.onClick.AddListener(delegate{SceneManager.LoadScene("ranking");});
		
    }
}
