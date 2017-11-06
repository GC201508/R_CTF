using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	-	-	-	-	-	-	-	-	-	
 
	・オブジェクトの色変えるスクリプト会釈！！！！！！！
	・プレイヤーEntryシーンで使うとアイ・アムうまいアム！！！！！　レスキュー！！レスキュー！！
 
-	-	-	-	-	-	-	-	-	*/

public class ColorSprict : MonoBehaviour {

	

	// Use this for initialization
	void Start () {
		//選択されてない状態の色は灰色で透けさせる.
		GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,0.7f);
	}
	
	// Update is called once per frame
	void Update () {
		//コントローラのスタートボタンの入力があったら色の状態を戻す.
		if (Input.GetKeyDown(KeyCode.Joystick1Button7))
		{ 
			GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,1.0f);
		}

	}
}
