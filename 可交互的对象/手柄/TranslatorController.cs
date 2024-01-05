using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TranslateDirection{X,Y,Z}
public class TranslatorController : MonoBehaviour
{
    [Header("关联的对象")]
    [SerializeField] Transform transObj;
    [Header("相关属性（最大最小位移是世界坐标）")]
    [SerializeField] float translateSpeed;//平移属性
    [SerializeField] float maxTrans;
    [SerializeField] float minTrans;
    [SerializeField] float curTrans;
    public TranslateDirection translateDirection;

    Vector3 start_pos,pre_pos,offset;//记录鼠标拖拽时各个位置的变量

    private void Awake() {
        switch(translateDirection){
            case TranslateDirection.X:{
                curTrans=transObj.position.x;
                break;
            }
            case TranslateDirection.Y:{
                curTrans=transObj.position.y;
                break;
            }
            case TranslateDirection.Z:{
                curTrans=transObj.position.z;
                break;
            }
        }
    }

    public void OnMouseDown() {
        pre_pos = Input.mousePosition;
        start_pos = Input.mousePosition;
    }
    private void OnMouseDrag() {
        if(transObj.GetComponent<MirrorController>()!=null)
                transObj.GetComponent<MirrorController>().SetState(MirrorState.OPERATING);
        if(transObj.GetComponent<RoadController>()!=null){
            transObj.GetComponent<RoadController>().SetState(RoadState.OPERATING);
            transObj.GetComponent<RoadController>().SetTranslator(this);
        }

        offset = (pre_pos-Input.mousePosition)*translateSpeed;
        pre_pos = Input.mousePosition;
        switch(translateDirection){
            case TranslateDirection.X:{
                //transform.Translate(new Vector3(offset.x,0,0),Space.World);
                transObj.Translate(new Vector3(offset.x,0,0),Space.World);
                curTrans+=offset.x;
                if(curTrans>maxTrans){
                    transObj.position=new Vector3(maxTrans,transObj.position.y,transObj.position.z);
                    curTrans=maxTrans;
                }
                else if(curTrans<minTrans){
                    transObj.position=new Vector3(minTrans,transObj.position.y,transObj.position.z);
                    curTrans=minTrans;
                }
                break;
            }
            case TranslateDirection.Y:{
                //transform.Translate(new Vector3(0,offset.y,0),Space.World);
                transObj.Translate(new Vector3(0,offset.y,0),Space.World);
                curTrans+=offset.y;
                if(curTrans>maxTrans){
                    transObj.position=new Vector3(transObj.position.x,maxTrans,transObj.position.z);
                    curTrans=maxTrans;
                }
                else if(curTrans<minTrans){
                    transObj.position=new Vector3(transObj.position.x,minTrans,transObj.position.z);
                    curTrans=minTrans;
                }
                break;
            }
            case TranslateDirection.Z:{
                //transform.Translate(new Vector3(0,0,offset.y),Space.World);
                transObj.Translate(new Vector3(0,0,offset.y),Space.World);
                curTrans+=offset.y;
                if(curTrans>maxTrans){
                    transObj.position=new Vector3(transObj.position.x,transObj.position.y,maxTrans);
                    curTrans=maxTrans;
                }
                else if(curTrans<minTrans){
                    transObj.position=new Vector3(transObj.position.x,transObj.position.y,minTrans);
                    curTrans=minTrans;
                }
                break;
            }
        }        
    }

    private void OnMouseUp() {
        if(transObj.GetComponent<MirrorController>()!=null)
                transObj.GetComponent<MirrorController>().SetState(MirrorState.UNCORRECT);
        if(transObj.GetComponent<RoadController>()!=null)
            transObj.GetComponent<RoadController>().SetState(RoadState.UNCORRECT);
    }
}
