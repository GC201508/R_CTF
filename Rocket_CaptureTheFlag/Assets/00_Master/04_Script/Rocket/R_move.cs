using UnityEngine;
using System.Collections;

public class R_move : MonoBehaviour {

    Rigidbody2D rb2d;
    float beforeAxis;
    string sHorizontal;

    public int joyNum = 0;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        sHorizontal = (joyNum == 0 ) ? "Horizontal":"Horizontal" + joyNum;
    }
	
	// Update is called once per frame
	void Update () {
        MoveAcceleration();
        MoveRotation();
    }

    /*旋回が行われていない間加速し続けていく処理*/
    void MoveAcceleration()
    {
        //左右入力が解かれた瞬間に加速する,
        if(Input.GetAxisRaw(sHorizontal) == 0.0f && beforeAxis != 0.0f)
        { 
           rb2d.velocity *= 0.5f;
        }
        
        //徐々に加速していく,
        rb2d.AddForce(transform.up * 2.0f);
        
    }


    /*左右入力時回転するが減速する*/
    void MoveRotation()
    {
        //Axisに入力を保持,
        float Axis = -Input.GetAxisRaw(sHorizontal);

        //入力があった場合Rocketと回転させる,
        if (Axis != 0)
        {
            //速度が1.0fを上回っている場合減速させる,
            if (rb2d.velocity.magnitude > 1.0f)
            {
                rb2d.velocity *= 0.97f;
            }
            transform.Rotate(new Vector3(0, 0, 2 * Axis));
        }

        //前回入力を保持
        beforeAxis = Axis;
    }
}
