using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/*	-	-	-	-	-	-	-	-	
 
	・シーン遷移作るよ！！！！！！！！！！１　会釈！！！！！！！
 
 -	-	-	-	-	-	-	-	-*/

public class SceneSprict : MonoBehaviour {

	public string nextSceneName;	//遷移先のシーン名入れてね！！！！！！！！！！！！！！！！！！！！！！


	// Use this for initialization
	void Start () {
		
		//呼び出したら使えるようにするよ！！！！！！！！　はしゃぎすぎ！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！！
		SceneManager.LoadScene(nextSceneName);
	}
	
	// Update is called once per frame 多分使わない！！！！！！！！！！！！！！！！！
	void Update () {
		
	}
}
