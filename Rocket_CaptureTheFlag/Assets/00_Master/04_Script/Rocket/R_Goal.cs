using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*	-	-	-	-	-	-	-	-	-	-
 
	・ゴールした時の処理す.
	・StageConfigがゴール後の処理をする.

 -	-	-	-	-	-	-	-	-	-	*/
public class R_Goal : MonoBehaviour {

	bool isGoal = false; //ゴール状態か否か.

	/*衝突判定がトリガーなのに注意*/
	void OnTriggerEnter2D(Collider2D der2d)
	{ 
		if(der2d.gameObject.tag == "GoalFlag")
		{
			isGoal = true;
		}
	}

	/*Rocketがゴールしたかを返す
	 *StageConfigで使用する.		*/
	public bool IsRocketGoal()
	{ 
		return isGoal;
	}
	
}