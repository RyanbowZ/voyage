using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneFirstButton : RepressedButtonBase
{
    [SerializeField] RotatorController rotator;
    public override void TriggerSth()
    {
        rotator.SetCanRotate(true);
        base.TriggerSth();
    }

    public override void ResetSth()
    {
        rotator.SetCanRotate(false);
        base.ResetSth();
    }
}
