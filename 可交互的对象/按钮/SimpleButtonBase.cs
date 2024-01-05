using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//脚本功能：只按一次的按钮基类
//因为考虑到很多按钮的功能都不太统一，所以改成了继承的方式
public class SimpleButtonBase : MonoBehaviour
{
    Animator selfAnim;//按钮自己的动画

    private void Awake() {
        Animator animator=this.GetComponent<Animator>();
        if(this.GetComponent<Animator>()!=null) {
            selfAnim=animator;
            selfAnim.enabled=true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Player"))   TriggerSth();

    }

    #region 按钮按压时触发的操作
    //基类只包含按钮按下动画的播放
    //需要在子类进行重写！！
    //base.triggersth()要放在最后面！不知道原因
    virtual public void TriggerSth(){
        if(GetComponent<MeshRenderer>().enabled==true)
            AudioManager.PlayPressButtonMusic();
        if(selfAnim!=null) selfAnim.SetTrigger("down");
        Invoke("Remove",2f);
    }
    void Remove(){
        Destroy(this.gameObject);
    }
    #endregion

    
    
}
