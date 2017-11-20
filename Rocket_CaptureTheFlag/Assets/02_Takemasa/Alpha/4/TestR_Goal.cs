using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*	-	-	-	-	-	-	-	-	-	
 
	・ロケットが旗（ゴール）にたどり着いた時の処理.
	・とりあえずリザルトのシーンに飛ばすだけやろう.

 -	-	-	-	-	-	-	-	-	*/

public class TestR_Goal : MonoBehaviour {

	public GameObject sceneManager; //リザルトに飛ぶように設定した奴いれたれ.

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D der2d)
	{ 
		if(der2d.gameObject.tag == "GoalFlag")
		{
			sceneManager.SetActive(true);
		}
	}

}
