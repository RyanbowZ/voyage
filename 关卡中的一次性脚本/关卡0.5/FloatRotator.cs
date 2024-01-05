using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatRotator : SimpleButtonBase
{
    [SerializeField]Animator rotatorAnim;
    public override void TriggerSth()
    {
        rotatorAnim.enabled=true;
        base.TriggerSth();
    }
}
