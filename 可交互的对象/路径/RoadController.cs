using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//注意正确的路径位置索引，要和镜子对上号！
public enum RoadState{UNCORRECT,OPERATING,CORRECT,STANBY,FINISHED}
public class RoadController : MonoBehaviour
{
    public bool isTest;

    [Header("普通预设参数")]

    public RoadStat_SO curRoadStat;
    public RoadStat_SO nxtRoadStat{get{return curRoadStat.nxtRoadStat;}}
    public bool moreRoad{get{return curRoadStat.moreRoad;}}
    public List<int> correctIndex;//正确操作的所有索引
    public int solutionAmount{get{return curRoadStat.solutionAmount;}}//总操作的数量
    public bool[][] AcurOperation;//实际的操作情况
    public bool[] ANeedOperation;//需要完成的操作情况

    public bool needRotate{get{return curRoadStat.needRotate;}}//需要旋转
    public float rotateUnit{get{return curRoadStat.rotateUnit;}}//单次旋转的单位
    public int rotatePrecision{get{return curRoadStat.rotatePrecision;}}//保留的小数点位数
    public float rotateOffset{get{return curRoadStat.rotateOffset;}}//单位操作的偏移量
    public float correctRotateSpeed{get{return curRoadStat.correctRotateSpeed;}}//旋转矫正速度
    public bool needTrans{get{return curRoadStat.needTrans;}}//需要平移
    public float transUnit{get{return curRoadStat.transUnit;}}
    public int transPrecision{get{return curRoadStat.transPrecision;}}
    public float transOffset{get{return curRoadStat.transOffset;}}
    public float correctTransSpeed{get{return curRoadStat.correctTransSpeed;}}
    public bool needScale{get{return curRoadStat.needScale;}}//需要缩放
    public int scalePrecision{get{return curRoadStat.scalePrecision;}}
    public int scaleOffset{get{return curRoadStat.scaleOffset;}}


    [Header("关联的游戏物体")]
    public Transform player;
    public MirrorController[] mirrors;
    public RotatorController rotator;
    public TranslatorController translator;
    public ScaleController scalator;
    public Quaternion rotateTarget;
    Quaternion rotatorRTarget;
    public Vector3 tranlateTarget;
    [Header("实时记录的路径属性")]
    public RoadState roadState;
    public Vector3[] correctRotation{get{return curRoadStat.correctRotation;}}
    public Vector3 curRotation;
    public Vector3[] correctTranslation{get{return curRoadStat.correctTranslation;}}
    public Vector3 curTranslation;
    public Vector3[] correctScale{get{return curRoadStat.correctScale;}}
    public Vector3 curScale;   

    [Header("是否需要固定玩家及相应操作")]
    public Transform playerParent;
    public bool ableToFix;
    public bool needFix{get{return curRoadStat.needFix;}}

    private void Start() {
        UpdateAttribute();
        InitAttribute(); 
    }
    void Update()
    {
        if(!isTest)
            DoAccordToState();
        else{
            UpdateAttribute();
        }
    }

    #region 初始化对象成员
    void InitAttribute(){
        correctIndex=new List<int>();
        ANeedOperation=new bool[3];
        AcurOperation=new bool[solutionAmount][];
        for(int i=0;i<solutionAmount;i++)    AcurOperation[i]=new bool[3];//初始化多种操作数组,注意不能直接=AneedOperation，此时传的是引用，而非重新复制一个数组
        ANeedOperation[0]=needRotate; 
        ANeedOperation[1]=needTrans;
        ANeedOperation[2]=needScale;

        roadState=RoadState.UNCORRECT;
        
        rotateTarget=Quaternion.Euler(curRotation);
        tranlateTarget=curTranslation;  
    }
    #endregion

    #region 根据路径状态执行相应操作
    void DoAccordToState(){
        switch(roadState){
            case RoadState.UNCORRECT:{
                //UnFixPlayer();
                CorrectOperate();               
                UpdateAttribute();
                CheckCorrect();
                break;
            }
            case RoadState.OPERATING:{
                UpdateAttribute();
                FixPlayer();
                break;
            }
            case RoadState.CORRECT:{
                //UnFixPlayer();               
                //NavManager.BulidMesh();
                break;
            }
            case RoadState.STANBY:{
                break;
            }
            case RoadState.FINISHED:{
                break;
            }
        }
    }
    #endregion

    #region 切换状态时触发的操作
    //从操作中变为操作结束
    void FromUNCORRECTToOperating(){
        FixPlayer(); 
    }
    void FromOPERATINGToUNCORRECT(){
        AutoRotate();
        AutoTrans();
        AutoScale();
    }

    void FromCORRECTToOPERATING(){
        foreach(var mirror in mirrors)
            mirror.SetNeedStair(false,null);
    }

    void FromUNCORRECTToCORRECT(){
        foreach(var mirror in mirrors)
            mirror.SetNeedStair(true,correctIndex);
        NavManager.BulidMesh();
        UnFixPlayer();
        if(moreRoad)
            UpdateOperation();
    }

    void FromCORRECTToSTANBY(){
        //Debug.Log("yes");
        //CloseOperator();
        UpdateOperation();
    }
    void ToFINISH(){
        //Debug.Log("消灭他们");
        Destroy(this);
        if(rotator!=null) Destroy(rotator.gameObject);
        if(translator!=null) Destroy(translator.gameObject);
        if(scalator!=null) Destroy(scalator.gameObject);
    }
    #endregion
    
    #region 完成一次操作后对操作进行更新
    void UpdateOperation(){
        if(moreRoad){
            curRoadStat=nxtRoadStat;
            InitAttribute(); 
            SetState(RoadState.UNCORRECT);
            return;
        }
        else{
            SetState(RoadState.FINISHED);
        }
        /*
        if(AneedRotate.Length!=0) needRotate=AneedRotate[curOperationIndex];ANeedOperation[0]=needRotate;
        if(AneedTrans.Length!=0) needTrans=AneedTrans[curOperationIndex];ANeedOperation[1]=needTrans;
        if(AneedScale.Length!=0) needScale=AneedScale[curOperationIndex];ANeedOperation[2]=needScale;
        for(int i=0;i<AcurOperation.Length;i++)
            AcurOperation[i]=false;
            */
        SetState(RoadState.UNCORRECT);
    }
    #endregion

    #region 关闭或打开控制路径操作的物体
    void CloseOperator(){
        if(rotator!=null) rotator.gameObject.SetActive(false);
        if(translator!=null) translator.gameObject.SetActive(false);
        if(scalator!=null) scalator.gameObject.SetActive(false);
    }

    void OpenOperator(){
        if(rotator!=null) rotator.gameObject.SetActive(true);
        if(translator!=null) translator.gameObject.SetActive(true);
        if(scalator!=null) scalator.gameObject.SetActive(true);
    }
    #endregion

    #region 固定玩家
    void FixPlayer(){
        if(needFix&&ableToFix){
            NavManager.DisableAgent();
            player.SetParent(playerParent);
        }
    }

    void UnFixPlayer(){
        if(needFix){
            NavManager.EnableAgent();
            player.SetParent(null);
        } 
    }

    void OnTriggerStay(Collider other){
        if(other.gameObject.CompareTag("Player"))
            ableToFix=true;
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player"))
            ableToFix=false;
    }
    #endregion

    #region 实时更新路径的属性
    void UpdateAttribute(){
        curRotation=new Vector3(KillDecimal(this.transform.eulerAngles.x,rotatePrecision),KillDecimal(this.transform.eulerAngles.y,rotatePrecision),KillDecimal(this.transform.eulerAngles.z,rotatePrecision));
        curTranslation=new Vector3(KillDecimal(this.transform.position.x,transPrecision),KillDecimal(this.transform.position.y,transPrecision),KillDecimal(this.transform.position.z,transPrecision));
        curScale=new Vector3(KillDecimal(this.transform.localScale.x,scalePrecision),KillDecimal(this.transform.localScale.y,scalePrecision),KillDecimal(this.transform.localScale.z,scalePrecision));       
    }

    void CheckCorrect(){
        if(needRotate){
            List<int> tmpIndex=new List<int>();
            FindCorrectIndex(tmpIndex,correctRotation,curRotation);
            ResetRow(AcurOperation,0);
            foreach(int it in tmpIndex)
                AcurOperation[it][0]=true;
        }
        if(needTrans){
            List<int> tmpIndex=new List<int>();
            FindCorrectIndex(tmpIndex,correctTranslation,curTranslation);
            ResetRow(AcurOperation,1);
            foreach(int it in tmpIndex)
                AcurOperation[it][1]=true;
        }
        if(needScale){
            List<int> tmpIndex=new List<int>();
            FindCorrectIndex(tmpIndex,correctScale,curScale);
            ResetRow(AcurOperation,2);
            foreach(int it in tmpIndex)
                AcurOperation[it][2]=true;
        }
        FindCorrectIndex(correctIndex,AcurOperation,ANeedOperation);
        if(correctIndex.Count!=0) SetState(RoadState.CORRECT);
        else SetState(RoadState.UNCORRECT);
    }
    #endregion

    #region 自动矫正
    void CorrectOperate(){
        var step= correctRotateSpeed*Time.deltaTime*1;
        if(needRotate){
            transform.rotation=Quaternion.RotateTowards(transform.rotation,rotateTarget,step);
        }
        if(needTrans){
            this.transform.position=Vector3.MoveTowards(curTranslation,tranlateTarget,correctTransSpeed*Time.deltaTime);
        }
        //if(rotator!=null)rotator.transform.rotation=Quaternion.RotateTowards(rotator.transform.rotation,rotatorRTarget,step);
    }
    #endregion

    #region 设置旋转、平移、缩放的自动矫正
    void AutoRotate(){
        if(needRotate){
            float x=curRotation.x;
            float y=curRotation.y;
            float z=curRotation.z;
            switch(rotator.GetComponent<RotatorController>().rotateAxix){
                case RotateDirection.HORIZONTAL:{
                    if(((y-rotateOffset)%rotateUnit)/(rotateUnit/2)>1){
                        rotateTarget=Quaternion.Euler(curRotation.x,rotateUnit*((int)((y-rotateOffset)/rotateUnit)+1)+rotateOffset,curRotation.z);
                        rotatorRTarget=Quaternion.Euler(rotator.transform.eulerAngles.x,rotateUnit*((int)((y-rotateOffset)/rotateUnit)+1)+rotateOffset,rotator.transform.eulerAngles.z);
                    }
                    else{
                        rotateTarget=Quaternion.Euler(curRotation.x,rotateUnit*((int)((y-rotateOffset)/rotateUnit))+rotateOffset,curRotation.z);  
                        rotatorRTarget=Quaternion.Euler(rotator.transform.eulerAngles.x,rotateUnit*((int)((y-rotateOffset)/rotateUnit))+rotateOffset,rotator.transform.eulerAngles.z);
                    }
                    break;
                    }
                case RotateDirection.VERTICAL:{
                    if(((z-rotateOffset)%rotateUnit)/(rotateUnit/2)>1){
                        rotateTarget=Quaternion.Euler(curRotation.x,curRotation.y,rotateUnit*((int)((z-rotateOffset)/rotateUnit)+1));
                        rotatorRTarget=Quaternion.Euler(rotator.transform.eulerAngles.x,rotator.transform.eulerAngles.y,rotateUnit*((int)((z-rotateOffset)/rotateUnit)+1));
                    }
                    else{
                        rotateTarget=Quaternion.Euler(curRotation.x,curRotation.y,rotateUnit*((int)((z-rotateOffset)/rotateUnit)));
                        rotatorRTarget=Quaternion.Euler(rotator.transform.eulerAngles.x,rotator.transform.eulerAngles.y,rotateUnit*((int)((z-rotateOffset)/rotateUnit)));
                    }
                    break;
                }
                case RotateDirection.RIGHT:{
                    if(((x-rotateOffset)%rotateUnit)/(rotateUnit/2)>1){
                        rotateTarget=Quaternion.Euler(rotateUnit*((int)((x-rotateOffset)/rotateUnit)+1)+rotateOffset,curRotation.y,curRotation.z);
                        rotatorRTarget=Quaternion.Euler(rotateUnit*((int)((x-rotateOffset)/rotateUnit)+1)+rotateOffset,curRotation.y,rotator.transform.eulerAngles.z);
                    }
                    else{
                        rotateTarget=Quaternion.Euler(rotateUnit*((int)((x-rotateOffset)/rotateUnit))+rotateOffset,curRotation.y,curRotation.z);  
                        rotatorRTarget=Quaternion.Euler(rotateUnit*((int)((x-rotateOffset)/rotateUnit))+rotateOffset,curRotation.y,rotator.transform.eulerAngles.z);
                    }
                    break;
                    }
            }
        }
    }

    public Quaternion GetRotateTarget(){
        return rotateTarget;
    }

    public Vector3 GetTransTarget(){
        return tranlateTarget;
    }

    void AutoTrans(){
        if(needTrans){
           float x=curTranslation.x-transOffset;
           float y=curTranslation.y-transOffset;
           float z=curTranslation.z-transOffset;
           switch(translator.translateDirection){
               case TranslateDirection.X:{
                   //不同于旋转，坐标大于0/小于0需要加以区分开
                    if((Mathf.Abs(x)%transUnit)/(transUnit/2)>1){
                        if(x>0)
                            tranlateTarget=new Vector3(transUnit*((int)(x/transUnit)+1)+transOffset,curTranslation.y,curTranslation.z);
                        else
                            tranlateTarget=new Vector3(transUnit*((int)(x/transUnit)-1)+transOffset,curTranslation.y,curTranslation.z);
                    }
                    else{
                        tranlateTarget=new Vector3(transUnit*((int)(x/transUnit))+transOffset,curTranslation.y,curTranslation.z);
                    }
                    break;
               }
               case TranslateDirection.Y:{
                    if((Mathf.Abs(y)%transUnit)/(transUnit/2)>1){
                        if(y>0)
                            tranlateTarget=new Vector3(curTranslation.x,transUnit*((int)(y/transUnit)+1)+transOffset,curTranslation.z);
                        else
                            tranlateTarget=new Vector3(curTranslation.x,transUnit*((int)(y/transUnit)-1)+transOffset,curTranslation.z);
                    }
                    else{
                        tranlateTarget=new Vector3(curTranslation.x,transUnit*((int)(y/transUnit))+transOffset,curTranslation.z);
                    }
                    break;
               }
               case TranslateDirection.Z:{
                    if((Mathf.Abs(z)%transUnit)/(transUnit/2)>1){
                        if(z>0)
                            tranlateTarget=new Vector3(curTranslation.x,curTranslation.y,transUnit*((int)(z/transUnit)+1)+transOffset);
                        else
                            tranlateTarget=new Vector3(curTranslation.x,curTranslation.y,transUnit*((int)(z/transUnit)-1)+transOffset);
                    }
                    else{
                        tranlateTarget=new Vector3(curTranslation.x,curTranslation.y,transUnit*((int)(z/transUnit))+transOffset);
                    }
                    break;
               }
           }
        }
    }

    void AutoScale(){//
        if(needScale){
            //FIXME:需要更改
            switch(scalator.scaleDirection){
                case ScaleDirection.X:{
                    this.transform.localScale=new Vector3(KillDecimal(curScale.x,scalePrecision)+scaleOffset,curScale.y,curScale.z);
                    break;
                }
            }
        }
    }
    #endregion

    #region 状态切换
    public void SetState(RoadState state){
        if(roadState==RoadState.OPERATING&&state==RoadState.UNCORRECT) FromOPERATINGToUNCORRECT();
        else if(roadState==RoadState.CORRECT&&state==RoadState.OPERATING) FromCORRECTToOPERATING();
        else if((roadState==RoadState.UNCORRECT||roadState==RoadState.OPERATING)&&state==RoadState.CORRECT) FromUNCORRECTToCORRECT();
        else if(roadState==RoadState.CORRECT&&state==RoadState.STANBY)FromCORRECTToSTANBY();
        else if(roadState==RoadState.UNCORRECT&&state==RoadState.OPERATING)FromUNCORRECTToOperating();
        else if(state==RoadState.FINISHED) ToFINISH();
        roadState=state;
    }
    #endregion

    #region 与其他对象的通信
    public void SetTranslator(TranslatorController _translator){
        translator=_translator;
    }

    /*
    public void SetMirror(MirrorController _mirror){
        foreach(var mirror in mirrors)
            mirror=_mirror;
    }*/
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
