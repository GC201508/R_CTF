using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		p1 = GetNormalizeColor(181, 10);
		p2 = GetNormalizeColor(0, 44, 165);
		p3 = GetNormalizeColor(181, 195, 16);
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
}