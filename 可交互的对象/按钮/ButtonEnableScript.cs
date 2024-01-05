using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEnableScript : SimpleButtonBase
{
    public MirrorController[] mirrorControllers;
    public RoadController[] roadControllers;

    public override void TriggerSth()
    {
        foreach(var controller in mirrorControllers)
            controller.enabled=true;
        foreach(var controller in roadControllers)
            controller.enabled=true;
        base.TriggerSth();
    }
}
