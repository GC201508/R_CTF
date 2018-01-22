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

	public Text goalText;	//ゴールテキスト.
	int goalPlayerNum;	//ゴールしたプレイヤー番号.
	// Use this for initialization
	void Start () {
		//全滅時とゴール時で表示するテキストを変える予定.

		//ゴールしたプレイヤーがいる時.
		GameObject gameConfig = GameObject.FindGameObjectWithTag("GameConfig"); //タグからGameConfigを取得す.
		goalPlayerNum = gameConfig.GetComponent<GameConfig>().GetGoalPlayerNumber(); //ゴールしたプレイヤー番号を受渡.
		goalText.text = goalPlayerNum + " P　ゴ ー ル"; //ゴールしたプレイヤーのテキスト表示.
	
		//ゴールしたプレイヤーがいない時.
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	/*ゴールボーナス入れるとこ*/

	/*仲良死ボーナス入れるとこ*/
}
