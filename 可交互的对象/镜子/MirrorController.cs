using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MirrorState{UNCORRECT,OPERATING,CORRECT,STANDBY,FINISHED}
public enum EffectType{APPEAR,SEND}
public class MirrorController : MonoBehaviour
{
    [SerializeField] bool isTest;
    public bool isDebug;
    public LightHint hint;
    public MirrorStat_SO curMirrorStat;
    public MirrorStat_SO nxtMirrorStat{get {return curMirrorStat.nxtMirrorStat;}}
    //bool needRotate=false;//需要旋转
    //bool needTrans=false;//需要平移
    //bool needScale;//需要缩放
    [Header("普通预设参数")]
    public int nextButtonIndex = -1;
    public bool[] ANeedOperation;//需要完成的操作情况
    public bool[][] AcurOperation;//实际的操作情况，每行表示一种正确的操作组合，每列表示一种操作 0旋转1平移2缩放3楼梯4相机5按钮
    public int correctIndex=-1;//完成了第几个正确的解法
    public int curMirrorIndex=-1;//传过来的其余镜子处于正确的位置的索引
    public EffectType effectType;
    public bool moreMirror{get {return curMirrorStat.moreMirror;}}//是否还有下一面镜子要操作
    public bool needStair{get{return curMirrorStat.needStair;}}//需要楼梯位置进行配合
    public bool needCam{get{return curMirrorStat.needCam;}}//需要相机角度进行配合
    public bool needMirror{get{return curMirrorStat.needMirror;}}//需要其他镜子的配合
    public bool needButton{get{return curMirrorStat.needButton;}}//需要与其他按钮配合
    //public bool[] AneedStair;//记录接下来的操作分别需要什么东西的数组
    //public bool[] AneedCam;
    //public bool[] AneedMirror;
    //public bool[] AneedButton;
    public int solutionAmount{get{return curMirrorStat.solutionAmount;}}//解法的总和
    public bool needRotate{get{return curMirrorStat.needRotate;}}
    public float rotateUnit{get{return curMirrorStat.rotateUnit;}}//单次旋转的单位
    public float rotateOffset{get{return curMirrorStat.rotateOffset;}}//单位操作的偏移量
    public int rotatePrecision{get{return curMirrorStat.rotatePrecision;}}//保留的位数
    public float correctRotateSpeed{get{return curMirrorStat.correctRotateSpeed;}}//旋转矫正速度
    public bool needTrans{get{return curMirrorStat.needTrans;}}
    public float transUnit{get{return curMirrorStat.transUnit;}}
    public float transOffset{get{return curMirrorStat.transOffset;}}
    public int transPrecision{get{return curMirrorStat.transPrecision;}}
    public float correctTransSpeed{get{return curMirrorStat.correctTransSpeed;}}
    public int inMirrorLayer{get{return curMirrorStat.inMirrorLayer;}}
    public int outMirrorLayer{get{return curMirrorStat.outMirrorLayer;}}

    [Header("关联的游戏物体")]
    public Transform GappPar;//生成路径的父物体
    public Transform player;
    public RotatorController rotator;
    public TranslatorController translator;
    public CameraController cam;
    public RoadController[] stairs;
    public GameObject[] initializedRoad;//已经生成的路径的索引
    //[SerializeField] ScaleController scalator;
    [SerializeField] MirrorController[] connectMirrors;//关联的镜子
    public Quaternion rotateTarget;
    public Quaternion rotatorRTarget;
    public Vector3 tranlateTarget;

    [Header("实时记录的镜子属性")]
    public MirrorState mirrorState;
    public Vector3 curRotation;
    public Vector3 curTranslation;
    //public bool canSend=false;//镜子位置正确，可以传送玩家了
   //public bool hasSend=false;//是否已经传送了玩家
    public Vector3[] correctRotation{ get{return curMirrorStat.correctRotation;}}
    public Vector3[] correctTranslation{get{return curMirrorStat.correctTranslation;}}
    public Vector3[] correctCamRotation{get{return curMirrorStat.correctCamRotation;}}
    public double camOffset{get{return curMirrorStat.camOffset;}}
    //[SerializeField] Vector3[] correctScale;[SerializeField] Vector3 curScale;   [SerializeField] float differenceScale;
    public GameObject[] Gappear{get{return curMirrorStat.Gappear;}}
    //public GameObject[] Gdisappear{get{return curMirrorStat.Gdisappear;}}
    public Vector3[] playerPosInMirror{get{return curMirrorStat.playerPosInMirror;}}//玩家从镜子外进入镜子里时出现的位置
    //public Vector3[] playerPosOutMirror{ get{return curMirrorStat.playerPosOutMirror;}}//玩家从镜子里返回到镜子外时出现的位置
    public Vector3[] nextPos{ get{return curMirrorStat.nextPos;}} 
    Material Mmirror;//镜面材质

    private void Awake() {
        UpdateAttribute();
    }


    void Start(){
        InitAttribute();
    }

    void Update()
    {
        if(isDebug)debug();
        if(!isTest)
            DoAccordToState();
        else{
            UpdateAttribute();
        }
    }

    #region 初始化对象成员
    void InitAttribute(){
        ANeedOperation=new bool[6];
        AcurOperation=new bool[solutionAmount][];
        initializedRoad=new GameObject[solutionAmount];
        for(int i=0;i<solutionAmount;i++)    AcurOperation[i]=new bool[6];//初始化多种操作数组,注意不能直接=AneedOperation，此时传的是引用，而非重新复制一个数组
        ANeedOperation[0]=needRotate; ANeedOperation[1]=needTrans;
        ANeedOperation[3]=needStair; ANeedOperation[4]=needCam;
        ANeedOperation[5]=needButton;
        //Debug.Log(gameObject+" "+AcurOperation[1][0]+" "+solutionAmount);
        mirrorState=MirrorState.UNCORRECT;

        rotateTarget=Quaternion.Euler(curRotation);
        tranlateTarget=curTranslation;
    }
    #endregion

    #region 根据镜子状态执行相应操作
    void DoAccordToState(){
        switch(mirrorState){
            case MirrorState.UNCORRECT:{
                CorrectOperate();               
                UpdateAttribute();
                CheckCorrect();
                break;
            }
            case MirrorState.OPERATING:{
                UpdateAttribute();
                break;
            }
            case MirrorState.CORRECT:{
                UpdateAttribute();
                CheckCorrect();
                //HideThis();

                //if(effectType==EffectType.APPEAR)
                //    AppearObj();
                //else
                //    StartCoroutine(SendPlayerIn());
                //
                //if(needCam)cam.SetState(CameraStates.STANDBY);
                //if(needStair){
                //    foreach(var stair in stairs)
                //        stair.SetState(RoadState.STANBY);
                //}
                //UpdateOperation();
                break;
            }
            case MirrorState.STANDBY:
                break;
            case MirrorState.FINISHED:{
                //StartCoroutine(DestroyThis());
                break;
            }
        }
    }
    #endregion

    #region 切换状态时触发的操作
    //从旋转变为旋转结束
    void FromOPERATINGToUNCORRECT(){
        AutoRotate();
        AutoTrans();
        //AutoScale();
    }
    //从已正确显示路径变为再次开始旋转
    void FromSTANDBYToUNCORRECT(){
        //if(effectType==EffectType.SEND) SendPlayerOut();
        InitAttribute();
        ShowThis();
    }
    void FromUNCORRECTToCORRECT(){
        // 正确后设置下一个应该按的按钮
        if(SceneManager.GetActiveScene().buildIndex == 10)
        {
            Debug.Log("SC");
            ButtonHint.instance.currentIndex = nextButtonIndex;
        }      

        if(moreMirror){
            HideOperator();
        }

        AudioManager.PlayFlattenPathMusic();
        if(effectType==EffectType.APPEAR)
            AppearObj();
        else    StartCoroutine(SendPlayerIn());
        
        //if(needCam)cam.SetState(CameraStates.STANDBY);
        //if(needStair){
        //    foreach(var stair in stairs)
        //        stair.SetState(RoadState.STANBY);
        //}
    }
    void FromCORRECTToUNCORRECT(){
        //Debug.Log("恢复不正确状态");
        foreach(var road in initializedRoad){
            if(road!=null)  road.SetActive(false);
        } 
        correctIndex=-1;
        NavManager.BulidMesh();
        NavManager.PauseMove();
    }

    void ToFINISH(){
        StartCoroutine(DestroyThis());
    }
    #endregion

    #region 实时更新镜子的属性
    //更新镜子的三维
    void UpdateAttribute(){
        curRotation=new Vector3(KillDecimal(this.transform.eulerAngles.x,rotatePrecision),KillDecimal(this.transform.eulerAngles.y,rotatePrecision),KillDecimal(this.transform.eulerAngles.z,rotatePrecision));
        curTranslation=new Vector3(KillDecimal(this.transform.position.x,transPrecision),KillDecimal(this.transform.position.y,transPrecision),KillDecimal(this.transform.position.z,transPrecision));
        //curScale=this.transform.localScale;
    }
    //检测是否正确    
    void CheckCorrect(){
        if(needRotate){
            List<int> Rtmp=new List<int>();
            FindCorrectIndex(Rtmp,correctRotation,curRotation);
            ResetRow(AcurOperation,0);
            foreach(int it in Rtmp)
                AcurOperation[it][0]=true;
        }

        if(needTrans){
            List<int> Ttmp=new List<int>();
            FindCorrectIndex(Ttmp,correctTranslation,curTranslation);
            ResetRow(AcurOperation,1);
            foreach(int it in Ttmp)
                AcurOperation[it][1]=true;
        }

        if(needCam){
            List<int> Ttmp=new List<int>();
            FindAlmostCorrectIndex(Ttmp,correctCamRotation,cam.curRotation);
            ResetRow(AcurOperation,4);
            foreach(int it in Ttmp)
                AcurOperation[it][4]=true;
        }
//      if (needMirror){
//          int tmpIndex=FindCorrectIndex(AcurOperation,ANeedOperation);
//          foreach(var connectMirror in connectMirrors)
//              connectMirror.SetNeedMirror(false,tmpIndex);
//          if(tmpIndex>=0){
//               foreach(var connectMirror in connectMirrors)
//                  connectMirror.SetNeedMirror(true,tmpIndex);
//          } 
//      
//      }
        //for(int i=0;i<AcurOperation[0].Length;++i)           Debug.Log(i+" "+AcurOperation[0][i]+" "+ANeedOperation[i]);
        //if(needMirror&&correctIndex!=curMirrorIndex) return;//其余镜子没就位的话，不允许生成东西
        /*
        if(effectType==EffectType.SEND){
            if(FindMirrorCorrectIndex(AcurOperation,ANeedOperation)>=0) canSend=true;
            else{
                canSend=false;
                if(hasSend==true){
                    SendPlayerOut();
                    hasSend=false;
                }
            } 

        } 
        */
        correctIndex=FindAllCorrectIndex(AcurOperation,ANeedOperation);
        if(correctIndex>=0) {
            SetState(MirrorState.CORRECT);
        }
        else{
            SetState(MirrorState.UNCORRECT);
        } 
    }
    #endregion

    #region 设置旋转、平移、缩放的自动矫正位置
    void AutoRotate(){
        if(needRotate){
            float y=curRotation.y-rotateOffset;
            float z=curRotation.z-rotateOffset;
            switch(rotator.rotateAxix){
                case RotateDirection.HORIZONTAL:{
                    if((y%rotateUnit)/(rotateUnit/2)>1){
                        rotateTarget=Quaternion.Euler(curRotation.x,rotateUnit*((int)(y/rotateUnit)+1)+rotateOffset,curRotation.z);
                        //rotatorRTarget=Quaternion.Euler(rotator.transform.eulerAngles.x,rotateUnit*((int)(y/rotateUnit)+1)+rotateOffset,rotator.transform.eulerAngles.z);
                    }
                    else{
                        rotateTarget=Quaternion.Euler(curRotation.x,rotateUnit*((int)(y/rotateUnit))+rotateOffset,curRotation.z);  
                        //rotatorRTarget=Quaternion.Euler(rotator.transform.eulerAngles.x,rotateUnit*((int)(y/rotateUnit))+rotateOffset,rotator.transform.eulerAngles.z);
                    }
                    break;
                    }
                case RotateDirection.VERTICAL:{
                    if((z%rotateUnit)/(rotateUnit/2)>1){
                        rotateTarget=Quaternion.Euler(curRotation.x,curRotation.y,rotateUnit*((int)(z/rotateUnit)+1)+rotateOffset);
                        //rotatorRTarget=Quaternion.Euler(rotator.transform.eulerAngles.x,rotator.transform.eulerAngles.y,rotateUnit*((int)(z/rotateUnit)+1)+rotateOffset);
                    }
                    else{
                        rotateTarget=Quaternion.Euler(curRotation.x,curRotation.y,rotateUnit*((int)(z/rotateUnit))+rotateOffset);
                        //rotatorRTarget=Quaternion.Euler(rotator.transform.eulerAngles.x,rotator.transform.eulerAngles.y,rotateUnit*((int)(z/rotateUnit))+rotateOffset);
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
                   //不同于旋转，坐标大于0/小于0需要加以区分开
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
                   //不同于旋转，坐标大于0/小于0需要加以区分开
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

    /*void AutoScale(){
        if(needScale){
            switch(scalator.scaleDirection){
                case ScaleDirection.X:{
                    curScale=new Vector3(KillDecimal(curScale.x,scalePrecision)+scaleOffset,curScale.y,curScale.z);
                    break;
                }
            }
        }
    }*/
    #endregion

    #region 自动矫正
    void CorrectOperate(){
        var step= correctRotateSpeed*Time.deltaTime*5;
        if(needRotate)
            transform.rotation=Quaternion.RotateTowards(transform.rotation,rotateTarget,step);
        if(needTrans)
            this.transform.position=Vector3.MoveTowards(this.transform.position,tranlateTarget,correctTransSpeed*Time.deltaTime);
        if(rotator!=null)
            rotator.transform.rotation=Quaternion.RotateTowards(rotator.transform.rotation,rotatorRTarget,step);

    }
    #endregion

    #region 完成一次操作后对操作进行更新
    
    void UpdateOperation(){
        if(moreMirror){
            HideOperator();//
            curMirrorStat=nxtMirrorStat;
            SetState(MirrorState.STANDBY);
            return;
        }
        else{
            SetState(MirrorState.FINISHED);
        }
        /*if(AneedRotate.Length!=0) {needRotate=AneedRotate[curOperationIndex];ANeedOperation[0]=needRotate;}
        if(AneedTrans.Length!=0) {needTrans=AneedTrans[curOperationIndex];ANeedOperation[1]=needTrans;}
         //needScale=AneedScale[curOperationIndex];ANeedOperation[2]=needScale;
        if(AneedStair.Length!=0) {needStair=AneedStair[curOperationIndex];ANeedOperation[3]=needStair;}
        if(AneedCam.Length!=0) {needCam=AneedCam[curOperationIndex];ANeedOperation[4]=needCam;}
        if(AneedMirror.Length!=0) {needMirror=AneedMirror[curOperationIndex];ANeedOperation[5]=needMirror;}
        if(AneedButton.Length!=0) {needButton=AneedButton[curOperationIndex];ANeedOperation[6]=needButton;}
        for(int i=0;i<AcurOperation.Length;i++)
            AcurOperation[i]=false;
        SetState(MirrorState.STANDBY);*/
    }
    #endregion

    #region 完成操作后触发的效果
    void AppearObj(){
        //if(hint!=null) hint(false);
        if(Gappear.Length!=0&&Gappear[correctIndex]!=null) {
            if(initializedRoad[correctIndex]!=null) initializedRoad[correctIndex].SetActive(true);
            else {
                var obj=Instantiate(Gappear[correctIndex],GappPar);
                InitRoadsManager.AddRoad(obj);
                initializedRoad[correctIndex]=obj;
            }
        }  

        NavManager.BulidMesh();
        NavManager.EnableAgent();

        if(nextPos.Length!=0&&nextPos[correctIndex]!=(new Vector3(0,0,0))){
            NavManager.SetNxtPos(nextPos[correctIndex]);
        }
                if(moreMirror) UpdateOperation();
    }
    //将玩家传送至镜子里
    IEnumerator SendPlayerIn(){
        //hasSend=true;

        //if(hint!=null) hint.SwitchHintState(false);
        if(Gappear.Length!=0&&Gappear[correctIndex]!=null) {
            if(initializedRoad[correctIndex]!=null) initializedRoad[correctIndex].SetActive(true);
            else {
                var obj=Instantiate(Gappear[correctIndex],GappPar);
                InitRoadsManager.AddRoad(obj);
                initializedRoad[correctIndex]=obj;
            }
        } 

        NavManager.BulidMesh();
        NavManager.DisableAgent();
        ChangePlayerMaterial.BeginMakeTransparent();

        yield return new WaitForSeconds(1f);
               
        Transform[] trans=player.GetComponentsInChildren<Transform>();
        foreach(Transform tran in trans) tran.gameObject.layer=inMirrorLayer;
        player.transform.position = playerPosInMirror[correctIndex]; 
        player.transform.SetParent(null);
        NavManager.EnableAgent();
        ChangePlayerMaterial.BeginMakeUntransparent();

        yield return new WaitForSeconds(1f);

        if(nextPos.Length!=0&&nextPos[correctIndex]!=(new Vector3(0,0,0))){
            NavManager.SetNxtPos(nextPos[correctIndex]);
        } 
        if(moreMirror) UpdateOperation();

    }
    #endregion

    #region 关闭与打开镜子
    //重新显示镜子
    void ShowThis(){
        this.GetComponent<Alpha>().toTransparent=false;
    }
    //关闭镜子
    void HideOperator(){
        if(rotator!=null)   rotator.gameObject.SetActive(false);
        if(translator!=null)translator.gameObject.SetActive(false);
        //AudioManager.PlayflattenPathMusic();
    }
    IEnumerator DestroyThis(){
        HideOperator();
        this.GetComponent<Alpha>().toTransparent=true;
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);
        
    }
    #endregion

    #region 与其他对象的通信
    //分别设置楼梯、相机、镜子是否处于正确位置
    public void SetNeedStair(bool b,List<int> corIndex){
        if(b==false) ResetRow(AcurOperation,3);
        else 
            foreach(int it in corIndex) AcurOperation[it][3]=true;
    }

    public void SetNeedCam(bool b,List<int> corIndex){
        if(b==false) ResetRow(AcurOperation,4);
        else {
            foreach(int it in corIndex) AcurOperation[it][4]=true;
        }
    }

    public void SetNeedMirror(bool b,int corIndex){
        if(b==false)    curMirrorIndex=-1;
        else curMirrorIndex=corIndex;
    }

    public void SetNeedButton(bool b){
        //Debug.Log(solutionAmount);
        for(int i=0;i<solutionAmount;++i){
            //Debug.Log(i);
            AcurOperation[i][5]=b;
        }
    }

    public MirrorState GetState(){
        return mirrorState;
    }
    public void SetState(MirrorState state){
        if(mirrorState==MirrorState.OPERATING&&state==MirrorState.UNCORRECT) FromOPERATINGToUNCORRECT();
        if(mirrorState==MirrorState.STANDBY&&state==MirrorState.UNCORRECT) FromSTANDBYToUNCORRECT();
        if((mirrorState==MirrorState.UNCORRECT||mirrorState==MirrorState.OPERATING)&&state==MirrorState.CORRECT)FromUNCORRECTToCORRECT();
        if(mirrorState==MirrorState.CORRECT&&(state==MirrorState.OPERATING||state==MirrorState.UNCORRECT))FromCORRECTToUNCORRECT();
        if(state==MirrorState.FINISHED) ToFINISH();
        mirrorState=state;
//        Debug.Log(mirrorState);
    }

    public void SetEffect(EffectType type){
        effectType=type;
    }
    #endregion

    #region 辅助操作
    //将某种操作全部置为不正确，在玩家操作错误时调用
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
        if(corOpe==null||curOpe==null) return;

        for(int i=0;i<solutionAmount;i++)
            if(curOpe==corOpe[i])
                list.Add(i);
    }
    //检测两个浮点数向量是否在误差范围内
    void FindAlmostCorrectIndex(List<int>list, Vector3[] corOpe,Vector3 curOpe){
        if(corOpe==null||curOpe==null) return;

        for(int i=0;i<solutionAmount;i++)
            if((curOpe-corOpe[i]).magnitude<camOffset)
                list.Add(i);
    }

    //检测除配合的镜子外所有条件是否已满足
    int FindAllCorrectIndex(bool[][] Acur,bool[] Aneed){
        if(Acur==null||Aneed==null) return -1;

        for(int i=0;i<solutionAmount;i++)
            if(CompareArray(Acur[i],Aneed))
                return i;
        
        return -1;
    }
    //检查镜子本身的旋转平移是否处于正确状态，以设定传送玩家
    int FindMirrorCorrectIndex(bool[][] Acur,bool[] Aneed){
        if(Acur==null||Aneed==null) return -1;

        for(int i=0;i<solutionAmount;i++)
            if(Acur[i][0]==Aneed[0]&&Acur[i][1]==Aneed[1])
                return i;
        
        return -1;
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

    void debug(){
        for(int i=0;i<6;++i){
            Debug.Log(AcurOperation[0][i]+" "+ANeedOperation[i]);
        }
    }
    #endregion
    
}
