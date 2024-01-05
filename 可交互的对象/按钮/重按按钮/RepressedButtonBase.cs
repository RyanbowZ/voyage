using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可重复按下的按钮的基类
public class RepressedButtonBase : MonoBehaviour
{
    public bool isTest;
    Animator selfAnim;

    private void Awake() {
        selfAnim=this.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {
        if(isTest) Debug.Log(other.gameObject);
        if(other.gameObject.CompareTag("Player")){
            if(isTest) Debug.Log("enter");
            TriggerSth();
        }   
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject.CompareTag("Player")){
            if(isTest) Debug.Log("exit");
            ResetSth();
        }   
    }

    #region 按下和离开时触发的操作
    //
    //基类只有按钮的按下和复原动画！
    virtual public void TriggerSth(){
        //AudioManager.PlayPressButtonMusic();
        selfAnim.SetTrigger("down");
    }

    virtual public void ResetSth(){
        selfAnim.SetTrigger("up");
    }

    #endregion
}
