using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameCamera : MonoBehaviour {

    Vector3 moveVec = Vector3.zero;
    float moveSpeed;
    public float moveTime = 3.0f;
    public GameObject GoalPoint;
    //public GameObject Player;
    Vector3 toGoalPoint;
    bool Awake;
    private GameObject nearPlayer;

    // Use this for initialization
    void Start () {
        Awake = true;
        nearPlayer = serchTag(gameObject, "Player");

    }

    // Update is called once per frame
    void Update () {
        Vector3 CameraPos = transform.position;
        nearPlayer = serchTag(gameObject, "Player");

        //ゴールポイントとの距離を保持する変数
        Vector3 toGoalPointDis = GoalPoint.transform.position - CameraPos;
        toGoalPoint = toGoalPointDis.normalized;
        if(moveVec == Vector3.zero)
        {
            JDI(toGoalPointDis);
        }
        //CameraPos += moveVec * Time.deltaTime;

        //if (Awake == true)
        //{
        //    Player = GameObject.Find("Player_1P");
        //    Awake = false;
        //}
        //CreateMoveSpeed(Player);

        //プレイヤーを入れ込む、検索方法検討中。
        moveSpeed = GameObject.Find("Player_1P").GetComponent<Rigidbody2D>().velocity.magnitude / moveTime;
        //moveSpeed = nearPlayer.GetComponent<Rigidbody2D>().velocity.magnitude / moveTime;

        //CameraPos += toGoalPointDis * moveSpeed * Time.deltaTime;
        CameraPos += moveVec * moveSpeed * Time.deltaTime;
        transform.position = new Vector3(CameraPos.x, CameraPos.y, -10);

	}

    void JDI (Vector3 toGPD) //ゴールポイントまでの距離を秒数で割るんだ！！(引数はゴールポイントとの距離)
    {
        moveVec = toGPD / moveTime;
    }

    void CreateMoveSpeed(GameObject player) //プレイヤーの移動速度を取得していろいろ調整。
    {
        if(player.tag == "Player")
        {
            //moveSpeed = Player.GetComponent<Rigidbody2D>().velocity.magnitude / moveTime;
        }
    }


    //指定されたTagの中で最も距離の近いGameObjectを取得する関数
    GameObject serchTag(GameObject nowObj, string tagname)
    {
        float tmpDis = 0;               //距離を保持する用の一時変数
        float nearDis = 0;              //最も近いオブジェクトを保持する変数

        GameObject targetObj = null;    //オブジェクト

        //タグ付けされたオブジェクトを配列で取得
        foreach(GameObject obs in GameObject.FindGameObjectsWithTag(tagname))
        {
            //自身と取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, nowObj.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得。
            //一時変数に取得
            if(nearDis == 0 || nearDis < tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;

            }
        }

        //最も近かったオブジェクトを返す。
        return targetObj;
    }
}
