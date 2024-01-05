using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//将玩家从镜子里传送出来的按钮
public class ButtonSendPlayerOut : SimpleButtonBase
{
    public Transform player;
    Vector3 pos1;
    public Vector3 pos2;

    public override void TriggerSth()
    {
        //pos1=this.transform.position;
        //Vector3 addPos=pos2-pos1;

        NavManager.DisableAgent();
        ChangePlayerMaterial.BeginMakeTransparent();

        StartCoroutine(StepTwo());

    }
    //玩家先消失变透明，再传送出来，再变得不透明
    IEnumerator StepTwo(){
        yield return new WaitForSeconds(1f);

        player.position=pos2;
        ChangePlayerMaterial.BeginMakeUntransparent();

        yield return new WaitForSeconds(1f);

        NavManager.EnableAgent();
    }
}
