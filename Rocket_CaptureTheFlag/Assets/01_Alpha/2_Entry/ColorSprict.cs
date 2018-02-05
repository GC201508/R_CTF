using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	-	-	-	-	-	-	-	-	-	
 
	・オブジェクトの色変えるスクリプト会釈！！！！！！！
	・プレイヤーEntryシーンで使うとアイ・アムうまいアム！！！！！　レスキュー！！レスキュー！！
	・対応したロケットの色変えたりシーンの移動したりはマスター版でEntryManager使ってやってね。これテストプログラムだから。　オゾリンナ。

-	-	-	-	-	-	-	-	-	*/

public class ColorSprict : MonoBehaviour {

	public GameObject sceneManager; //ステージセレクトに遷移する状態にしたシーンマネージャ入れてね.
	bool isEntry = false;	//エントリー状態か否か,

	// Use this for initialization
	void Start () {
		//選択されてない状態の色は灰色で透けさせる.
		GetComponent<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,0.7f);
	}
	
	// Update is called once per frame
	void Update () {

		//シーンActiveにしてステージセレクトに遷移させる.
		if (Input.GetKeyDown(KeyCode.Joystick1Button7) && isEntry)
		{ 
			sceneManager.SetActive(true);	
		}

		//コントローラのスタートボタンの入力があったら色の状態を戻す.		
		if (Input.GetKeyDown(KeyCode.Joystick1Button7) && !isEntry)
		{ 
			GetComponent<SpriteRenderer>().color = new Color(1.0f,1.0f,1.0f,1.0f);
			isEntry = true;
		}

	}
}
