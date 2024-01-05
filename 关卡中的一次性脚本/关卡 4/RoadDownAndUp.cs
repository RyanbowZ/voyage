using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//一个可以升降的平台
public class RoadDownAndUp : RepressedButtonBase
{
    [SerializeField] Transform player;
    [SerializeField] Transform parent;//楼梯升降时玩家需要固定位置的parent
    [SerializeField]Animator animPlate;
    [SerializeField] Animator roadAnim;
    bool isUp=true,isDown=false;

    private void Update() {
        SwitchAnim();
    }

    public override void TriggerSth()
    {
        roadAnim.enabled=true;
        NavManager.DisableAgent();
        player.SetParent(parent);
        isUp=!isUp;
        isDown=!isDown;
        base.TriggerSth();
    }

    void ResumeNav(){
        player.SetParent(null);
        NavManager.EnableAgent();
        NavManager.BulidMesh();
    }

    void SwitchAnim(){
        animPlate.SetBool("up",isUp);
        animPlate.SetBool("down",isDown);
    }
}
