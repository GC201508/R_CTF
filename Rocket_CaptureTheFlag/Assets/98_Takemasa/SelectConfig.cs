using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
/*	-	-	-	-	-	-	-	-	-	-	-	
	
	・ステージの選択を行った後シーンを移動する.
	・ここではSceneManagerを使ったシーン遷移を行わない.
		=>ステージ名 ＝ オブジェクト名とし、選択状態のオブジェクトからシーン遷移を行う.
	・SelectItem'sCountText（UI Text）を項目に応じて変更する.
	
	
	・総ステージは １０ステージ　で確定とし、　変更は絶対に受け付けないものとする。
	・1度に画面内に表示する選択数は　３つ　で確定とし、　　変更は絶対に無い。　無いったら無い.	
	

	（スコア表示関連はScoreConfigが担う。　スコア処理のコード不要）
 
 -	-	-	-	-	-	-	-	-	-	-	*/

public class SelectConfig : MonoBehaviour
{
	public Text selectItemCntText; //項目表示するText.
	public RectTransform rtParentIcon;	//子クラスにIconをもつオブジェクトのRectTransform.

	public GameObject[] slectStage; //シーン遷移先の名前と同等のオブジェクトを選ぶ(Iconの子クラスにあるよ).
	
	//ステージ選択に使う構造体.
	public struct StageContent
	{
		public GameObject Stage;
		public bool isSelect;
	}

	//構造体のリスト.
	List<StageContent> listStageContent = new List<StageContent>();


	GameConfig gameConfig;	//GameConfigのコンポーネント.
	int stageMax = 0;   //登録したステージ数。 slectStage配列の長さで初期化してね.
	int selectNum = 0;  //現在選択しとるステージ.
	bool isInputAxis = false;   //Axisに入力があるとき.


	// Use this for initialization
	void Start()
	{

		gameConfig = GameObject.FindGameObjectWithTag("GameConfig").GetComponent<GameConfig>(); //タグからGameConfigを取得す.

		//ステージオブジェクトをリストに追加する.
		foreach (GameObject go in slectStage)
		{
			StageContent sc;
			sc.Stage = go;
			sc.isSelect = false;

			listStageContent.Add(sc);
		}

		selectNum = gameConfig.GetSelectStageNumber();	//最後に選んだステージ番号を取得する.

		SetActiveSC(listStageContent[selectNum], true); //選択状態にするステージをActiveにする.

		stageMax = slectStage.Length - 1; //合計ステージ数.

		ChangeTextItemCount();	//レイアウトを調整する.
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

			ChangeTextItemCount();  //項目数の変更や表示領域の移動を行う(必要ならば).
		}

	}

	/*ステージを決定してシーンを移動する.*/
	void DecisionStage()
	{
		//Aボタン(JoystickButton0)で決定する.
		if (Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			gameConfig.SetSelectStageNumber(selectNum);//GameConfigに最後に選択したステージ番号を記録する.
			SceneManager.LoadScene(listStageContent[selectNum].Stage.name);
		}
	}

	/*ステージの項目数を表示するUI Textの値を変更する.
			=>MoveIconEmpty関数で選択領域の移動も行う. 
	 *ステージ項目数は必ず１０とする.
	 *変更するテキスト　"◀xxxOxx/xx4xxx▶"*/
	void ChangeTextItemCount()
	{
		int moveCnt = 0;	//どれだけ移動するかをwidthSizeに乗算する.		
	
		//１項目参照	1/4
		if (selectNum <= 2)
		{
			selectItemCntText.text = "    1  /  4   ▶";
		}

		//２項目参照	2/4
		else if (selectNum <= 5)
		{
			selectItemCntText.text = "◀   2  /  4   ▶";
			moveCnt = 1;
			
		}

		//３項目参照	3/4
		else if (selectNum <= 8)
		{
			selectItemCntText.text = "◀   3  /  4   ▶";
			moveCnt = 2;
		}

		//４項目参照	4/4
		else
		{
			selectItemCntText.text = "◀   4  /  4    ";
			moveCnt = 3;
		}

		MoveIconEmpty(moveCnt); //移動させる.
	}

	/*項目数に応じて選択領域の表示を移動させるための関数.
	 *第1引数：表示している項目 - 1 の数字(moveCnt).
	 *ChangeTextItemCount関数内で使用する.*/
	void MoveIconEmpty(int cnt)
	{
		float widthSize = 800; //横スクリーンサイズ。これが移動量になる.
		Vector3 toPos = rtParentIcon.localPosition;	//値受け渡し用変数.

		toPos.x = widthSize * cnt * -1; //cntに応じた位置に移動.

		rtParentIcon.localPosition = toPos;	//値渡して終了.
	}
}
