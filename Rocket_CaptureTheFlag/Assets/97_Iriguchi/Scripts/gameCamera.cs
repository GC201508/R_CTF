using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameCamera : MonoBehaviour {

    Vector3 moveVec = Vector3.zero;
    float moveSpeed;
    float moveTime = 4.7f;
    GameObject GoalPoint;
    GameObject Corse;
    //public GameObject Player;
    Vector3 toGoalPoint;
    //Vector3 toNextCorse;
    bool Awake = true;
    private GameObject nearPlayer = null;
    MoveCameraArea cameraArea;

    // Use this for initialization
    void Start () {
        nearPlayer = GameObject.Find("Player_01");
        GoalPoint = GameObject.FindGameObjectWithTag("GoalFlag");
    }

    // Update is called once per frame
    void Update () {
        Vector3 CameraPos = transform.position;

        nearPlayer = serchTagSpeed(gameObject, "Player");

        //ゴールポイントとの距離を保持する変数
        Vector3 toGoalPointDis = GoalPoint.transform.position - CameraPos;
        toGoalPoint = toGoalPointDis.normalized;


        if (moveVec == Vector3.zero)
        {
            JDI(toGoalPointDis);
        }
        //CameraPos += moveVec * Time.deltaTime;

        if (Awake == true)
        {
            cameraArea = GameObject.Find("CameraArea").GetComponent<MoveCameraArea>();
            Awake = false;
        }
        //CreateMoveSpeed(Player);

        //プレイヤーを入れ込む、検索方法検討中。
        //moveSpeed = GameObject.Find("Player_1P").GetComponent<Rigidbody2D>().velocity.magnitude / moveTime;
        if (nearPlayer)
        {
            moveSpeed = nearPlayer.GetComponent<Rigidbody2D>().velocity.magnitude / moveTime;
        }
        //CameraPos += toNext * moveSpeed * Time.deltaTime;
        if (cameraArea.IsRocketStay() && (toGoalPointDis.x > 2.0f || toGoalPointDis.y > 2.0f))
        {
            CameraPos += moveVec * moveSpeed * Time.deltaTime;
        }

        //if(toNext.x < 5.0f && toNext.y < 5.0f)
        //{
        //    ChangeNextChild();
        //}

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
    GameObject serchTagDistance(GameObject nowObj, string tagname)
    {
        float tmpDis = 0;               //距離を保持する用の一時変数
        float nearDis = 0;              //最も近いオブジェクトを保持する変数

        float tmpSpeed = 0;
        float fastest = 0;

        GameObject targetObj = null;    //オブジェクト

        //タグ付けされたオブジェクトを配列で取得
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagname))
        {
            //ゴールと取得したオブジェクトの距離を取得
            tmpDis = Vector3.Distance(obs.transform.position, GoalPoint.transform.position);

            //オブジェクトの距離が近いか、距離0であればオブジェクト名を取得。
            //一時変数に取得
            if (nearDis == 0 || nearDis < tmpDis)
            {
                nearDis = tmpDis;
                targetObj = obs;

            }
        }

        //最も近かったオブジェクトを返す。
        return targetObj;
    }

    GameObject serchTagSpeed(GameObject nowObj, string tagname)
    {
        float tmpSpeed = 0;
        float fastest = 0;

        GameObject targetObj = null;    //オブジェクト

        //タグ付けされたオブジェクトを配列で取得
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag(tagname))
        {
            //オブジェクトのmagnitudeを取得
            tmpSpeed = obs.GetComponent<Rigidbody2D>().velocity.magnitude;

            //他より速いか比較する
            if (tmpSpeed > fastest)
            {
                fastest = tmpSpeed;
                targetObj = obs;

            }
        }

        //最も早かったオブジェクトを返す。
        return targetObj;
    }



    void ChangeNextChild()
    {
        Transform pt = Corse.transform;
        foreach (Transform child in pt)
        {
            if (child)
            {
                Corse = child.gameObject;
                break;
            }

        }
    }

}
