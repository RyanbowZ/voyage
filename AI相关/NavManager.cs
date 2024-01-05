using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
脚本功能：自动寻路操作的管理
编写者：孙秋林
初次编写时间：2022/2/7
上次修改时间：2022/2/7
*/
public class NavManager : MonoBehaviour
{
    public bool isTest;
    public static NavManager instance;
    public NavMeshSurface surface;//导航范围
    public NavMeshAgent agent;//人物导航代理

    private void Awake() {
        if(instance!=null)Destroy(this);
        instance=this;
    }
    private void Update() {
        //Debug.Log(agent.destination);
    }


    #region 开关导航
    /*
    针对需要传送玩家或其他操作，临时关闭、打开导航系统

    警告：无
    */
    //
    public static void EnableAgent(){
        if(instance.isTest) Debug.Log("启用寻路AI");
        instance.agent.GetComponent<Rigidbody>().WakeUp();
        instance.agent.enabled=true;
    }
    public static void DisableAgent(){
        if(instance.isTest) Debug.Log("停用寻路AI");
        instance.agent.enabled=false;
        instance.agent.GetComponent<Rigidbody>().Sleep();
    }        
    #endregion

    #region 重新bake导航网络
    public static void BulidMesh(){
        if(instance.isTest) Debug.Log("重构寻路路径");
        instance.surface.BuildNavMesh();
    }
    #endregion

    #region 设置下一个前往的点和停止前往下一个点
    public static void SetNxtPos(Vector3 pos){
        if(instance.isTest)Debug.Log("下一个寻路点为"+ pos);
        instance.agent.destination=pos;
    }
    public static void PauseMove(){
        if(instance.isTest)Debug.Log("暂停移动");
        SetNxtPos(instance.agent.GetComponent<Transform>().position);
    }
    //
    #endregion
}
