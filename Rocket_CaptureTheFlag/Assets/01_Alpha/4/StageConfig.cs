using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
/*	-	-	-	-	-	-	-	-	-	-	-	-	-
 
	・エントリーしたプレイヤーを生成す.
	・ゴール後のシーン遷移.

	＜予定＞
	・Rocketらがゴールせずに全滅した際の処理.
	
 
 -	-	-	-	-	-	-	-	-	-	-	-	-	*/
public class StageConfig : MonoBehaviour
{

	public bool testModel = false;  //開発用の動作確認をする時にtrueにしてね.

	public GameObject sceneManager; //シーンマネジャ.
	public GameObject rocket;       //Rocketのプレファブ.
	public Sprite[] rocketColor;    //Rocket4色. 
	
	


	// Use this for initialization
	void Start()
	{
		GameObject[] sortSPoint = GameObject.FindGameObjectsWithTag("StartPoint"); //スタートポイント.
		GameObject[] startPoint = new GameObject [4];	//ソート後のスタートポイントを格納.
		Regex re = new Regex(@"[^1-4]");	//文字列から数字(1~4)だけ取り出す.

		foreach (GameObject obj in sortSPoint)
		{
			int index = int.Parse(re.Replace(obj.name,""));	//オブジェクトの名前から数値のみ取得.
			startPoint[index - 1] = obj;	//数値通りに配列に追加.
		}

		//テストモードじゃない時.
		if (!testModel)
		{
			GameObject gameConfig = GameObject.FindGameObjectWithTag("GameConfig"); //タグからGameConfigを取得す.

			//エントリーしたプレイヤーのみ生成する.
			for (int i = 0; i < 4; i++)
			{

				int playerNum = i + 1; //プレイヤー番号.

				//プレイヤーが不参加の場合はコンティニュする.
				if (!gameConfig.GetComponent<GameConfig>().IsEntryPlayer(playerNum))
				{
					Destroy(startPoint[i]); //スタートポイントを消す.
					continue;
				}

				SpawnRocket(startPoint[i], playerNum);//rocketをスタートポイントから生成する.
				Destroy(startPoint[i]); //スタートポイントを消す.
			}
		}

		//テストモードの時.
		else
		{
			//全員生成する.
			for (int i = 0; i < 4; i++)
			{

				int playerNum = i + 1; //プレイヤー番号.

				SpawnRocket(startPoint[i], playerNum);//rocketをスタートポイントから生成する.
				Destroy(startPoint[i]); //スタートポイントを消す.
			}

		}

	}

	// Update is called once per frame
	void Update()
	{
		NakayoDieRocket();
		GoalRocket();
		AllExplosionRocket();
	}

	/*スタートポイントからRocketを生成する*/
	void SpawnRocket(GameObject sPoint, int playernum)
	{
		GameObject obj = Instantiate(rocket, sPoint.transform.position, sPoint.transform.localRotation); //プレハブからインスタンスを生成.
		obj.GetComponent<R_move>().SetJoystickNumber(playernum);    //rocketにプレイヤー番号を指定.
		obj.GetComponent<SpriteRenderer>().sprite = rocketColor[playernum - 1];//rocketに色をつける.
		obj.name = "Player_" + playernum + "P"; //オブジェクトの名前を変更する.

	}

	/*Rocketがゴールした場合に行う処理*/
	void GoalRocket()
	{
		GameObject[] Player = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject p in Player)
		{
			
			//該当プレイヤーがゴールした.
			if (p.GetComponent<R_Goal>().IsRocketGoal())
			{
				GameObject gameConfig = GameObject.FindGameObjectWithTag("GameConfig"); //タグからGameConfigを取得す.
				int goalPlayerNumber = p.GetComponent<R_move>().GetJoystickNumber();　 //プレイヤー番号を取得.
				gameConfig.GetComponent<GameConfig>().OnBounusGoal(goalPlayerNumber);  //該当プレイヤーにゴールボーナスを設定.
				sceneManager.SetActive(true); //シーンを移動する.
			}
			
		}
	}

	/*Rocketがプレイヤーと衝突した場合に行う処理*/
	void NakayoDieRocket()
	{
		GameObject[] Player = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject p in Player)
		{
			
			//該当プレイヤーが仲良死した.
			if (p.GetComponent<R_Explosion>().IsRocketNakayoDie())
			{
				GameObject gameConfig = GameObject.FindGameObjectWithTag("GameConfig"); //タグからGameConfigを取得す.
				int goalPlayerNumber = p.GetComponent<R_move>().GetJoystickNumber();　 //プレイヤー番号を取得.
				gameConfig.GetComponent<GameConfig>().OnBounusNakayoDie(goalPlayerNumber);  //該当プレイヤーに仲良死ボーナスを設定.
			}
			
		}
	}

	/*Rocketが全滅した時にシーン移動する処理*/
	void AllExplosionRocket()
	{
		GameObject[] Player = GameObject.FindGameObjectsWithTag("Player");
		if(Player.Length == 0)
		{
			Invoke("InvokeActiveSceneManager",3.0f);
		}
	}

	/*Invoke用シーン移動*/
	void InvokeActiveSceneManager()
	{
		sceneManager.SetActive(true); //シーンを移動する.
	}

}
