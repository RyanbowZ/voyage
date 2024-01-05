using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//通过手柄来关闭提示
public class CloseTipOperator : MonoBehaviour{

    //public UImanager uImanager;
    public GameObject Ttip;//提示的文本
    Animator anim;//提示的动画

    private void Start() {
        anim=Ttip.GetComponent<Animator>();
    }

    void OnMouseDown(){
        if(Ttip.activeSelf==true)
            anim.SetTrigger("hide");
    }

}
