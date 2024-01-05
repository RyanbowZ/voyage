using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickSetNxtPos : ClickButtonBase
{
    public Animator tip;

    public override void DownEvent()
    {
        NavManager.SetNxtPos(this.transform.position);
        if(tip.gameObject.activeSelf==true) tip.SetTrigger("hide");
        base.DownEvent();
    }
}
