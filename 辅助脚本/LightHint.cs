using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHint : MonoBehaviour
{
    /*
    public MirrorController mirror;
    public RoadController road;
    public CameraController cam;
    public GameObject[] affectObj;//会发光提示的对象
    public Material[] curMat;//发光提示对象当前的材质
    public Material[] defaultMat;//路径默认材质
    public Material lightMat;//路径发光材质
    public bool needMirrorRotate{get{if(mirror!=null)return mirror.curMirrorStat.needRotate;else return false;}}
    //public float mirrorRotateHintUnit{get{if(mirror)return mirror.curMirrorStat.rotateHintUnit;else return 0;}}
    public Vector3[] mirrorCorrectRotation{get{if(mirror!=null)return mirror.curMirrorStat.correctRotation;else return null;}}
    public Vector3 mirrorCurRotation{get{if(mirror!=null)return mirror.curRotation;else return new Vector3(0,0,0);}}
    public bool needMirrorTrans{get{if(mirror!=null)return mirror.curMirrorStat.needTrans;else return false;}}
    //public float mirrorTransHintUnit{get{if(mirror)return mirror.curMirrorStat.transHintUnit;else return 0;}}
    public bool needMirrorCam{get{if(mirror!=null)return mirror.curMirrorStat.needCam;else return false;}}
    public Vector3[] camCorrectRotation{get{if(mirror!=null&&cam!=null)return cam.correctRotations;else return null;}}
    public Vector3 camCurRotation{get{if(mirror!=null&&cam!=null)return cam.curRotation;else return new Vector3(0,0,0);}}
    public bool needStair{get{if(mirror!=null)return mirror.curMirrorStat.needStair;else return false;}}
    public bool needStairRotate{get{if(road!=null&&needStair)return road.curRoadStat.needRotate;else return false;}} 
    public bool needStairTrans{get{if(road!=null&&needStair)return road.curRoadStat.needTrans;else return false;}}
    public float stairTransHintUnit{get{if(road!=null&&needStair)return road.curRoadStat.transHintUnit;else return 0;}}
    public Vector3[] stairCorrectTranslation{get{if(road!=null&&needStair)return road.curRoadStat.correctTranslation;else return null;}}
    public Vector3 stairCurTranslation{get{if(road!=null&&needStair)return road.curTranslation;else return new Vector3(0,0,0);}}
    public bool needStairScale{get{if(road!=null&&needStair)return road.curRoadStat.needScale;else return false;}}
    public int calAmount{get{return(needMirrorRotate?1:0)+(needMirrorTrans?1:0)+(needStairRotate?1:0)+(needStairTrans?1:0)+(needStairScale?1:0);}}//百分比最后除以的分母，因为每项操作默认发光提示是1

   #region 闪烁设置
    [Header("闪烁时间设置")]
    public float onceTwinkleTime;                       //单次闪烁花费的时间/秒
    public float totalTwinkleTime;                      //按下提示后闪烁的总时长
    public float twinkleDensityRnage;                   //亮度变化最大值
    float m_twinkleOnceTimeCounter;                     //单次闪烁计时器
    float m_twinkleTotalTimeCounter;                    //总闪烁计时器
    float m_twinkleDensityCounter;                      //闪烁亮度变化记录
    float m_twinkleDensityStart;                        //初始亮度
    bool m_isTwinkling = false;

    void Twinkle(Material targetMaterial)
    {
        m_twinkleDensityStart = 4f; //初始亮度

        if((m_twinkleTotalTimeCounter < totalTwinkleTime) && m_isTwinkling)
        {
            m_twinkleDensityCounter = TwinkleOnceTimeChange(m_twinkleOnceTimeCounter);
            
            lightMat.SetFloat("_AlphaPower",m_twinkleDensityStart + m_twinkleDensityCounter);

            m_twinkleTotalTimeCounter += Time.deltaTime;
            m_twinkleOnceTimeCounter = m_twinkleTotalTimeCounter % onceTwinkleTime;
        }
        else
        {
            m_twinkleTotalTimeCounter = 0;
            m_isTwinkling = false;
        }
    }
    float TwinkleOnceTimeChange(float currentTime) //模拟单次闪烁的亮度周期
    {
        currentTime = Mathf.Clamp(currentTime, 0f, onceTwinkleTime);
        return Mathf.Sin(currentTime / onceTwinkleTime * 2f *Mathf.PI) * twinkleDensityRnage;
    }
    #endregion

    #region 初始化提示信息
    void Start(){
        curMat=new Material[affectObj.Length];
        defaultMat=new Material[affectObj.Length];
        for(int i=0;i<curMat.Length;++i){
            defaultMat[i]=affectObj[i].GetComponent<MeshRenderer>().material;
            //curMat[i]=defaultMat[i];           
        }
    }
    #endregion

    #region 实时检测是否要开启提示
    void Update(){
        if(mirror==null) return;
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            m_isTwinkling = true;
            SwitchHintState(true);
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            m_isTwinkling = false;
            SwitchHintState(false);
        }
        if(Input.GetKey(KeyCode.Space)) SetLightIntense();

        Twinkle(lightMat);
    }
    #endregion

    #region 在提示状态和非提示状态间进行切换
    public void SwitchHintState(bool b){
        if(b==true){
            for(int i=0;i<curMat.Length;++i)
                affectObj[i].GetComponent<MeshRenderer>().material=lightMat;
                //curMat[i]=lightMat;
        }
        else{
            for(int i=0;i<curMat.Length;++i){
                 affectObj[i].GetComponent<MeshRenderer>().material=defaultMat[i];
            }
        }

    }
    #endregion

    #region 计算发光强度
    float CalLightIntense(){
        float hintPercent=0;
        if(needMirrorRotate){
            float tmp=1-CalAngel(mirrorCurRotation,mirrorCorrectRotation)/mirrorRotateHintUnit;
            hintPercent+=(tmp>0)?tmp:0;
        }
        if(needStairTrans){
            float tmp=1-CalDis(stairCurTranslation,stairCorrectTranslation)/stairTransHintUnit;
            hintPercent+=(tmp>0)?tmp:0;
        }
        if(needMirrorCam){
            float b=CalAngel(camCurRotation,camCorrectRotation);
            if(b!=0)
                hintPercent=0;
        }
        //Debug.Log((1-CalAngel(mirrorCurRotation,mirrorCorrectRotation)/180f)+" " +(1-CalDis(stairCurTranslation,stairCorrectTranslation)/stairTransHintUnit)+" "+hintPercent/calAmount);
        return hintPercent/calAmount;
    }
    #endregion

    #region 设置发光强度
    void SetLightIntense(){
        float hintPercent=CalLightIntense();
        lightMat.SetFloat("_AllPower",hintPercent*4+1);
    }
    #endregion

    #region 找到最近的解法并计算差值
    //计算角度差值
    float CalAngel(Vector3 curV3,Vector3[] corV3){
        if(corV3==null) return 0;
        float ret=999999;
        foreach(Vector3 v in corV3){
            float diff=(v-curV3).magnitude;
            if(diff>180) diff=360-diff;
            ret=Mathf.Min(ret,diff);
        }
        
        return ret;
    }
    //计算距离差值
    float CalDis(Vector3 curV3,Vector3[] corV3){
        if(corV3==null) return 0;
        float ret=99999;
        foreach(Vector3 v in corV3){
            float diff=(v-curV3).magnitude;
            if(diff<0)diff=-diff;
            ret=Mathf.Min(ret,diff);
        }
        return ret;
    }
    #endregion
    */
}
