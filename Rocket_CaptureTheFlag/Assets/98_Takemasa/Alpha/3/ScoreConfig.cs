using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*

	・SelectStageSceneで使うよ。　SSSだわ.
	・スコアの表示しまーす.

	・プレイヤー毎にRocketのスプライト変えるから.
	・スコアはGameConfigに教えてもらってね.
	
	・プレイヤーの参加人数で作成するスコアボード変わるわ.
		=>横等分になるようにしてね.


	＜実装しましょ＞
	1. エントリーしたプレイヤー数だけスコアボードを作成する.
 
 */
public class ScoreConfig : MonoBehaviour {

	//public Text score;
	public GameObject prefabSB; //スコアボードのプレハブ. 
	public Texture2D[] rocketColor;    //Rocket4色. 

	public struct PlayerScore
	{
		public int plyaerNumber;//プレイヤーのナンバー.
		//プレイヤーの順位(予定).
		public GameObject scoreBoard;//スコアボード.
	}
	//PlayerScore構造体のリスト.
	List<PlayerScore> listPlayerScore = new List<PlayerScore>();

	GameObject gameConfig;	//ゲームコンフィング.
	Text targetText; //テキスト変更用変数.
	
	// Use this for initialization
	void Start () {
		gameConfig = GameObject.FindGameObjectWithTag("GameConfig"); //タグからGameConfigを取得す.
	
		CreateScoreBoard();
	}

	/* エントリーしたプレイヤーに応じたスコアボードを作成する. */
	void CreateScoreBoard()
	{
			//エントリーしたプレイヤーのみ生成する.
			for (int i = 0; i < 4; i++)
			{

				int playerNum = i + 1; //プレイヤー番号.

				//プレイヤーが不参加の場合はコンティニュする.
				if (!gameConfig.GetComponent<GameConfig>().IsEntryPlayer(playerNum))
				{
					continue;
				}

				//ここから作成開始.
				PlayerScore ps;
				
				GameObject obj = Instantiate(prefabSB);	//スコアボードのインスタンスを作る.
				obj.transform.SetParent(gameObject.transform,false);//意図しない座標にオブジェクトが配置されるのを阻止.
				obj.name = playerNum + "P_SocreBoard";	//スコアボードの名前を変える.
				
				//子オブジェクトであるRocketのスプライトをプレイヤーに応じたカラーにする.
				Image img = obj.transform.Find("Rocket").gameObject.GetComponent<Image>();
				Texture2D rocketTex = rocketColor[playerNum -1];
				img.sprite = Sprite.Create(rocketTex,new Rect(0,0,rocketTex.width,rocketTex.height),Vector2.zero);
				
				//子オブジェクトであるScoreのテキストをGameConfigから取得した値に変更.
				targetText = obj.transform.Find("Score").gameObject.GetComponent<Text>();
				targetText.text = gameConfig.GetComponent<GameConfig>().GetPlayerScore(playerNum).ToString();
				
				ps.plyaerNumber = playerNum;
				ps.scoreBoard = obj;
			}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
