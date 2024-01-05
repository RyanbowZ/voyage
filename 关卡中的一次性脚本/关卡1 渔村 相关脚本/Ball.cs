using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rgd;
    public Vector3 initPos;//小球的生成位置
    public Quaternion initRot;//初始旋转角度
    public Transform groove;//凹槽的位置
    public float downBorder;//下边界，超过这个范围就遣返
    public Vector3 curPos;

    void Awake(){
        rgd=this.GetComponent<Rigidbody>();
    }

    void Update(){
        curPos=this.transform.position;
        ResetBall();
    }

    void ResetBall(){
        if(this.transform.position.y<downBorder){
            groove.localRotation=initRot;
            rgd.velocity=new Vector3(0f,0f,0f);
            this.transform.position=initPos;
        }
    }

}
