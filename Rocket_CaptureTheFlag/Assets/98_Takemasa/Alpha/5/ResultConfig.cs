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
	public Font allDiefont;	//全滅時に差し替えるフォント.

	int goalPlayerNum;	//ゴールしたプレイヤー番号.
	// Use this for initialization
	void Start () {

		//ゴールしたプレイヤーがいる時.
		GameObject gameConfig = GameObject.FindGameObjectWithTag("GameConfig"); //タグからGameConfigを取得す.
 
		goalPlayerNum = gameConfig.GetComponent<GameConfig>().GetGoalPlayerNumber(); //ゴールしたプレイヤー番号を受渡.
		
		if(goalPlayerNum != 0)
		{
			goalText.text = goalPlayerNum + " P　ゴ ー ル"; //ゴールしたプレイヤーのテキスト表示.
		}
		//ゴールしたプレイヤーがいない時.
		else
		{
			goalText.font = allDiefont;
			goalText.text = "全　滅";
		}
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	/*ゴールボーナス入れるとこ*/

	/*仲良死ボーナス入れるとこ*/
}
