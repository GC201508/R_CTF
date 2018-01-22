using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/*

	・SelectStageSceneで使うよ。　SSSだわ.
	・【Canvasにアタッチ】してね.
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
	//<Select・Rusult共通：public変数>
	public Texture2D[] rocketColor;    //Rocket4色. 
	public bool isStageSelectMode = false;  //trueでStageSelectシーンのスコア処理を行う.
	public bool isRusultMode = false;   //trueでResultシーンのスコア処理を行う.
	public GameObject prefabSB; //スコアボードのプレハブ. 

	//<Rusultのみ使用：public変数>
	public GameObject goalRusult;//GoalRusultオブジェクト(StageSlectでは不必要).
	public int addGoalScore = 9999; //加算するゴールスコア.
	public int addNDieScore = 4444; //加算する仲良死スコア.


	//<Select・Rusult共通：private変数>
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

	//<Rusultのみ使用：private変数>
	int goalPlayerNum;  //ゴールしたプレイヤーの番号.
	int[] localPlayersScore = new int[4] { 1, 2, 3, 4 };    //スコア追加前の各プレイヤースコア.
	Text targetTextGScore;	//GoalTextのコンポーネント.
	Text targetTextScore;	//ScoreTextのコンポーネント.

	// Use this for initialization
	void Start()
	{
		Debug.Log("====-------------ScoreConfigLoad--------------====");

		//チェック入れ忘れログ表示
		if (!isStageSelectMode && !isRusultMode) { Debug.Log("(どのModeか)ぜんぜんわからん！"); }

		//タグからGameConfigを取得す.
		gameConfig = GameObject.FindGameObjectWithTag("GameConfig").GetComponent<GameConfig>();

		//ステージセレクト時にスコア表示.
		if (isStageSelectMode)
		{
			CreateScoreBoard();  //スコアボード作成.
			AbjustmentPosScoreBoard();  //配置を調整.
		}

		//リザルト時にスコア表示.
		else if (isRusultMode)
		{
			goalPlayerNum = gameConfig.GetGoalPlayerNumber();//ゴールしたプレイヤー番号を取得.
			InitPlayerScore();  //GameConfigへポイント追加を済ます.
			InitRusultGoal(); //リザルト演出の準備.
			
			//TODO:スコア演出確認用Invokeです。　呼び出し方が変わる可能性あり.
			Invoke("ScoreAddAnimation", 1.0f);
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


	/* GameConfigへ各ボーナスのスコア追加をします.
	 * 加算前のスコアをlocalPlayersScoreへ.
	 * ※【ボーナスの初期化は実行しない】.
	 *Rusultで使用する.*/
	void InitPlayerScore()
	{
		//ゴールしたプレイヤーのスコア加算.
		Debug.Log("ScoreConfig:" + goalPlayerNum + "P_加算前スコア：" + gameConfig.GetPlayerScore(goalPlayerNum));
		localPlayersScore[goalPlayerNum - 1] = gameConfig.GetPlayerScore(goalPlayerNum);
		gameConfig.AddPlayerScore(goalPlayerNum, addGoalScore);

		//仲良死したプレイヤーのスコア加算.
		for (int i = 0; i < 4; i++)
		{
			int playerNum = i + 1;  //プレイヤー番号.
			Debug.Log("ScoreConfig:" + playerNum + "P_加算前スコア：" + gameConfig.GetPlayerScore(playerNum));
			//加算対象でない場合コンティニュする.
			if (!gameConfig.IsEntryPlayer(playerNum))	//参加してない.
			{
				Debug.Log("ScoreConfig:" + playerNum + "P_参加してない！ｗ");
				continue;
			}   
			if (!gameConfig.IsBounusNakayoDiePlayer(playerNum) ||   //ボーナス取得してない.
				playerNum == goalPlayerNum) //ゴールボーナス取得してる.
			{
				Debug.Log("ScoreConfig:" + playerNum + "P_仲良死ボーナス獲得権利なし！ｗ");
				continue;
			}

			//仲良死加算
			localPlayersScore[playerNum] = gameConfig.GetPlayerScore(playerNum);
			gameConfig.AddPlayerScore(playerNum, addNDieScore);
			Debug.Log("ScoreConfig:" + playerNum + "P_仲良死スコア加算後:" + gameConfig.GetPlayerScore(playerNum));
			Debug.Log("ScoreConfig:" + playerNum + "P_localスコア:" + localPlayersScore[playerNum]);
		}
	}

	/* GoalScore集計の準備をしますよ. 
	 * ゴールしたプレイヤーのスコア(Text)を初期化.
	 * RocketImageをゴールしたロケットの色(Texture2D)に差し替え.
	 *Rusultで使用する.*/
	void InitRusultGoal()
	{

		//タグから該当するオブジェクトを取得.
		GameObject[] objRocket = GameObject.FindGameObjectsWithTag("RocketImage");

		//各スプライト差し替え.
		foreach (GameObject objR in objRocket)
		{
			Image img = objR.gameObject.GetComponent<Image>();
			Texture2D rocketTex = rocketColor[goalPlayerNum - 1];
			img.sprite = Sprite.Create(rocketTex, new Rect(0, 0, rocketTex.width, rocketTex.height), Vector2.zero);
		}

		//子オブジェクトScoreBoardを取得.
		GameObject objS = goalRusult.transform.Find("ScoreBoard").gameObject;

		//Score(ScoreBoardの子)のTextコンポーネント取得.
		targetTextScore = objS.transform.Find("Score").gameObject.GetComponent<Text>();

		//Scoreテキストを加算前スコア(localPlayerScore)で書き換え.
		targetTextScore.text = localPlayersScore[goalPlayerNum - 1].ToString();

		//GoalScoreのTextコンポーネント取得.
		targetTextGScore = goalRusult.transform.Find("GoalScore").gameObject.GetComponent<Text>();

		//加算するゴールスコアで書き換え.
		targetTextGScore.text = "＋" + addGoalScore;
	}

	// Update is called once per frame
	void Update()
	{

		//Rusult時に実行.
		if (isRusultMode)
		{
	

		}
	}

	/* スコア加算演出を行うます.(Animationって表現あってんのかな…?)
	 *Rusultで使用する.*/
	void ScoreAddAnimation()
	{
		float startScore = localPlayersScore[goalPlayerNum - 1];	//演出開始スコア.
		float endScore = gameConfig.GetPlayerScore(goalPlayerNum);  //演出終了スコア.
		float duration = 1.0f;	//演出時間.
			
		//ゴールスコアからプレイヤースコアへ値を移す演出.
		StartCoroutine(ScoreAnimation(targetTextScore,startScore,endScore,duration));
		
		startScore = addGoalScore;
		endScore = 0;
		
		StartCoroutine(ScoreAnimation(targetTextGScore,startScore,endScore,duration,true));
		
		Debug.Log("ScoreConfig:" + "ゴールスコア演出終了");
		
	}
	IEnumerator ScoreAnimation(Text targetTex, float startScore, float endScore ,float duration,bool isAddPlus = false)
	{
		float startTime = Time.time; //開始時間.
		float endTime = startTime + duration;　//終了時間.

		do {
			//現在の時間の割合.
			float timeRate = (Time.time - startTime) / duration;

			//数値を更新.
			float updateValue = (float)((endScore - startScore) * timeRate + startScore);
		
			//テキストの更新("f0"の"0"は小数点以下の除数指定).
			targetTex.text = (isAddPlus) ? "＋" + updateValue.ToString("f0") : 
											updateValue.ToString("f0"); 
		
			//1フレーム待つ.
			yield return null;
		} while(Time.time < endTime);

		//最終的なスコア.
		targetTex.text = (isAddPlus) ? "＋" + endScore.ToString() : endScore.ToString();
		
	}
}
