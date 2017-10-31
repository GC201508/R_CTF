using UnityEngine;
using System.Collections;

/*	-	-	-	-	-	-	-	-	-	-	-
 
	・チェックポイントに一番近いRocket(プレイヤー)を取得する,
 
 -	-	-	-	-	-	-	-	-	-	-	*/

public class CameraConfig : MonoBehaviour
{

	GameObject[] playerTagObjs; //シーンにあるRocket,
	GameObject nearRocket;		//チェックポイントに一番近いゲームオブジェクト,
	float min = 9999.9f;

	// Use this for initialization
	void Start()
	{

		//Playerタグのオブジェクトを取得する,
		playerTagObjs = GameObject.FindGameObjectsWithTag("Player");

//http://game.granbluefantasy.jp/#quest/supporter/400181/4

	}

	// Update is called once per frame
	void Update()
	{

		foreach (GameObject obj in playerTagObjs)
		{

		}

	}
}
