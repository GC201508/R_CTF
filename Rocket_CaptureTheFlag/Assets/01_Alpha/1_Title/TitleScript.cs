using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -	-	-	-	-	-	-	
 
	・タイトルの制御会釈！！！！！！！会釈！！！！！！！　レスキュー！！！！！！！！！！！！！！！！！！

 -	-	-	-	-	-	-	*/

public class TitleScript : MonoBehaviour {

	public GameObject sceneManager;	//Entryに移行する用設定したオブジェクトダンス隊登場！！！！！

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//スタートボタンでsceneManagerをActiveTrue会釈！！
		if(Input.GetKeyDown(KeyCode.JoystickButton7))
		{
			sceneManager.SetActive(true);
		}
	}
}
