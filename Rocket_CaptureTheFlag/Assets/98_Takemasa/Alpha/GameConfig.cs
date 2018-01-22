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


public class GameConfig : MonoBehaviour
{

	public bool[] isEntryPlayer = new bool[4] { false, false, false, false };   //参加プレイヤー.
	int[] playerScore = new int[4] { 1000, 2000, 3000, 4000 };  //各プレイヤーのスコア.

	//ボーナス配分を決めるための構造体.
	public struct ScoreBonus
	{
		public int playerNumber;    //プレイヤーのナンバー.
		public bool isGoalBonus;    //ゴールした時にtrue.
		public bool isNakayoDieBounus;  //仲良死した時にtrue.
	}
	//ScoreBounus構造体のリスト.
	List<ScoreBonus> listScoreBounus = new List<ScoreBonus>();

	//Rusultデバッグ用
	public bool RUSULT_DEBUG = false; //trueで適用.


	// Use this for initialization
	void Start()
	{
		Debug.Log("====-------------GameConfigLoad--------------====");
		//Rusultデバッグ用
		if (RUSULT_DEBUG)
		{
			SetEntryPlayer(1);
			SetEntryPlayer(2);
			SetEntryPlayer(3);

			OnBounusGoal(1);
			OnBounusNakayoDie(2);
			OnBounusNakayoDie(3);
		}

		//このオブジェクトはシーン遷移で破棄されない.
		DontDestroyOnLoad(this);
	}

	// Update is called once per frame
	void Update()
	{

	}

	/*ゲームに参加するプレイヤーを決める。
	 *参加プレイヤーのScoreBonusをリストに追加する.	
	 * EntryConfigが使用する*/
	public void SetEntryPlayer(int playerNumber)
	{

		isEntryPlayer[playerNumber - 1] = true;

		ScoreBonus sb;  //リスト追加用変数.
		sb.playerNumber = playerNumber; //エントリーするプレイヤー番号入れる.
		sb.isGoalBonus = false;         //falseで初期化する.
		sb.isNakayoDieBounus = false;   //falseで初期化する.
		listScoreBounus.Add(sb);    //スコボーナスリストに追加する.

	}

	/*ゲームに参加の有無を取得する。 
	 * EntryConfig・StageConfigが使用する*/
	public bool IsEntryPlayer(int playerNumber) { return isEntryPlayer[playerNumber - 1]; }

	/*指定したプレイヤーのスコアを任意の値で足す(負の値で引ける).
	 * ResultConfigで使用する.				*/
	public void AddPlayerScore(int playerNumber, int scoreNum)
	{
		playerScore[playerNumber - 1] += scoreNum;
	}

	/*指定したプレイヤーのスコアを返す.
	 * ScoreConfigが使用する.		*/
	public int GetPlayerScore(int playerNumber) { return playerScore[playerNumber - 1]; }

	/*StageSceneにてゴールしたプレイヤーにボーナスを設定.
	 * StageConfigで使用する.*/
	public void OnBounusGoal(int playerNumber)
	{
		for (int i = 0; i < listScoreBounus.Count; i++)
		{
			ScoreBonus sb = listScoreBounus[i];

			//ボーナス設定するプレイヤーが一致.
			if (sb.playerNumber == playerNumber)
			{
				sb.isGoalBonus = true;  //ゴールボーナスをtrue.
				listScoreBounus[i] = sb; //要素を更新.
				Debug.Log("GameConfig:" + playerNumber + "Pにゴールボーナス");
			}

		}
	}

	/*ボーナスを参照し、ゴールしたプレイヤー番号を返す.
	 *いなかったら0を返す.	
	 * ResultConfigで使う.*/
	public int GetGoalPlayerNumber()
	{
		int returnNumber = 0;  //返す番号;
		foreach (ScoreBonus sb in listScoreBounus)
		{
			if (sb.isGoalBonus)
			{
				returnNumber = sb.playerNumber;
			}
		}
		return returnNumber;
	}

	/*StageSceneにてプレイヤーに衝突したプレイヤーにボーナス設定.
	 * StageConfigで使用する.*/
	public void OnBounusNakayoDie(int playerNumber)
	{
		for (int i = 0; i < listScoreBounus.Count; i++)
		{
			ScoreBonus sb = listScoreBounus[i];

			//ボーナス設定するプレイヤーが一致.
			if (sb.playerNumber == playerNumber && !sb.isNakayoDieBounus)
			{
				sb.isNakayoDieBounus = true;    //仲良死ボーナスをtrue.
				listScoreBounus[i] = sb; //要素を更新.
				Debug.Log("GameConfig:" + playerNumber + "Pに仲良死ボーナス");
			}

		}
	}

	/*引数で指定したプレイヤー番号が仲良死ボーナス得ているか返す.
 	 * ResultConfigのループ式内で使用する。しなさい.*/
	public bool IsBounusNakayoDiePlayer(int playerNumber)
	{
		foreach (ScoreBonus sb in listScoreBounus)
		{
			if (sb.playerNumber == playerNumber)
			{
				return sb.isNakayoDieBounus;
			}
		}

		Debug.Log("GameConfig:" + "listScoreBounus内でreturnが発生しませんでした\n値はfalseを返します.");
		return false;
	}

	/*listScoreBounus内全てのisGoalBounusとisNakayoDieBounusをfalseにする.
	 * ResultConfigで使用する.*/
	public void InitBounus()
	{
		for (int i = 0; i < listScoreBounus.Count; i++)
		{
			ScoreBonus sb;
			sb = listScoreBounus[i];    //sbに要素を渡す(playerNumbarは変更無).
			sb.isGoalBonus = false;
			sb.isNakayoDieBounus = false;
			listScoreBounus[i] = sb; //要素を更新.
		}
	}
}
