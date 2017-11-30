using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*	-	-	-	-	-	-	-	-	-	-	-	
	
	・ステージの選択を行った後シーンを移動する.
	・ここではSceneManagerを使ったシーン遷移を行わない.
	
	＜予定＞
	・各プレイヤーが保有しているポイントを表示する.
 
 -	-	-	-	-	-	-	-	-	-	-	*/

public class SelectConfig : MonoBehaviour {

	public GameObject[] slectStage;	//選択するステージの黒いほう.
	public struct StageContent {
		public GameObject Stage;
		public bool isSelect;
	}
	
	List<StageContent> listStageContent = new List<StageContent>();

	// Use this for initialization
	void Start () {
		foreach (GameObject go in slectStage)
		{ 
			StageContent sc;
			sc.Stage = go;
			sc.isSelect = false;

			listStageContent.Add(sc);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
