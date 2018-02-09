using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
	・Stageのcanvasにアタッチする.
	・演出終わりにStageConfigをActiveにする.
 */
public class ReadyScript : MonoBehaviour
{

	const float READY_TIME = 2.3f;
	const float GO_TEXT_TIME = 1.2f;

	Text readyText;
	Outline readyOutLine;
	Image gauge;
	R_move[] rocketMoveScript;
	GameObject[] rocketParticle;

	// Use this for initialization
	void Start()
	{
		//各コンポーネントを取得.
		readyText = gameObject.transform.Find("ReadyText").GetComponent<Text>();    //子オブジェクトのReadyTextから取得.
		readyOutLine = gameObject.transform.Find("ReadyText").GetComponent<Outline>(); //前述と同じやつからOutLine取得.
		gauge = gameObject.transform.Find("BackGauge")
						.transform.Find("TimeGauge").GetComponent<Image>(); //こんなコードで取得できるUnityスゲーな.
		
		InitRocket(); //スタート前のRocketオブジェクトに変更を加える.

		Invoke("InvokeScoreAnim", 0.8f);
	}

	/*R_moveコンポーネント取得からenabledをfalseにする.*/
	void InitRocket()
	{
		//Playerタグからオブジェクトを取得.
		GameObject[] obj = GameObject.FindGameObjectsWithTag("Player");
		rocketMoveScript = new R_move[obj.Length];	//配列を初期化.
		rocketParticle = new GameObject[obj.Length]; //同上(型除く).
		for(int i = 0; i < obj.Length; i++)
		{ 
			rocketMoveScript[i] = obj[i].GetComponent<R_move>();	//R_moveコンポーネント取得.
			rocketMoveScript[i].enabled = false;	//!enabledでRocketの動きを止める.
		
			rocketParticle[i] = obj[i].transform.Find("RocketFire").gameObject;	//パーティクルオブジェクト取得.
			rocketParticle[i].SetActive(false); //非表示.
		}
	}

	/*Ready演出終了（ゲームスタート）時にRocketに変更を施す.*/
	void StartRocket()
	{
		//R_moveのenabledをtrueに.
		foreach(R_move rm in rocketMoveScript)
		{
			rm.enabled = true;
		}

		//パーティクルオブジェクトをアクティブにする.
		foreach(GameObject obj in rocketParticle)
		{
			obj.SetActive(true);
		}
	}

	/*Invoke呼出用関数.*/
	void InvokeScoreAnim() { StartCoroutine(ScoreAnimation(gauge, READY_TIME)); }

	/*時間経過で小さくになるゲージ処理
	 *	=>上記処理を終えた後、親オブジェクトもろとも殺す.
	 *	=>Rocketの変更を元に戻す(StartRocket関数を呼出).	
	 *	=>テキストを変更後、徐々に薄くなる演出を実行する.*/
	IEnumerator ScoreAnimation(Image targetImage, float duration)
	{
		float startTime = Time.time; //開始時間.
		float endTime = startTime + duration; //終了時間.
		float scaleX = targetImage.rectTransform.localScale.x;
		Vector3 fromScale = targetImage.rectTransform.localScale;
		do
		{
			//現在の時間の割合.
			float timeRate = (Time.time - startTime) / duration;

			//数値を更新.
			float updateValue = (float)((0 - scaleX) * timeRate + scaleX);

			fromScale.x = updateValue;
			//Image更新.
			targetImage.rectTransform.localScale = fromScale;

			//1フレーム待つ.
			yield return null;
		} while (Time.time < endTime);

		//最終的なxスケール.
		fromScale.x = 0.0f;
		targetImage.rectTransform.localScale = fromScale;

		//親Object(背景ゲージ)と一緒にですとろーい.
		Destroy(gameObject.transform.Find("BackGauge").gameObject);

		//Textを更新.
		readyText.text = "G  O";
		
		//Rocketの状態を元に戻す.
		StartRocket();

		//徐々に消えましょう.
		StartCoroutine(GoTextAnimation(readyText,readyOutLine,GO_TEXT_TIME));
		
	}

	/*時間経過で薄くなるテキストとアウトラインの処理
	 *	=>上記処理の後、オブジェクトもろとも殺す.*/
	IEnumerator GoTextAnimation(Text targetText,Outline targetOutLine, float duration)
	{
		float startTime = Time.time; //開始時間.
		float endTime = startTime + duration; //終了時間.
		float alphaText = targetText.color.a;
		float alphaOutLine = targetOutLine.effectColor.a;
		Color fromTextColor = targetText.color;
		Color fromOutLineColor = targetOutLine.effectColor;

		do
		{
			//現在の時間の割合.
			float timeRate = (Time.time - startTime) / duration;

			//数値を更新(TextAlpha).
			float updateValue = (float)((0 - alphaText) * timeRate + alphaText);
			fromTextColor.a = updateValue;
			
			//TextColor更新.
			targetText.color = fromTextColor;

			//数値を更新(OutLineAlpha).
			updateValue = (float)((0 - alphaOutLine) * timeRate + alphaOutLine);
			fromOutLineColor.a = updateValue;

			//OutLineColor更新.
			targetOutLine.effectColor = fromOutLineColor;

			//1フレーム待つ.
			yield return null;
		} while (Time.time < endTime);

		//最終的に物理的に消える的に.
		Destroy(gameObject);
	}
}