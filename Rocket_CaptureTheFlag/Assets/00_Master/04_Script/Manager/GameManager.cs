using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	-	-	-	-	-	-	-	-	-	-	-
 
	・シーンを遷移しても保持すべきデータを管理する.
 
 -	-	-	-	-	-	-	-	-	-	-	*/

public class GameManager : MonoBehaviour {

	int entryPlayer; //参加するプレイヤー数.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
				
	}
	
	//参加するプレイヤー数をセットする.
	//EntryManagerが使う.
	void SetPlayerEntry(int entry)
	{
		entryPlayer = entry;
	}

}
