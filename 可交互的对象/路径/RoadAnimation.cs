using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadAnimation : MonoBehaviour
{
    public Transform parent;
    public Transform player;
    public void AnimStart(){
        if(parent!=null&&player!=null){
            player.SetParent(parent);
            NavManager.DisableAgent();
        }
    }

    public void AnimStop(){
        if(player!=null){
           player.SetParent(null);
           NavManager.EnableAgent();
        }
        NavManager.BulidMesh();
    }
}
