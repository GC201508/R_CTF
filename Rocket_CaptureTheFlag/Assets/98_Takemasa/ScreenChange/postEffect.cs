using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class postEffect : MonoBehaviour
{

	//画面切り替え
	const float fadeMax = 64.0f;
	float fadeCount = 1.0f;
	float fadeSpeed = 5.0f;
	bool fadeFlag = true;

	//ゲームコンフィグのコンポーネント.
	GameConfig gameConfig;

	//ポストエフェクトシェーダー入りのマテリアル
	public Material postEffectMat;

	// Use this for initialization
	void Start()
	{
		gameConfig = GameObject.FindGameObjectWithTag("GameConfig").GetComponent<GameConfig>(); //タグからGameConfigを取得す.

		StartCoroutine(ScreenChangeUpdate(false));
	}

	// Update is called once per frame	
	void Update()
	{
		//画面切り替え
		//（仮処理：左ボタンクリックをしたら切り替え）
		if (Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			//fadeFlag = !fadeFlag;

			//StartCoroutine(ScreenChangeUpdate(!fadeFlag));
		}

		//fadeCount = Mathf.Clamp(fadeCount + (fadeFlag ? fadeSpeed : -fadeSpeed) / fadeMax, 0.0f, 1.0f);

		//Debug.Log(fadeFlag);
		//Debug.Log(fadeCount + "だよ！！");
		//postEffectMat.SetFloat("_FadeCount", fadeCount);
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, postEffectMat);
	}

	public void StartScreenChange()
	{
		ScreenChangeUpdate(true);
	}

	IEnumerator ScreenChangeUpdate( bool isfade)
	{

		fadeFlag = isfade;		

		do {
				fadeCount = Mathf.Clamp(fadeCount + (fadeFlag ? fadeSpeed : -fadeSpeed) / fadeMax, 0.0f, 1.0f);

				postEffectMat.SetFloat("_FadeCount", fadeCount);
		
		
			//1フレーム待つ.
			yield return null;

		}while( (fadeFlag && fadeCount != 1) || (!fadeFlag && fadeCount != 0) );

		Debug.Log("処理終わり");

	}
}
