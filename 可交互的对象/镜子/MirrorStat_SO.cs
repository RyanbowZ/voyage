using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Mirror Data",menuName ="New Data/New Obj Data/New Mirror Data")]
public class MirrorStat_SO : ScriptableObject
{
    [Header("普通预设参数")]
    public MirrorStat_SO nxtMirrorStat;
    public bool moreMirror;//是否还有下一面镜子要操作
    public bool needStair=false;//需要楼梯位置进行配合
    public bool needMirror=false;//需要其他镜子的配合
    public bool needButton=false;//需要与其他按钮配合
    public int solutionAmount;
    [Header("旋转预设参数")]
    public bool needRotate;
    public float rotateUnit;//单次旋转的单位
    public float rotateOffset;//单位操作的偏移量
    public int rotatePrecision;//保留的位数
    public float correctRotateSpeed=5f;//旋转矫正速度
    [Header("平移预设参数")]
    public bool needTrans;
    public float transUnit;
    public float transOffset;
    public int transPrecision;
    public float correctTransSpeed=1f;
    [Header("旋转视角参数")]
    public bool needCam=false;//需要相机角度进行配合
    public double camOffset=3f;//当摄像机角度在这个误差范围内时，正确
    public Vector3[] correctCamRotation;
    [Header("镜子里外传送相关参数")]
    public int inMirrorLayer=10;//传送进镜子里后层级的变化
    public int outMirrorLayer=12;//传送出镜子外后层级的变化
    [Header("判断位置正确及效果")]
    public Vector3[] correctRotation;
    public Vector3[] correctTranslation;
    public GameObject[] Gappear;
    //public GameObject[] Gdisappear;
    public Vector3[] playerPosInMirror;
    //public Vector3[] playerPosOutMirror;
    [Header("玩家将要前往的位置")]
    public Vector3[] nextPos;
    /*
    [Header("提示相关")]
    public float rotateHintUnit=90;
    public float transHintUnit=5;//默认提示参考距离
    */
}
