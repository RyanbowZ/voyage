using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotator : MonoBehaviour
{
    [Header("关联的游戏物体")]
    [SerializeField] GameObject rotatableObj;
    public float rotateSpeed=1f;//旋转速度

    Vector3 start_pos,pre_pos,offset1,offset2;//记录鼠标拖拽时各个位置的变量
    public void OnMouseDown() {
        pre_pos = Input.mousePosition;
        start_pos = Input.mousePosition;
    }
    public void OnMouseDrag(){
        
        offset1 = (Input.mousePosition - pre_pos)*rotateSpeed;
        offset2= (Input.mousePosition - pre_pos)*rotateSpeed*1.25f;
        pre_pos = Input.mousePosition;

        Vector3 dis=new Vector3(0f,0f,0f);
        dis = new Vector3(offset1.x, 0, 0);
        rotatableObj.transform.Rotate(Vector3.Cross(dis, Vector3.forward), offset1.magnitude, Space.World);
        this.transform.Rotate(Vector3.Cross(dis, Vector3.forward), offset1.magnitude, Space.World);
           
    }
}
