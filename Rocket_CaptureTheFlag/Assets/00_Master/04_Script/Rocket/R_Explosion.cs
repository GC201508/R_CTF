using UnityEngine;
using System.Collections;

public class R_Explosion : MonoBehaviour
{

	public GameObject explosion;
	bool isDestroy = false;

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
			Invoke("InvokeDestroyObject", 3.0f);
		}


	}

	/*爆発オブジェクトとRocketオブジェクトを削除する*/
	void InvokeDestroyObject()
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
		GameObject obj = (GameObject)Instantiate(explosion, transform.position, Quaternion.identity);
		// 作成した爆発オブジェクトを子として登録
		obj.transform.parent = transform;
		//スケール調整
		obj.transform.localScale = new Vector3(20.0f, 20.0f, 20.0f);

		//子オブジェクトRocketFireを消す
		Destroy(transform.Find("RocketFire").gameObject);

		//画像を非表示にする
		Destroy(GetComponent<SpriteRenderer>());

		isDestroy = true;
	}


	/*コリジョンに衝突したら*/
	void OnCollisionEnter2D(Collision2D sion2d)
	{
		//一度も衝突していない時.
		if (!isDestroy)
		{
			RocketExplosion();
		}
	}
}
