using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*	-	-	-	-	-	-	-	-	-
 
	・遊ぶプレイヤーを決める.

 -	-	-	-	-	-	-	-	-	*/

public class EntryConfig : MonoBehaviour
{

	public GameObject[] rocket;
	public GameObject sceneManager;

	GameConfig gcSprict;

	// Use this for initialization
	void Start()
	{
		foreach (GameObject go in rocket)
		{
			//選択されてない状態の色は灰色で透けさせる.
			go.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.5f, 0.5f, 0.7f);
		}

		//GameConfigオブジェクト内のスクリプトを取得する.
		gcSprict = GameObject.FindGameObjectWithTag("GameConfig").GetComponent<GameConfig>(); //タグからGameConfigを取得す.

	}

	// Update is called once per frame
	void Update()
	{
		EntryPlayer();
		GameStart();
	}

	/*エントリーしたRocketの色を戻す*/	
	void ReColor(GameObject go)
	{
		go.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
	}

	/*ゲームに参加するプレイヤーを決める*/
	void EntryPlayer()
	{
		//1pのエントリー(STARTボタン(JoystickButton7))を押して参加. 
		if (!gcSprict.IsEntryPlayer(1) && Input.GetKeyDown(KeyCode.Joystick1Button7))
		{
			ReColor(rocket[0]);
			gcSprict.SetEntryPlayer(1);
		}

		//2p
		if (!gcSprict.IsEntryPlayer(2) && Input.GetKeyDown(KeyCode.Joystick2Button7))
		{
			ReColor(rocket[1]);
			gcSprict.SetEntryPlayer(2);
		}

		//3p
		if (!gcSprict.IsEntryPlayer(3) && Input.GetKeyDown(KeyCode.Joystick3Button7))
		{
			ReColor(rocket[2]);
			gcSprict.SetEntryPlayer(3);
		}

		//4p
		if (!gcSprict.IsEntryPlayer(4) && Input.GetKeyDown(KeyCode.Joystick4Button7))
		{
			ReColor(rocket[3]);
			gcSprict.SetEntryPlayer(4);
		}
	}

	/*エントリーが完了したプレイヤーがAボタン（JoystickButton0）を押すとシーンを移動する*/
	void GameStart()
	{
		bool isStart = false;
		//1p
		if (gcSprict.IsEntryPlayer(1) && Input.GetKeyDown(KeyCode.Joystick1Button0))
		{
			isStart = true;
		}

		//2p
		if (gcSprict.IsEntryPlayer(2) && Input.GetKeyDown(KeyCode.Joystick2Button0))
		{
			isStart = true;
		}

		//3p
		if (gcSprict.IsEntryPlayer(3) && Input.GetKeyDown(KeyCode.Joystick3Button0))
		{
			isStart = true;
		}

		//4p
		if (gcSprict.IsEntryPlayer(4) && Input.GetKeyDown(KeyCode.Joystick4Button0))
		{
			isStart = true;
		}


		if (isStart)
		{
			sceneManager.SetActive(true);
		}
	}

}
