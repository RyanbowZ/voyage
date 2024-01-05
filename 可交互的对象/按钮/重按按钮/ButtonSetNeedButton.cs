using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//按下后，镜子的needbutton可以得到满足
public class ButtonSetNeedButton : RepressedButtonBase
{
    [SerializeField] MirrorController mirror;
    //[SerializeField] Transform par;
    [SerializeField] Transform player;
    public ChangePlayerMaterial changePlayer;//镜中的虚像玩家
    public bool isSend=false;//避免重复触发传送
    public override void TriggerSth()
    {
        
        //if(!isSend&&player!=null&&mirror.canSend==true) {
            //isSend=true;
            //StartCoroutine(MeltInMirror());
        //}
        //else mirror.SetNeedButton(true);
        mirror.SetNeedButton(true);
        base.TriggerSth();
    }

    private void OnTriggerStay(Collider other) {
        /*if(!isSend&&player!=null&&mirror.canSend==true) {
            isSend=true;
            StartCoroutine(MeltInMirror());
        }*/
    }

    //玩家进入镜子
    IEnumerator MeltInMirror(){
        //changePlayer.BeginMakeTransparent();
        yield return new WaitForSeconds(1f);
        if(GetComponent<MeshRenderer>().enabled==true)
            AudioManager.PlaySendPlayerMusic();
        //Transform[] trans=player.GetComponentsInChildren<Transform>();
        //foreach(Transform tran in trans) tran.gameObject.layer=9;
        //changePlayer.BeginMakeUntransparent();
        //mirror.SendPlayerIn();
        //yield return new WaitForSeconds(0.5f);
        //fakePlayer.SetActive(false);
        //mirror.SetNeedButton(true);
    }

    public override void ResetSth()
    {
        Transform[] trans=player.GetComponentsInChildren<Transform>();
        foreach(Transform tran in trans) tran.gameObject.layer=10;
        mirror.SetNeedButton(false);
        base.ResetSth();
    }
}
