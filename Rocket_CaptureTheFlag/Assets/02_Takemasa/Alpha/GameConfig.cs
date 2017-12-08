using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*	-	-	-	-	-	-	-	-
 
	・ゲームが終了するまで記録し続けるべき情報を総括するスクリプト.
	・各Configで必要になる関数を作りましょ.

＜Entry＞
	・ゲームに参加するプレイヤーを記録する（初回のみ）.
 
＜SelectStage＞
	・プレイヤー別のポイントを表示させる.

＜Stage＞
	・参加プレイヤーをStageConfig側で作成する.

＜Result＞
	・旗取ったプレイヤーに得点を加算する.
	・仲良死ボーナス(プレイヤー同士で爆発)で少し得点追加.
	・その他変動なし.


 -	-	-	-	-	-	-	-	-*/


public class GameConfig : MonoBehaviour {

	bool[] isEntryPlayer = new bool[4] {false,false,false,false};	//参加プレイヤー.
	int[] playerPoint = new int[4] {1000,1000,1000,1000};	//各プレイヤーのポイント.

	// Use this for initialization
	void Start () {

		//このオブジェクトはシーン遷移で破棄されない.
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*ゲームに参加するプレイヤーを決める。 EntryConfigが使用する*/
	public void SetEntryPlayer(int playerNumber)
	{
		isEntryPlayer[playerNumber - 1] = true;
	}

	/*ゲームに参加の有無を取得する。 
	 * EntryConfig・StageConfigが使用する*/
	public bool IsEntryPlayer(int plyerNumber) {return isEntryPlayer[plyerNumber - 1];}
	
	
}
