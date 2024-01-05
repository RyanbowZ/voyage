using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTip1SC : MonoBehaviour
{
    public GameObject Ttip;//提示的文本
    public Animator anim;//提示的动画

    private void Update() {
        if(Ttip.activeSelf==true&&Input.GetKeyDown(KeyCode.Space))
            anim.SetTrigger("hide");
    }
}
