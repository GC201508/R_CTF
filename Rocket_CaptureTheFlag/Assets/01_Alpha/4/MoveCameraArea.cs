using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
	・カメラ外のゴールオブジェクトの位置に応じて移動する判定.
	・この判定に重なっているRocketオブジェクトを取得.
	・Rocketオブジェクトから1番速度が早い値をゲームカメラに送る.
 */
public class MoveCameraArea : MonoBehaviour {

	//float firstSpeed;	//一番早い速度.
	GameObject[] playerRocket;
	BoxCollider2D cameraArea2d;	//カメラエリアのBoxCollider2D.
	GameObject goalPoint;	//ゴールポインツ.

	Vector2 wihgtSize = new Vector2(10.0f,0.0f);
	Vector2 heightSize = new Vector2(0.0f,6.0f);
	Vector2 skewSizeVec2 = new Vector2(7.0f,5.0f);
	Vector2 offset;

	bool isStayRocket = false;

	// Use this for initialization
	void Start () {
		cameraArea2d = gameObject.GetComponent<BoxCollider2D>();
		playerRocket =  GameObject.FindGameObjectsWithTag("Player");	//プレイヤー取得.
		goalPoint = GameObject.FindGameObjectWithTag("GoalFlag");	//ゴールポイント取得.
		
	}
	
	// Update is called once per frame
	void Update () {
		ChangeOffSetCameraArea();
		StayRocketArea();
	}

	/*カメラの範囲外のゴールポイントの位置に応じてエリア位置を変える.*/
	void ChangeOffSetCameraArea()
	{
		Vector2 gpPos = goalPoint.transform.position;
		Vector3 view_pos = Camera.main.WorldToViewportPoint(gpPos);
		float skewSizeX = skewSizeVec2.x;
		float skewSizeY = skewSizeVec2.y;


		//範囲内のとき.
		SetOffSet(Vector2.zero);

		 if(IsLeftOut(view_pos))	SetOffSet(-wihgtSize);	//左外.	
		 if (IsRightOut(view_pos))	SetOffSet(wihgtSize);	//右外.
		 if(IsUpOut(view_pos))	SetOffSet(heightSize);		//上外.
		 if (IsDownOut(view_pos))	SetOffSet(-heightSize);	//下外.		

		//左上外.
		if(IsLeftOut(view_pos) && IsUpOut(view_pos))	SetOffSet(-skewSizeX,skewSizeY);
	

		//右上外.
		if(IsRightOut(view_pos) && IsUpOut(view_pos))	SetOffSet(skewSizeVec2);
		

		//左下外.
		if(IsLeftOut(view_pos) && IsDownOut(view_pos))	SetOffSet(-skewSizeVec2);
		

		//右下外.
		if(IsRightOut(view_pos) && IsDownOut(view_pos))	SetOffSet(skewSizeX,-skewSizeY);

		
		//Offsetサイズを確定.
		cameraArea2d.offset = offset;
	}

	/*プレイヤーがカメラエリアに重なっている時にisStayRocketをtrueにする*/
	void StayRocketArea()
	{
		bool isSR = false;
		foreach(GameObject obj in playerRocket)
		{
			if(obj.GetComponent<R_move>().IsCameraAreaStay()) isSR = true;
		}
		isStayRocket = isSR;
	}

	void SetOffSet(float x,float y)
	{
		offset = new Vector2(x,y);
	}

	void SetOffSet(Vector2 vec2)
	{
		offset = vec2;
	}

	bool IsLeftOut(Vector3 pos) 
	{  
		bool isOut = false;
		if( pos.x < -0.0f) isOut = true;
		return isOut;
	}

	bool IsRightOut(Vector3 pos) 
	{  
		bool isOut = false;
		if( pos.x > 1.0f) isOut = true;
		return isOut;
	}

	bool IsDownOut(Vector3 pos) 
	{  
		bool isOut = false;
		if( pos.y < -0.0f) isOut = true;
		return isOut;
	}

	bool IsUpOut(Vector3 pos) 
	{  
		bool isOut = false;
		if( pos.y > 1.0f) isOut = true;
		return isOut;
	}


	/*Rocketがエリアに重なっとる時にtrueを返す.
	 * GameCameraで使用する.*/
	public bool IsRocketStay()
	{
		return isStayRocket;
	}
}
