using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Road Data",menuName ="New Data/New Obj Data/New Road Data")]
public class RoadStat_SO : ScriptableObject
{
    public RoadStat_SO nxtRoadStat;
    public bool moreRoad;
    public int solutionAmount;
    [Header("预设旋转参数")]
    public bool needRotate=false;//需要旋转
    public float rotateUnit;//单次旋转的单位
    public int rotatePrecision;//保留的小数点位数
    public float rotateOffset;//单位操作的偏移量
    public float correctRotateSpeed=5f;//旋转矫正速度

    [Header("预设平移参数")]
    public bool needTrans=false;//需要平移
    public float transUnit;
    public int transPrecision;
    public float transOffset;
    public float correctTransSpeed;

    [Header("预设缩放参数")]
    public bool needScale=false;//需要缩放
    public int scalePrecision;
    public int scaleOffset;
    [Header("预设正确位置")]
    
    public Vector3[] correctRotation;
    public Vector3[] correctTranslation;
    public Vector3[] correctScale;
    [Header("提示相关")]
    public float rotateHintUnit=90;
    public float transHintUnit=5;//默认提示参考距离
    [Header("是否需要固定玩家")]
    public bool needFix;
}
