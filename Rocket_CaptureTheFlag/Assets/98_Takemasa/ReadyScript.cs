using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
	・Stageのcanvasにアタッチする.
	・演出終わりにStageConfigをActiveにする.
 */
public class ReadyScript : MonoBehaviour {

	Text readyText;
	Image gauge;

	// Use this for initialization
	void Start () {
		
		//コンポーネントを取得する.
		readyText = gameObject.transform.Find("ReadyText").GetComponent<Text>();
		gauge = gameObject.transform.Find("TimeGauge").GetComponent<Image>();

		StartCoroutine(ScoreAnimation(gauge,5.0f));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/*時間経過で小さくになるゲージ処理*/
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
		//targetTex.text = (isAddPlus) ? "＋" + endScore.ToString() : endScore.ToString();
		fromScale.x = 0.0f;
		targetImage.rectTransform.localScale = fromScale;

	}
}
