using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTip : SimpleButtonBase
{
    public GameObject tip;
    public override void TriggerSth()
    {
        tip.SetActive(true);
    }
}
