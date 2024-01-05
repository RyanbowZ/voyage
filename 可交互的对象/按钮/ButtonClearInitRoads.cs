using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//把所有生成的路径删除
public class ButtonClearInitRoads : SimpleButtonBase
{
    public override void TriggerSth()
    {
        InitRoadsManager.ClearRoad();
    }
}
