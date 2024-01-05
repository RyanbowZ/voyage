using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{//
    public static AudioManager instance;
    public bool needGoAway=false;//音乐需要渐出
    [SerializeField] AudioSource MoperateHandle;//对手柄进行操作的音效
    //[SerializeField]float FPCoolDown=1f;//拼合路径音效固定冷却时间
    //float curFPCoolDown;//拼合路径音效当前冷却时间
    [SerializeField] AudioSource MflattenPath;
    [SerializeField]float FPCoolDown=1f;//拼合路径音效固定冷却时间
    float curFPCoolDown;//拼合路径音效当前冷却时间
    [SerializeField] AudioSource MpressButton;
    [SerializeField]float PBCoolDown=1f;
    float curPBCoolDown;
    [SerializeField] AudioSource MappearSth;
    [SerializeField]float ASCoolDown=1f;
    float curASCoolDown;
    [SerializeField] AudioSource MsendPlayer;//传送玩家的音效
    [SerializeField]float SPCoolDown=1f;
    float curSPCoolDown;
    [SerializeField] AudioSource MpassLevel;//通过音效
    [SerializeField]float PLCoolDown=1f;
    float curPLCoolDown;
    [SerializeField] AudioSource bgm;

    private void Awake() {
        if(instance!=null)Destroy(this);
        instance=this;
    }

    private void Start() {
    }

    void FixedUpdate(){
        if(needGoAway){
            instance.bgm.volume = Mathf.Lerp(instance.bgm.volume, 0.0f, 0.05f);
        }

    }
    void Update(){
        if(curFPCoolDown>0) curFPCoolDown-=Time.deltaTime;
        if(PBCoolDown>0) curPBCoolDown-=Time.deltaTime;
        if(ASCoolDown>0) curASCoolDown-=Time.deltaTime;
        if(SPCoolDown>0) curSPCoolDown-=Time.deltaTime;
        if(PLCoolDown>0) PLCoolDown-=Time.deltaTime;

    }

    //音乐的淡出
    public static void bgmGoAway(){
        instance.needGoAway=true;
    }
    public static void PlayOperateHandleMusic(){
        instance.MoperateHandle.Play();
    }
    public static void PauseOperateHandleMusic(){
        instance.MoperateHandle.Pause();
    }
    public static void PlayPassLevelMusic(){
        if(instance.curPLCoolDown<=0){
            instance.MpassLevel.Play();
            instance.curPLCoolDown=instance.PLCoolDown;
        }
    }

    public static void PlayFlattenPathMusic(){
        if(instance.curFPCoolDown<=0){
            instance.MflattenPath.Play();
            instance.curFPCoolDown=instance.FPCoolDown;
        }
    }
    public static void PlayPressButtonMusic(){
        if(instance.curPBCoolDown<=0){
            instance.MpressButton.Play();
            instance.curPBCoolDown=instance.PBCoolDown;
        }
    }
    public static void PlayAppearSthMusic(){
        if(instance.curASCoolDown<=0){
            instance.curASCoolDown=instance.ASCoolDown;
            Debug.Log("出现新东西!"+instance.curASCoolDown+" "+instance.ASCoolDown);
            instance.MappearSth.Play();
        }
    }
    public static void PlaySendPlayerMusic(){
        if(instance.curSPCoolDown<=0){
            instance.MsendPlayer.Play();
            instance.curSPCoolDown=instance.SPCoolDown;
        }
    }
}
