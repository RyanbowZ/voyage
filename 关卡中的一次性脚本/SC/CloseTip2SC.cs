using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTip2SC : MonoBehaviour
{
    public GameObject Ttip;//提示的文本
    public Animator anim;//提示的动画

    // Update is called once per frame
    void Update()
    {
        if(Ttip.activeSelf==true&&(Input.GetKeyDown(KeyCode.Q)||Input.GetKeyDown(KeyCode.E)))
            anim.SetTrigger("hide");
    }
}
