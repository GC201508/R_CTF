using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*	-	-	-	-	-	-	-	-
 
	・ゲームが終了するまで記録し続けるべき情報を総括するスクリプト.
	・各Configで必要になる関数を作りましょ.

＜Entry＞
	・ゲームに参加するプレイヤーを記録する（初回のみ）.
 
＜SelectStage(ScoreConfig)＞
	・プレイヤー別のスコアを表示させる(SocreConfig).

＜Stage＞
	・参加プレイヤーをStageConfig側で作成する.
	・ゴールしたプレイヤーを取得する => Resultで必要なため.
	・プレイヤー同士で衝突したプレイヤーを取得する => 同上.

＜Result＞
	・旗取ったプレイヤーに得点を加算する.
	・仲良死ボーナス(プレイヤー同士で爆発)で少し得点追加.
	・その他変動なし.


 -	-	-	-	-	-	-	-	-*/


public class GameConfig : MonoBehaviour {

	public bool[] isEntryPlayer = new bool[4] {false,false,false,false};	//参加プレイヤー.
	int[] playerScore = new int[4] {1000,2000,3000,4000};	//各プレイヤーのスコア.

	// Use this for initialization
	void Start () {

		//このオブジェクトはシーン遷移で破棄されない.
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*ゲームに参加するプレイヤーを決める。
	 * EntryConfigが使用する*/
	public void SetEntryPlayer(int playerNumber)
	{	
	
		isEntryPlayer[playerNumber - 1] = true;
	}

	/*ゲームに参加の有無を取得する。 
	 * EntryConfig・StageConfigが使用する*/
	public bool IsEntryPlayer(int playerNumber) {return isEntryPlayer[playerNumber - 1];}

	/*指定したプレイヤーのスコアを返す。
	 * ScoreConfigが使用する.		*/
	public int GetPlayerScore(int playerNumber) {return playerScore[playerNumber - 1];}
	

	/*指定したプレイヤーのスコアを任意の値で足す(負の値で引ける)。
	 * ResultConfigで使用する.				*/
	public void AddPlayerScore(int playerNumber,int scoreNum)
	{
		playerScore[playerNumber - 1] += scoreNum;
	}
}
