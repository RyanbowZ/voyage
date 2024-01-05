using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitRoadsManager : MonoBehaviour
{
    public static InitRoadsManager instance;
    public bool isTest=false;

    static Queue<GameObject> initRoads=new Queue<GameObject>();//存储所有已经生成的路径的引用

    void Awake(){
        if(instance!=null)Destroy(this);
        instance=this;
    }

    public static void AddRoad(GameObject obj){
        initRoads.Enqueue(obj);
    }

    public static void ClearRoad(){
        if(instance.isTest)Debug.Log("清除路径");
        while(initRoads.Count!=0){
            GameObject obj=initRoads.Dequeue();
            Destroy(obj);
        }
    }
}
