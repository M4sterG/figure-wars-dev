using System.Collections;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class misiones : MonoBehaviour
{

	public Image misiones_image;
	public Sprite misiones_sprite1, misiones_sprite2;
    void Start()
    {
        InvokeRepeating("updateAnim", 1f, 2f);
    }
	
	void updateAnim() {
		StartCoroutine(doUpdateAnim());
	}
	
	IEnumerator doUpdateAnim() {
		misiones_image.sprite = misiones_sprite1;
		yield return new WaitForSeconds(1f);
		misiones_image.sprite = misiones_sprite2;
		yield return new WaitForSeconds(1f);
	}
}
