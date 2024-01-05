using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//针对出现新东西和播放动画的按钮的通用脚本
public class ButtonAppearSthOrPlayAnimation : SimpleButtonBase
{
    [Header("关联的游戏物体")]
    public GameObject[] Gappears;
    public GameObject[] Gdisappears; 
    public MirrorController[] mirrors;
    public RoadController[] roads;
    [Header("动画相关")]
    public string animTrigger;
    public Animator animator;

    public override void TriggerSth()
    {   //
        //if(Gappears.Length!=0)         AudioManager.PlayAppearSthMusic();

        foreach(var obj in Gdisappears)
            obj.SetActive(false);

        foreach(var obj in Gappears)
            obj.SetActive(true);
        
        foreach(var mirror in mirrors)
            mirror.SetState(MirrorState.FINISHED);
        foreach(var road in roads)
            road.SetState(RoadState.FINISHED);
        if(animator!=null){
                animator.SetTrigger(animTrigger);
        }
        NavManager.BulidMesh();
        base.TriggerSth();
    }


    
}
