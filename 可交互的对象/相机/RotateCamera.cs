using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public int rotateIndex;//当前旋转到哪个位置的下标
    [SerializeField] int leftBorder;//左旋转边界
    [SerializeField] int rightBorder;//右旋转边界
    [SerializeField] float rotateUnit;//每次旋转的角度
    [SerializeField] float rotateSpeed;
    [SerializeField] RotateDirection rotateDirection;
    public bool canRotate=false;
    public Quaternion rotateTarget;

    private void Start() {
        
        rotateTarget=Quaternion.Euler(KillDecimal(this.transform.eulerAngles.x,0),KillDecimal(this.transform.eulerAngles.y,0),KillDecimal(this.transform.eulerAngles.z,0));;
    }
    private void Update() {
        CanRotate();
        if(canRotate)   SetRotateTarget();
        CorrectOperate();
    }
    #region 是否可以旋转
    void CanRotate(){
        if((Mathf.Abs((this.transform.rotation.eulerAngles-rotateTarget.eulerAngles).magnitude)<5f)
                ||(360-Mathf.Abs((this.transform.rotation.eulerAngles-rotateTarget.eulerAngles).magnitude)<5f))
                canRotate=true;
        else canRotate=false;//针对马上到360度的情况，此时插值会到300度以上
    }
    #endregion
    
    #region 自动旋转至目标位置
    void CorrectOperate(){
        this.transform.rotation=Quaternion.Lerp(this.transform.rotation,rotateTarget,Time.deltaTime*rotateSpeed);
    }
    #endregion

    #region 设置相机的左右旋转目标位置(Q向左，E向右)
    void SetRotateTarget(){
        if(Input.GetKeyDown(KeyCode.Q)&&rotateIndex>leftBorder){
            --rotateIndex;
            switch(rotateDirection){
                case RotateDirection.VERTICAL:{
                    rotateTarget=Quaternion.Euler(rotateTarget.x-rotateUnit,rotateTarget.y,rotateTarget.z);
                    break;
                }
                case RotateDirection.HORIZONTAL:{
                    rotateTarget=Quaternion.Euler(rotateTarget.x,rotateTarget.eulerAngles.y-rotateUnit,rotateTarget.z);
                    break;
                }
            }
        }
        else if(Input.GetKeyDown(KeyCode.E)&&rotateIndex<rightBorder){
            ++rotateIndex;
            switch(rotateDirection){
                case RotateDirection.VERTICAL:{
                    rotateTarget=Quaternion.Euler(rotateTarget.x+rotateUnit,rotateTarget.y,rotateTarget.z);
                    break;
                }
                case RotateDirection.HORIZONTAL:{
                    rotateTarget=Quaternion.Euler(rotateTarget.x,rotateTarget.eulerAngles.y+rotateUnit,rotateTarget.z);
                    break;
                }
            }
        }
    }
    #endregion

    float KillDecimal(float num,int digit){
        float vt = Mathf.Pow (10, digit);
        //1.乘以倍数 + 0.5
        float vx = num * vt + 0.5f;
        //2.向下取整
        float temp = Mathf.Floor (vx);
        //3.再除以倍数
        return (temp / vt);
    }
}
