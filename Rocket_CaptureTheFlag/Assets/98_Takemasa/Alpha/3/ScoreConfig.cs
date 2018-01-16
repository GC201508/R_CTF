using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*

	・SelectStageSceneで使うよ。　SSSだわ.
	・Canvasにアタッチしてね.
	・スコアの表示しまーす.

	・プレイヤー毎にRocketのスプライト変えるから.
	・スコアはGameConfigに教えてもらってね.
	
	・プレイヤーの参加人数で作成するスコアボード変わるわ.
		=>横等分になるようにしてね.

 !!追加
	・Resultシーンでも使用する.
	・フラッグを取得したプレイヤーのスコアを加算する演出.
	・仲良死したプレイヤーにボーナス追加する演出.
	・前述２つを追加した後、スコアを各プレイヤーに加算.
	・GameConfigにあるlistScoreBounusの
 
 */
public class ScoreConfig : MonoBehaviour
{

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

	GameObject gameConfig;  //ゲームコンフィング.
	Text targetText; //テキスト変更用変数.

	// Use this for initialization
	void Start()
	{
		gameConfig = GameObject.FindGameObjectWithTag("GameConfig"); //タグからGameConfigを取得す.
		
		//ステージセレクト時にスコア表示.
		if (SceneManager.GetActiveScene().name == "TestSelectScene")
		{
			CreateScoreBoard();
			AbjustmentPosScoreBoard();
		}
	}

	/* エントリーしたプレイヤーに応じたスコアボードを作成. */
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

			GameObject obj = Instantiate(prefabSB); //スコアボードのインスタンスを作る.
			obj.transform.SetParent(gameObject.transform, false);//意図しない座標にオブジェクトが配置されるのを阻止.
			obj.name = playerNum + "P_SocreBoard";  //スコアボードの名前を変える.

			//子オブジェクトであるRocketのスプライトをプレイヤーに応じたカラーにする.
			Image img = obj.transform.Find("Rocket").gameObject.GetComponent<Image>();
			Texture2D rocketTex = rocketColor[playerNum - 1];
			img.sprite = Sprite.Create(rocketTex, new Rect(0, 0, rocketTex.width, rocketTex.height), Vector2.zero);

			//子オブジェクトであるScoreのテキストをGameConfigから取得した値に変更.
			targetText = obj.transform.Find("Score").gameObject.GetComponent<Text>();
			targetText.text = gameConfig.GetComponent<GameConfig>().GetPlayerScore(playerNum).ToString();

			PlayerScore ps;
			ps.plyaerNumber = playerNum; //プレイヤー番号.
			ps.scoreBoard = obj;        //上記で作成したスコアボードオブジェクト.
			listPlayerScore.Add(ps);    //リストに追加.
		}
	}

	/* スコアボードの配置を調整.*/
	void AbjustmentPosScoreBoard()
	{
		Vector3 canvasSize = gameObject.GetComponent<RectTransform>().sizeDelta;    //Canvasの横サイズ取得.
		float divSizeX = (canvasSize.x / (1 + listPlayerScore.Count)); //分割したサイズ幅を求めす.

		//表示位置を変える.
		foreach (var listPS in listPlayerScore.Select((Value, Index) => new { Value, Index }))
		{
			GameObject go = listPS.Value.scoreBoard; //スコアボードのオブジェクト代入.
			RectTransform goRT = go.GetComponent<RectTransform>();  //上記のRectTransformコンポーネント.
			Vector3 goPos = goRT.localPosition; //移動後の位置を作るためRectTransformでのlocalPosを取得.
			int mul = listPS.Index; //ｘ位置の倍率.

			goPos.x = (divSizeX + divSizeX * mul) - canvasSize.x / 2; //調整後の位置を決定す.
			goRT.localPosition = goPos; //RectTransformに戻して完成.
		}
	}


	// Update is called once per frame
	void Update()
	{

	}
}
