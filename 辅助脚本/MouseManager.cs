using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

/*
脚本功能：通过鼠标点击的方式，控制玩家的移动等
编写者：孙秋林
初次编写时间：2022/1/31
上次修改时间：2022/2/5
*/
[System.Serializable]
public class EventVector3:UnityEvent<Vector3>{}
public class MouseManager : MonoBehaviour
{
    [Header("点击特效")]
    public GameObject clickEffect;
    [Header("测试相关")]
    public bool test;
    public int[] hitLayers;
    public static MouseManager instance;
    public Camera cam;
    RaycastHit hitInfo;//鼠标碰撞信息
    public EventVector3 OnMouseClicked;
    private void Awake() {
        if(instance!=null)
            Destroy(this);
        instance=this;
    }

    private void Update() {
        SetCursorTexture();
        MouseControl();
        if(test)
            Debug.Log(hitInfo.collider);
    }
    #region (未实现)鼠标贴图的更换
    void SetCursorTexture(){
        Ray ray=cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out hitInfo))
        {
            //TODO:切换鼠标贴图
        }
    }
    #endregion

    /*
    根据鼠标点击做出相应操作

    修改历史：
        2022/1/31 孙秋林：创建该函数
    */

    void MouseControl(){
        if(Input.GetMouseButtonDown(0)&&hitInfo.collider!=null){
            foreach(int layer in hitLayers){
                if(layer==hitInfo.transform.gameObject.layer){
                    OnMouseClicked?.Invoke(hitInfo.point);//将点击地面上的坐标回传给OnMouseClicked事件
                    if(clickEffect != null)
                    {
                        GameObject temp;
                        temp = Instantiate<GameObject>(clickEffect, hitInfo.point, Quaternion.Euler(90f, 90f, 0f));
                        // temp.transform.Rotate(hitInfo.transform.eulerAngles);
                        temp.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y + 0.07f, hitInfo.point.z);
                    }
                }
            }
        }
    }


}
