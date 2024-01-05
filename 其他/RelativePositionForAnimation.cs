using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 此脚本使得在 Animation 中可以使用相对坐标
/// </summary>
public class RelativePositionForAnimation : MonoBehaviour 
{
    [HideInInspector] public Vector3 relativePosition;//动画的相对位移坐标
    private Vector3 startPosition;

	void Start () 
    {
        //this.startPosition = this.transform.position;
	}
	
	
	void Update () 
    {
        if(relativePosition==new Vector3(0,0,0)){
            this.startPosition = this.transform.position;
            //Debug.Log("yes");
        } 
        Vector3 newPos = this.startPosition + this.relativePosition;
        if(newPos != this.startPosition) //没有在动画中使用此脚本的情况
            this.transform.position = newPos;
	}
}
