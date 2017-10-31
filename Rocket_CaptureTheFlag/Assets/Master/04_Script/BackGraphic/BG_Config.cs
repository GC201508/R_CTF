using UnityEngine;
using System.Collections;

/*	-	-	-	-	-	-	-	-	-	-	-	-	-	-

	・画面内に背景を生成する,
	・RocketやCheckpointの位置から背景を動かす,
	・画像が途切れることなくループさせるようにする,

-	-	-	-	-	-	-	-	-	-	-	-	-	-	*/
public class BG_Config : MonoBehaviour
{

	float width;    //背景画像の幅,
	float height;   //背景画像の高さ,

	public GameObject cloneBG; //生成する背景のプレハブ

	// Use this for initialization
	void Start()
	{


		//条件:root(親オブジェクト)の時,
		if (transform.root.gameObject == gameObject)
		{

			/*SpriteRendererのboundsからSpriteの幅高さを取得する*/
			width = GetComponent<SpriteRenderer>().bounds.size.x;   //背景画像の幅,
			height = GetComponent<SpriteRenderer>().bounds.size.y;  //背景画像の高さ,


			/*１つ目(親)の背景をカメラ左上端に合わせる*/

			Vector3 camPos = Camera.main.ViewportToWorldPoint(Vector3.up); //mainカメラの左上座標を取得,
			camPos.x += width / 2.0f;               //背景の中心座標/2だけ移動しカメラ左端に合わす,
			camPos.y += height / 2.0f * -1.0f;  //背景の中心座標/2だけ移動しカメラ上端に合わす,
			camPos.z = transform.position.z;    //z座標を背景のものにする,	
			transform.position = camPos;    //座標を確定する,


			/*カメラ右下端までの値を取得し、生成する背景の数を決める*/
			Vector2 rightDownPos = Camera.main.ViewportToWorldPoint(new Vector2(1.0f, 0.0f));  //mainカメラの右下座標を取得,

			float maxRightNum = Mathf.Ceil(Mathf.Abs(rightDownPos.x) * 2.0f / width) + 1.0f;  //右へ生成する背景の数,
			float maxDownNum = Mathf.Ceil(Mathf.Abs(rightDownPos.y) * 2.0f / height) + 1.0f; //下へ生成する背景の数,
			


			// i ..　横側に生成する背景,
			// j ..	縦側に生成する背景,
			for (int i = 0; i < maxRightNum; i++)
			{
				Vector3 createPos = transform.position; //座標の起点で初期化,

				createPos.x += width * i; //生成するX座標に移動する,				
				GameObject objX = (GameObject)Instantiate(cloneBG, createPos, Quaternion.identity); //インスタンスを生成,
				objX.transform.parent = transform;	//作成したオブジェクトを子として登録,

				//背景生成（縦）,
				for (int j = 1; j < maxDownNum; j++)
				{
					createPos.y -= height; //生成するY座標に移動する,
			
					GameObject objY = (GameObject)Instantiate(cloneBG, createPos, Quaternion.identity);//インスタンスを生成,		
					objY.transform.parent = transform;//作成したオブジェクトを子として登録,

				}

			}

		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}
