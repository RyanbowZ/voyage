using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebulidRoad : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Transform parent;

    public void AnimStart(){
        NavManager.DisableAgent();
        NavManager.DisableAgent();
        player.SetParent(parent);
    }

    public void AnimStop(){
        player.SetParent(null);
        NavManager.EnableAgent();
        NavManager.BulidMesh();
    }
}
