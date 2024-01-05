using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//控制相机视角的旋转脚本

public enum CameraStates{UNCORRECT,CORRECT,STANDBY,FINISH}
public class CameraController : MonoBehaviour
{
    [Header("预设参数")]
    public CameraStat_SO curCameraStat;
    public MirrorController[] mirrors;
    public int solutionAmount{get {return curCameraStat.solutionAmount;}}
    public CameraStat_SO nxtCameraStat{get {return curCameraStat.nxtCameraStat;}}
    public bool moreCamera{get {return curCameraStat.moreCamera;}}//是否还要继续移动摄像机
    public Vector3[] correctRotations{get {return curCameraStat.correctRotations;}}
    [Header("实时属性")]
    public List<int> correctIndex;//所有满足正确位置的索引
    public Vector3 curRotation;
    public int curOperationIndex=-1;
    public int OperationAmount;
    public CameraStates cameraState;

    private void Update() {
        UpdateAttribute();
       // DoAccordToState();
    }

    #region 更新参数
    void UpdateAttribute(){
        curRotation=new Vector3(KillDecimal(this.transform.eulerAngles.x,0),KillDecimal(this.transform.eulerAngles.y,0),KillDecimal(this.transform.eulerAngles.z,0));
    }
    #endregion

    #region 更新状态
    void CheckCorrect(){
        FindCorrectIndex(correctIndex,correctRotations,curRotation);
        //Debug.Log(correctIndex.Count);
        if(correctIndex.Count!=0)SetState(CameraStates.CORRECT);
        else SetState(CameraStates.UNCORRECT);
    }
    #endregion

    #region 根据状态执行操作
    void DoAccordToState(){
        switch(cameraState){
            case CameraStates.UNCORRECT:{
                CheckCorrect();                
                break;
            }
            case CameraStates.CORRECT:{
                CheckCorrect();                
                break;
            }
            case CameraStates.STANDBY:{
                SetState(CameraStates.UNCORRECT);
                break;
            }
            case CameraStates.FINISH:{
                break;
            }
        }
    }

    #endregion

    #region 切换状态时触发的操作
    void FromCorrectToSTANBY(){
        UpdateOperation();
    }
    void FromUNCORRECTToCORRECT(){
        //Debug.Log("相机位置正确");
        //if(mirrors[curOperationIndex]!=null&&mirrors[curOperationIndex].isActiveAndEnabled)
        //mirrors[curOperationIndex].SetNeedCam(true,correctIndex);
    }
    void FromCORRECTToUNCORRECT(){
       // Debug.Log("触发相机不正确");
        //if(mirrors[curOperationIndex]!=null&&mirrors[curOperationIndex].isActiveAndEnabled)
        //mirrors[curOperationIndex].SetNeedCam(false,correctIndex);
    }
    #endregion

    #region 完成一次操作后对操作进行更新
    void UpdateOperation(){
        ++curOperationIndex;
        if(!moreCamera||curOperationIndex>=OperationAmount){
            SetState(CameraStates.FINISH);
        }
        else{
            curCameraStat=nxtCameraStat;
            cameraState=CameraStates.STANDBY;
        }
    }
    #endregion

    #region 与其他对象通信
    public void SetState(CameraStates state){
        if(cameraState==CameraStates.CORRECT&&state==CameraStates.STANDBY) {
            FromCorrectToSTANBY();
            return;
        }
        else if(cameraState==CameraStates.CORRECT&&state==CameraStates.UNCORRECT) FromCORRECTToUNCORRECT();
        else if(cameraState==CameraStates.UNCORRECT&&state==CameraStates.CORRECT) FromUNCORRECTToCORRECT();
        cameraState=state;
    }
    #endregion

    #region 辅助操作
    void ResetRow(bool[][]array,int columnIndex ){
        if(array==null||columnIndex<0) return;

        for(int i=0;i<solutionAmount;i++)
            array[i][columnIndex]=false;

        return;
    }
    void ResetRow(bool[]array){
        if(array==null) return;

        for(int i=0;i<solutionAmount;i++)
            array[i]=false;

        return;
    }
    //检测当前的位置是否满足其中一种正确的情况
    void FindCorrectIndex(List<int>list, Vector3[] corOpe,Vector3 curOpe){
        list.Clear();
        if(corOpe==null||curOpe==null) return;

        for(int i=0;i<solutionAmount;i++)
            if(curOpe==corOpe[i])
                list.Add(i);
    }

    //检测除配合的镜子外所有条件是否已满足
    void FindCorrectIndex(List<int>corI, bool[][] Acur,bool[] Aneed){
        corI.Clear();
        if(Acur==null||Aneed==null) return;

        for(int i=0;i<solutionAmount;i++)
            if(CompareArray(Acur[i],Aneed))
                corI.Add(i);
        
        return;
    }

    //因为数组是引用类型，不能直接比较，所以要另写一个函数比较数组中的每个值，来判断两个数组是否相等
    bool CompareArray(bool[] array1,bool[] array2){
        if(array1.Length!=array2.Length) return false;

        for(int i=0;i<array1.Length;i++)
            if(array1[i]!=array2[i])
                return false;

        return true;
    }
    float KillDecimal(float num,int digit){
        float vt = Mathf.Pow (10, digit);
        //1.乘以倍数 + 0.5
        float vx = num * vt + 0.5f;
        //2.向下取整
        float temp = Mathf.Floor (vx);
        //3.再除以倍数
        return (temp / vt);
    }
    #endregion
      
}
