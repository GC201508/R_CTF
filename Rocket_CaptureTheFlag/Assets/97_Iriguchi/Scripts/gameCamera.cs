using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameCamera : MonoBehaviour {

    Vector3 moveVec = Vector3.zero;
    float moveSpeed;
    public float moveTime = 3.0f;
    public GameObject GoalPoint;
    public GameObject Player;
    Vector3 toGoalPoint;
    bool Awake;

    // Use this for initialization
    void Start () {
        Awake = true;
    }

    // Update is called once per frame
    void Update () {
        Vector3 CameraPos = transform.position;

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

        moveSpeed = GameObject.Find("Player_1P").GetComponent<Rigidbody2D>().velocity.magnitude / moveTime;

        CameraPos += toGoalPoint * moveSpeed;
        transform.position = new Vector3(CameraPos.x, CameraPos.y, -10);

	}

    void JDI (Vector3 toGPD) //ゴールポイントまでの距離を秒数で割るんだ！！Just Do It!!(引数はゴールポイントとの距離)
    {
        moveVec = toGPD / moveTime;
    }

    void CreateMoveSpeed(GameObject player)
    {
        if(player.tag == "Player")
        {
            moveSpeed = Player.GetComponent<Rigidbody2D>().velocity.magnitude / moveTime;
        }
    }
}
