using UnityEngine;
using System.Collections;
/*
 
	・ロケットが爆発する処理.
	・壁に触れたら爆発.
	・カメラ外に出たら爆破.
	・プレイヤーに衝突したら爆発.
		=>仲良死ボーナスフラグがtrueになる.
 
 */
public class R_Explosion : MonoBehaviour
{

	public GameObject explosion;
	bool isDestroy = false;     //爆発し終えたら.
	bool isNakayoDie = false;   //仲良死(プレイヤーに衝突)したらtrue.

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		if (!isDestroy && CheckScreenOut(transform.position))
		{
			isDestroy = true;
			RocketExplosion();
		}


		if (isDestroy)
		{
			DestroyObject();
		}


	}

	/*爆発オブジェクトとRocketオブジェクトを削除する*/
	void DestroyObject()
	{
		Destroy(this.gameObject);
	}

	/*座標がカメラ外にいたらtrueを返す*/
	bool CheckScreenOut(Vector3 _pos)
	{
		Vector3 view_pos = Camera.main.WorldToViewportPoint(_pos);
		if (view_pos.x < -0.0f ||
		   view_pos.x > 1.0f ||
		   view_pos.y < -0.0f ||
		   view_pos.y > 1.0f)
		{
			// 範囲外 
			return true;
		}
		// 範囲内 
		return false;
	}

	void RocketExplosion()
	{
		//プレハブからインスタンスを生成
		//GameObject obj = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
		GameObject obj = (GameObject)Instantiate(explosion, transform.position, transform.localRotation);		

		//スケール調整
		obj.transform.localScale = new Vector3(20.0f, 20.0f, 20.0f);

		//子オブジェクトRocketFireを消す
		Destroy(transform.Find("RocketFire").gameObject);

		//画像を非表示にする
		Destroy(GetComponent<SpriteRenderer>());


		isDestroy = true;
	}

	/*Rocketが仲良死したかを返す.
	 * StageConfigで使用する.*/
	public bool IsRocketNakayoDie()
	{
		return isNakayoDie;
	}


	/*コリジョンに衝突したら*/
	void OnCollisionEnter2D(Collision2D sion2d)
	{
		//Playerに衝突.
		if (sion2d.gameObject.tag == "Player")
		{
			isNakayoDie = true;
		}

		//一度も衝突していない時.
		if (!isDestroy)
		{
			RocketExplosion();
		}
	}
}
