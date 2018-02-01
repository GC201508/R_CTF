using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
	public GameObject ndRusult; //NakayoDieRusultオブジェクト.
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
	int ndPlayerCnt = 0;    //仲良死したプレイヤー数.	
	bool isGoalScoreRusult = false; //trueでゴールスコアの集計をする.
	bool isNDieScoreRusult = false; //trueで仲良死スコアの集計をする.
	int[] localPlayersScore = new int[4] { 1, 2, 3, 4 };    //スコア追加前の各プレイヤースコア.
	Text targetTextGScore;  //GoalTextのコンポーネント.
	Text targetTextScore;   //ScoreTextのコンポーネント.
	Text targetTextNDScore; //NDieScoreTextのコンポーネント.
	Text[] targetTextScoreNDSB;  //NDSBのScoreTextのコンポーネント.
	RectTransform rtRusultLayout;   //リザルトレイアウトのRectトランスフォーム.
	bool isStartGoalScore = false;  //ゴールスコア集計始めた時 trueになる.
	bool isEndGoalScore = false;  //ゴールスコア集計を終えた時 trueになる.
	bool isEndNDieScore = false;  //仲良死スコア集計を終えた時 trueになる.
	bool isMoveStartRusultLayout = false; //リザルトレイアウトが移動開始した時 true.
	bool isMoveEndRusultLayout = false; //リザルトレイアウトが移動し終えた時 true.

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
			rtRusultLayout = gameObject.transform.Find("RusultLayout").GetComponent<RectTransform>(); //RusultLayoutのRectTransformを取得.
			InitPlayerScore();  //GameConfigへポイント追加を済ます.
			InitRusultGoal(); //ゴールリザルト演出の準備.
			InitRusultNakayoDie();//仲良死演出の準備.

			//TODO:スコア演出確認用Invokeです。
			//Invoke("GoalScoreAddAnimation", 1.0f);
			//Invoke("NDieScoreAddAnimation",2.5f);
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
	 * 仲良死ボーナス取得したプレイヤー数を
	 * ※【ボーナスの初期化は実行しない】.
	 *Rusultで使用する.*/
	void InitPlayerScore()
	{
		//ゴールしたプレイヤーのスコア加算.

		if (goalPlayerNum != 0) //ゴールプレイヤー番号が0でない時(全滅してない時)
		{
			Debug.Log("ScoreConfig:" + goalPlayerNum + "P_加算前スコア：" + gameConfig.GetPlayerScore(goalPlayerNum));
			localPlayersScore[goalPlayerNum - 1] = gameConfig.GetPlayerScore(goalPlayerNum);    //加算前スコアを記録.
			gameConfig.AddPlayerScore(goalPlayerNum, addGoalScore); //プレイヤースコアに加算.
		}
		//仲良死したプレイヤーのスコア加算.
		for (int i = 0; i < 4; i++)
		{
			int playerNum = i + 1;  //プレイヤー番号.
			Debug.Log("ScoreConfig:" + playerNum + "P_加算前スコア：" + gameConfig.GetPlayerScore(playerNum));
			//加算対象でない場合コンティニュする.
			if (!gameConfig.IsEntryPlayer(playerNum))   //参加してない.
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
			ndPlayerCnt++; //仲良死ボーナス取得人数をカウント.
			localPlayersScore[i] = gameConfig.GetPlayerScore(playerNum);    //加算前スコアを記録.
			gameConfig.AddPlayerScore(playerNum, addNDieScore); //プレイヤースコアに加算.
			Debug.Log("ScoreConfig:" + playerNum + "P_仲良死スコア加算後:" + gameConfig.GetPlayerScore(playerNum));
			Debug.Log("ScoreConfig:" + playerNum + "P_localスコア:" + localPlayersScore[playerNum - 1]);
		}
	}

	/* ゴールボーナス集計の準備をしますよ. 
	 * ゴールしたプレイヤーがいた場合
	 *		=> ゴールしたプレイヤーのスコア(Text)を初期化.
	 *		=> RocketImageをゴールしたロケットの色(Texture2D)に差し替え.
	 * 全滅している場合
	 *		=>全てのオブジェクトを削除する.
	 *Rusultで使用する.*/
	void InitRusultGoal()
	{
		if (goalPlayerNum != 0) //ゴールしたプレイヤーが居る時.
		{
			isGoalScoreRusult = true;   //ゴールスコアの集計をするのでtrue.

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
		else  //ゴールしたプレイヤーがいない時.
		{
			//ゴールテキストの子関係を解除.
			goalRusult.transform.Find("GoalText").gameObject.transform.SetParent(gameObject.transform.Find("RusultLayout"));


			DestroyImmediateChildObject(goalRusult.transform); //GoalRusultの子オブジェクトを全消去.
		}
	}

	/* 仲良死ボーナス集計の準備.
	 * 仲良死プレイヤーのスコアを初期化.
	 * RocketImageの色(Texture2D)に差し替え.
	 *Rusultで使用する. */
	void InitRusultNakayoDie()
	{
		GameObject[] objNDBS = GameObject.FindGameObjectsWithTag("NDSB"); //仲良死スコアボードを取得.
		GameObject[] sortNDBS = new GameObject[4];  //ソート後のNDBSを格納.
		Regex re = new Regex(@"[^1-4]");    //文字列から数字(1~4)だけ取り出す準備.

		foreach (GameObject obj in objNDBS)
		{
			int index = int.Parse(re.Replace(obj.name, "")); //スコアボードのnameから数値のみ取得.
			sortNDBS[index - 1] = obj; //数値通りにsort配列に追加する.
		}

		//仲良死ボーナス対象プレイヤー
		if (ndPlayerCnt == 0)   //仲良死ボーナスなし.
		{
			GameObject obj = ndRusult.transform.Find("NonNDText").gameObject; //NonDTextオブジェクトのコンポーネント取得.
			obj.transform.SetParent(gameObject.transform.Find("RusultLayout"));   //NakayoDieRusultとの子関係を解除.
			obj.SetActive(true);    //アクティブにする.

			obj = ndRusult.transform.Find("NDieText").gameObject;//NDieTextのオブジェクト取得.
			obj.transform.SetParent(gameObject.transform.Find("RusultLayout"));   //NakayoDieRusultとの子関係を解除.

			DestroyImmediateChildObject(ndRusult.transform); //NakayoDieRusultの中身を消.
		}
		else  //仲良死ボーナスあり.
		{
			isNDieScoreRusult = true;   //仲良死スコアを集計するのでtrueに.

			int ndsbCnt = 0;    //使用するNDSB番号.
			targetTextScoreNDSB = new Text[ndPlayerCnt];    //仲良死ボーナス取得数だけ作成.

			//仲良死ボーナスの.
			targetTextNDScore = ndRusult.transform.Find("NDieScore").gameObject.GetComponent<Text>();
			targetTextNDScore.text = "＋" + addNDieScore.ToString();

			for (int i = 0; i < 4; i++)
			{
				int playerNum = i + 1;  //プレイヤー番号.

				if (gameConfig.IsBounusNakayoDiePlayer(playerNum))
				{
					//子オブジェクトであるRocketのスプライトをプレイヤーに応じたカラーにする.
					Image img = sortNDBS[ndsbCnt].transform.Find("Rocket").gameObject.GetComponent<Image>();
					Texture2D rocketTex = rocketColor[playerNum - 1];
					img.sprite = Sprite.Create(rocketTex, new Rect(0, 0, rocketTex.width, rocketTex.height), Vector2.zero);

					//ScoreのTextコンポーネントを取得.
					targetTextScoreNDSB[ndsbCnt] = sortNDBS[ndsbCnt].transform.Find("Score").gameObject.GetComponent<Text>();

					targetTextScoreNDSB[ndsbCnt].text = localPlayersScore[i].ToString();    //テキスト挿入.

					sortNDBS[ndsbCnt].transform.SetParent(ndRusult.transform);    //NdieSocreBoard'sの子関係を削除.					

					PlayerScore ps; //受け渡し用変数.
					ps.plyaerNumber = playerNum;        //プレイヤー番号.	
					ps.scoreBoard = sortNDBS[ndsbCnt];  //NDのスコアボード.
					listPlayerScore.Add(ps); //リストに追加.

					ndsbCnt++;  //カウントを増やす.
				}
			}

			DestroyImmediateChildObject(ndRusult.transform.Find("NDieScoreBoard's")); //NakayoDieRusultの中身を消.
		}

	}

	//--------------------------------------------------------------------------------
	// Immediate版 即時に消えるのでリストの参照の仕方が異なる
	//--------------------------------------------------------------------------------
	public static void DestroyImmediateChildObject(Transform parent_trans)
	{
		for (int i = parent_trans.childCount - 1; i >= 0; --i)
		{
			GameObject.DestroyImmediate(parent_trans.GetChild(i).gameObject);
		}
	}

	// Update is called once per frame
	void Update()
	{

		//Rusult時に実行.
		if (isRusultMode)
		{

			if (!isStartGoalScore)
			{
				isStartGoalScore = true;
				Invoke("GoalScoreAddAnimation", 1.0f);
			}

			//Aボタン(JoystickButton0)で決定する.
			if (Input.GetKeyDown(KeyCode.Joystick1Button0))
			{

				if (isEndGoalScore && !isEndNDieScore)
				{
					isEndNDieScore = true;
				}

				if (isStartGoalScore)
				{
					isMoveStartRusultLayout = true;
				}
			}

			if (isMoveStartRusultLayout && !isMoveEndRusultLayout)
			{
				rtRusultLayout.localPosition += Vector3.left * 20.0f;

				if (rtRusultLayout.localPosition.x <= -800.0f)
				{
					isMoveEndRusultLayout = true;
					rtRusultLayout.localPosition = Vector3.left * 800.0f;
					isEndGoalScore = true;
					Invoke("NDieScoreAddAnimation", 0.5f);
				}
			}
		}
	}

	/* ゴールスコア加算演出を行うます.(Animationって表現あってんのかな…?)
	 *Rusultで使用する.*/
	void GoalScoreAddAnimation()
	{
		if (isGoalScoreRusult)
		{
			float startScore = localPlayersScore[goalPlayerNum - 1];    //演出開始スコア.
			float endScore = gameConfig.GetPlayerScore(goalPlayerNum);  //演出終了スコア.
			float duration = 1.0f;  //演出時間.

			//ゴールスコアからプレイヤースコアへ値を移す演出.
			StartCoroutine(ScoreAnimation(targetTextScore, startScore, endScore, duration));

			startScore = addGoalScore;
			endScore = 0;

			StartCoroutine(ScoreAnimation(targetTextGScore, startScore, endScore, duration, true));

			Debug.Log("ScoreConfig:" + "ゴールスコア演出終了");
		}
	}

	/* 仲良死スコア加算演出を行います.
	 *Rusultで使用する.*/
	void NDieScoreAddAnimation()
	{
		if (isNDieScoreRusult)
		{
			float startScore = 9999;    //演出開始スコア.
			float endScore = 6666;  //演出終了スコア.
			float duration = 1.0f;  //演出時間.

			foreach (var listPS in listPlayerScore)
			{
				int playerNum = listPS.plyaerNumber;    //プレイヤー番号.
				startScore = localPlayersScore[playerNum - 1]; //ローカルスコアを取得.
				endScore = startScore + addNDieScore;   //演出終了時の値.
				targetText = listPS.scoreBoard.transform.Find("Score").gameObject.GetComponent<Text>();  //スコア挿入先Textコンポーネント取得. 

				//仲良死スコアからプレイヤースコアへ値を移す演出.
				StartCoroutine(ScoreAnimation(targetText, startScore, endScore, duration));
			}

			startScore = addNDieScore;
			endScore = 0;

			StartCoroutine(ScoreAnimation(targetTextNDScore, startScore, endScore, duration, true));

			Debug.Log("ScoreConfig:" + "仲良死スコア演算終了");
		}
	}

	/* 開始スコアから終了スコアを設定し処理を行う.
	 * StartCoroutine(ScoreAnimation(xx,xx,xx,xx,[xx]))の形で使用.
	 * 第一引数：書換先テキストコンポーネント (Text).
	 * 第二引数：開始スコア (float).
	 * 第三引数：終了スコア (float).
	 * 第四引数：演出時間	(float).
	 * (第五引数：【任意】trueで先頭文字に"＋"をつける).
	 *Rusultで使用する.*/
	IEnumerator ScoreAnimation(Text targetTex, float startScore, float endScore, float duration, bool isAddPlus = false)
	{
		float startTime = Time.time; //開始時間.
		float endTime = startTime + duration; //終了時間.

		do
		{
			//現在の時間の割合.
			float timeRate = (Time.time - startTime) / duration;

			//数値を更新.
			float updateValue = (float)((endScore - startScore) * timeRate + startScore);

			//テキストの更新("f0"の"0"は小数点以下の除数指定).
			targetTex.text = (isAddPlus) ? "＋" + updateValue.ToString("f0") :
											updateValue.ToString("f0");

			//1フレーム待つ.
			yield return null;
		} while (Time.time < endTime);

		//最終的なスコア.
		targetTex.text = (isAddPlus) ? "＋" + endScore.ToString() : endScore.ToString();

	}


	/* 全てのスコア集計演出（仲良死スコア集計が終わった段階）でtrueを返す.
	 * プレイヤーが１人の場合、最初にボタンが押下された時点でtrueになるisMoveStartRusultLayoutを返す. 
	 *RusultConfigで使用する.*/
	public bool IsScoreEnd() 
	{
		if(gameConfig.GetEntryPlayerCount() == 1)
		{
			return isMoveStartRusultLayout;
		}		
	 
		return isEndNDieScore; 
	}

}
