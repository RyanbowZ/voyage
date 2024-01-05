using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//当场景初次生成时执行一些操作
public class OnSceneInit : MonoBehaviour
{
    public Vector3 pos;
    void Start()
    {
        NavManager.SetNxtPos(pos);
    }

}
