using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//玩家切换回临时场景时触发的操作
public class TmpLoad : MonoBehaviour
{
    public int PreSceneIndex{get{return PlayerPrefs.GetInt("PreScene");}}
    public int needIndex;//如果上一个场景是它，把玩家移动到指定位置
    public Transform player;
    public Vector3 playerPos;

    void OnLevelWasLoaded(){
        Debug.Log(PreSceneIndex);
        if(needIndex==PreSceneIndex){
            NavManager.DisableAgent();
            player.position=playerPos;
            NavManager.EnableAgent();
        }
    }
}
