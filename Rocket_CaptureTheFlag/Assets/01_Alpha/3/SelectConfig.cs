using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
/*	-	-	-	-	-	-	-	-	-	-	-	
	
	・ステージの選択を行った後シーンを移動する.
	・ここではSceneManagerを使ったシーン遷移を行わない.
	
	＜予定＞
	・各プレイヤーが保有しているポイントを表示する.
 
 -	-	-	-	-	-	-	-	-	-	-	*/

public class SelectConfig : MonoBehaviour
{

	public GameObject[] slectStage; //選択するステージの黒いほう.

	//ステージ選択に使う構造体.
	public struct StageContent
	{
		public GameObject Stage;
		public bool isSelect;
	}

	//構造体のリスト.
	List<StageContent> listStageContent = new List<StageContent>();

	int stageMax = 0;   //登録したステージ数。 slectStage配列の長さで初期化してね.
	int selectNum = 0;  //現在選択しとるステージ.
	bool isInputAxis = false;   //Axisに入力があるとき.


	// Use this for initialization
	void Start()
	{

		//ステージオブジェクトをリストに追加する.
		foreach (GameObject go in slectStage)
		{
			StageContent sc;
			sc.Stage = go;
			sc.isSelect = false;

			listStageContent.Add(sc);
		}

		SetActiveSC(listStageContent[0], true); //最初のステージをActiveにする.

		stageMax = slectStage.Length - 1; //ステージ数.
	}

	// Update is called once per frame
	void Update()
	{
		SelectStage();
		DecisionStage();
	}

	/*ステージオブジェクトのSetActiveをtrueにしたりfalseにしたりする.
	 *isSelectをtureにしたりfalseにしたりする.						*/
	void SetActiveSC(StageContent sc, bool isActive)
	{
		StageContent tmpSC; //値変更用.
		tmpSC = sc;
		tmpSC.isSelect = isActive;

		sc = tmpSC;
		sc.Stage.SetActive(isActive);
	}

	/*ジョイスティック左右でステージを選択する.*/
	void SelectStage()
	{
		//TODO(J.Takemasa):	動作テストの為にジョイスティック１の入力のみで動作する.
		//					Master版ではエントリーしたプレイヤーらの入力に変更す.

		float Axis = Input.GetAxisRaw("Horizontal");    //ジョイスティックの左右入力を取得.
		bool isChangeStage = false; //選択しとるステージに変更があったらtrueになる.
		int changeNum = 0;  //変更する数.

		//入力がない場合.
		if (Axis == 0)
		{
			isInputAxis = false;
		}

		//入力があった場合.
		if (Axis != 0 && !isInputAxis)
		{
			//左(-1)の入力があって,selectNumが0でない時.
			if (Axis == -1 && selectNum != 0)
			{
				selectNum--; //１減らす.
				changeNum = 1;  //1入れる.
				isChangeStage = true;
				isInputAxis = true;

			}

			//右(1)の入力があって,selectNumがMaxでない時.
			if (Axis == 1 && selectNum != stageMax)
			{
				selectNum++; //１増やす.
				changeNum = -1; //-1入れる.
				isChangeStage = true;
				isInputAxis = true;
			}

		}

		//選択に変更があった場合.
		if (isChangeStage)
		{
			SetActiveSC(listStageContent[selectNum + changeNum], false); //選択前のステージをoffに.
			SetActiveSC(listStageContent[selectNum], true); //選択後のステージをtrueに.
		}

	}

	/*ステージを決定してシーンを移動する.*/
	void DecisionStage()
	{
		//Aボタン(JoystickButton0)で決定する.
		if (Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			SceneManager.LoadScene(listStageContent[selectNum].Stage.name);
		}
	}
}
