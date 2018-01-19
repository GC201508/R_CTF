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
		=>縦等分に並べる.
	・前述２つを追加した後、スコアを各プレイヤーに加算.
	・GameConfigにあるlistScoreBounusの
 
 */
public class ScoreConfig : MonoBehaviour
{

	//public Text score;
	public GameObject prefabSB; //スコアボードのプレハブ. 
	public Texture2D[] rocketColor;    //Rocket4色. 

	public bool isStageSelectMode = false;	//trueでStageSelectシーンのスコア処理を行う.
	public bool isRusultMode = false;	//trueでResultシーンのスコア処理を行う.
	
	public GameObject goalRusult;//ゴールRusult(StageSlectでは不必要).
	
	public struct PlayerScore
	{
		public int plyaerNumber;//プレイヤーのナンバー.
								//プレイヤーの順位(予定).
		public GameObject scoreBoard;//スコアボード.
	}
	//PlayerScore構造体のリスト.
	List<PlayerScore> listPlayerScore = new List<PlayerScore>();

	GameConfig gameConfig;  //ゲームコンフィング.
	Text targetText; //テキスト変更用変数.
	int goalPlayerNum;	//ゴールしたプレイヤーの番号.
	

	// Use this for initialization
	void Start()
	{
		//チェック入れ忘れログ表示
		if(!isStageSelectMode && !isRusultMode) {Debug.Log("(どのModeか)ぜんぜんわからん！"); }		

		
		gameConfig = GameObject.FindGameObjectWithTag("GameConfig").GetComponent<GameConfig>(); //タグからGameConfigを取得す.
		
		//ステージセレクト時にスコア表示.
		if (isStageSelectMode)
		{
			CreateScoreBoard();
			AbjustmentPosScoreBoard();
		}

		//リザルト時にスコア表示.
		else if(isRusultMode)
		{
			goalPlayerNum = gameConfig.GetGoalPlayerNumber();
			ChangeRocketImage();
			InitRusultGoalScore();
		}
	}

	/* エントリーしたプレイヤーに応じたスコアボードを作成. 
	 *StageSlectで使用する. */
	void CreateScoreBoard()
	{
		//エントリーしたプレイヤーのみ生成する.
		for (int i = 0; i < 4; i++)
		{

			int playerNum = i + 1; //プレイヤー番号.

			//プレイヤーが不参加の場合はコンティニュする.
			if (!gameConfig.IsEntryPlayer(playerNum))
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
			targetText.text = gameConfig.GetPlayerScore(playerNum).ToString();

			PlayerScore ps;
			ps.plyaerNumber = playerNum; //プレイヤー番号.
			ps.scoreBoard = obj;        //上記で作成したスコアボードオブジェクト.
			listPlayerScore.Add(ps);    //リストに追加.
		}
	}

	/* スコアボードの配置を調整.
	 *StageSlectで使用する. */
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

	/* RocketImageをゴールしたロケットの色(Texture2D)に差し替え.
	 *Rusultで使用する.*/
	void ChangeRocketImage()
	{
		//タグから該当するオブジェクトを取得.
		GameObject[] objRocket = GameObject.FindGameObjectsWithTag("RocketImage");
		
		//各スプライト差し替え.
		foreach(GameObject obj in objRocket)
		{
			Image img = obj.gameObject.GetComponent<Image>();
			Texture2D rocketTex = rocketColor[goalPlayerNum - 1];
			img.sprite = Sprite.Create(rocketTex, new Rect(0, 0, rocketTex.width, rocketTex.height), Vector2.zero);
		}
		
	}

	/* ゴールしたプレイヤーのスコア(Text)を初期化.
	 * Rusultで使用する.*/
	void InitRusultGoalScore()
	{
		//子オブジェクトScoreBoardを取得.
		GameObject obj = goalRusult.transform.Find("ScoreBoard").gameObject;		
		
		//Score(ScoreBoardの子)のTextコンポーネント取得.
		targetText = obj.transform.Find("Score").gameObject.GetComponent<Text>();	
		
		//Scoreテキストをゴールプレイヤーの所持スコアで書き換え.
		targetText.text = gameConfig.GetPlayerScore(goalPlayerNum).ToString();		
	}

	// Update is called once per frame
	void Update()
	{

	}
}
