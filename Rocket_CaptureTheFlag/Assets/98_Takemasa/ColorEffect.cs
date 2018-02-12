using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
/*
 
	・該当するプレイヤーに対して徐々に変色する処理を行う.
	・ResultConfigで呼び出して使う.
 
 */
public class ColorEffect : MonoBehaviour
{

	Color[] colorA;
	Color[] colorB;

	// Use this for initialization
	void Start()
	{
		Color p1 = GetNormalizeColor(255, 30);
		Color p2 = GetNormalizeColor(0, 64, 239);
		Color p3 = GetNormalizeColor(255, 215, 16);
		Color p4 = GetNormalizeColor(111, 200, 0);

		colorA = new Color[] { p1, p2, p3, p4 };

		p1 = GetNormalizeColor(255, 186);
		p2 = GetNormalizeColor(0, 219, 216);
		p3 = GetNormalizeColor(238, 165, 16);
		p4 = GetNormalizeColor(30, 180);

		colorB = new Color[] { p1, p2, p3, p4 };
	}

	// Update is called once per frame
	void Update()
	{

	}

	/*0 ~ 255までの数値を0.0f ~ 1.0f範囲内に変換したColorを返す.*/
	Color GetNormalizeColor(float r = 0.0f, float g = 0.0f, float b = 0.0f)
	{

		return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
	}

	/* テキストの色をA -> B -> の順に変化させる.
	 *第1引数：カラー変更先Textコンポーネント.
	 *第2引数：プレイヤー番号.
	 *第3引数：徐々に変化させる時間. 
	 * RusultConfigで使用する.*/
	public IEnumerator LoopChangeTextColor(Text fromText,int playerNum,float duration)
	{
		Debug.Log("色変えるんよ");
		fromText.color =  colorA[playerNum - 1];	//始まりの色に変える.
		while (true)
		{
			yield return StartCoroutine(ChangeColor(fromText,colorA[playerNum - 1],duration));
			yield return StartCoroutine(ChangeColor(fromText,colorB[playerNum - 1],duration));
		}
		//yield break;
	}

	/*色変えを行う.
	 *第1引数：変更先カラーコンポーネント.
	 *第2引数：変化させたいカラー.
	 *第3引数：徐々に変化させる時間.			
	 */
	IEnumerator ChangeColor(Text fromText, Color toColor, float duration)
	{

		float startTime = Time.time;
		float endTime = Time.time + duration;
		float marginR = toColor.r - fromText.color.r;
		float marginG = toColor.g - fromText.color.g;
		float marginB = toColor.b - fromText.color.b;

		while (Time.time < endTime)
		{
			Color fromColor = fromText.color;
			fromColor.r = fromText.color.r + (Time.deltaTime / duration) * marginR;
			fromColor.g = fromText.color.g + (Time.deltaTime / duration) * marginG;
			fromColor.b = fromText.color.b + (Time.deltaTime / duration) * marginB;

			fromText.color = fromColor;
			yield return 0;
		}

		fromText.color = toColor;
		yield break;
	}
}