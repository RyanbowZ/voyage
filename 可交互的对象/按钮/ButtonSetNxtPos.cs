using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSetNxtPos : SimpleButtonBase
{
    public Vector3 nxtPos;
    public override void TriggerSth()
    {
        NavManager.SetNxtPos(nxtPos);
        //   
    }

    private void OnTriggerStay(Collider other) {
        if(other.gameObject.CompareTag("Player"))   TriggerSth();
    }
}
