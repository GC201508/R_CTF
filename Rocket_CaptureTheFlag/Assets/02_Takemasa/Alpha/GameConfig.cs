using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*	-	-	-	-	-	-	-	-
 
	・		ネ申
 
 -	-	-	-	-	-	-	-	-*/


public class GameConfig : MonoBehaviour {

	

	bool[] isEntryPlayer = new bool[4] {false,false,false,false};

	// Use this for initialization
	void Start () {

		//このオブジェクトはシーン遷移で破棄されない.
		DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//ゲームに参加するプレイヤーを決める。 EntryConfigが使用する.
	public void SetEntryPlayer(int playerNumber)
	{
		isEntryPlayer[playerNumber - 1] = true;
	}

	/*ゲームに参加の有無を取得する。 
	 * EntryConfig・StageConfigが使用する*/
	public bool IsEntryPlayer(int plyerNumber) {return isEntryPlayer[plyerNumber - 1];}
	
	
}
