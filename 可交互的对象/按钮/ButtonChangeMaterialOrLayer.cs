using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonChangeMaterialOrLayer : SimpleButtonBase
{
    //public ChangePlayerMaterial changePlayer;
    public int newLayer=-1;
    public bool needSetZ;//是否需要增加深度
    public bool needResetZ;//是否需要重置深度
    public override void TriggerSth()
    {
        if(newLayer!=-1) {
            foreach(Transform trans in ChangePlayerMaterial.childs){
                trans.gameObject.layer=newLayer;
            }
        }
        if(needSetZ) ChangePlayerMaterial.SetZ();
        else if(needResetZ) ChangePlayerMaterial.ResetZ();
    }
}
