using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ButtonSendPlayer : RepressedButtonBase
{
    public MeshCollider[] addStairs;
    public MeshCollider[] disStairs;//对应添加碰撞体的路径和取消碰撞体的路径
    public Transform player;
    Vector3 pos1;
    public Vector3 pos2;

    public override void TriggerSth()
    {
        pos1=this.transform.position;
        Vector3 addPos=pos2-pos1;

        NavManager.DisableAgent();
        foreach(var stair in addStairs) stair.enabled=true;
        foreach(var stair in disStairs) stair.enabled=false;
        player.position+=addPos;

        NavManager.EnableAgent();
    }
}
