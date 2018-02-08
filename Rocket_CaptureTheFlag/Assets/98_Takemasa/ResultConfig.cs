using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
	・旗取ったプレイヤーを賞賛する.
	・ポイント追加する.
	・
 */
public class ResultConfig : MonoBehaviour {

	public Font allDiefont;	//全滅時に差し替えるフォント.
	public GameObject goalText;	//ゴールテキストのオブジェクト.
	public GameObject scoreConfig; //スコアコンフィグのオブジェクト(実体はCanvas).
	public GameObject nextScene;	//シーンマネージャー.

	GameConfig gameConfig;  //GameConfigのコンポーネント.
	int goalPlayerNum;	//ゴールしたプレイヤー番号.
	Text textGoal;	//ゴールテキストのテキスト.
	Shadow shadowGoal;	//ゴールテキストのシャドウ.	

	// Use this for initialization
	void Start () {

		gameConfig = GameObject.FindGameObjectWithTag("GameConfig").GetComponent<GameConfig>(); //タグからGameConfigを取得す.

		goalPlayerNum = gameConfig.GetGoalPlayerNumber(); //ゴールしたプレイヤー番号を受渡.
		
		textGoal = goalText.gameObject.GetComponent<Text>();

		shadowGoal = goalText.gameObject.GetComponent<Shadow>();
		

		//ゴールしたプレイヤーがいる時.		
		if(goalPlayerNum != 0)
		{
			textGoal.text = goalPlayerNum + " P　ゴ ー ル"; //ゴールしたプレイヤーのテキスト表示.
			ColorEffect ce = GetComponent<ColorEffect>();
			StartCoroutine(ce.LoopChangeTextColor(textGoal,goalPlayerNum,0.5f));
		}
		//ゴールしたプレイヤーがいない時.
		else
		{
			//テキストのデザインを変更す.
			textGoal.font = allDiefont;
			textGoal.text = "全　滅";
			textGoal.fontSize = 88;
			textGoal.color = new Color(163f / 255f,15f / 255f,12 / 255f); // 163,15,12
			shadowGoal.effectDistance.Set(1.0f,-5.0f);  //x 1 y -5
			shadowGoal.effectColor = new Color(253f / 255f,215f / 255f,0);	//253 , 215 , 0
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//全てのスコア集計が終了した時、シーンを移動する.
		if(scoreConfig.GetComponent<ScoreConfig>().IsScoreEnd())
		{
			gameConfig.InitBounus();	//ボーナスをリセット.	
			nextScene.SetActive(true); //シーンを移動する.
		}
		
	}
}
