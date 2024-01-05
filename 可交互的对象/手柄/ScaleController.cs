using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScaleDirection{X,Y,Z}
public class ScaleController : MonoBehaviour
{
    [SerializeField] Transform scaleObj;
    [Header("缩放属性")]
    [SerializeField] float scaleSpeed;
    [SerializeField] float maxScale;
    [SerializeField] float minScale;
    [SerializeField] float curScale;
    public ScaleDirection scaleDirection;

    Vector3 start_pos,pre_pos,offset;//记录鼠标拖拽时各个位置的变量

    private void Start() {
        switch(scaleDirection){
            case ScaleDirection.X:{
                curScale=scaleObj.localScale.x;
                break;
            }
            case ScaleDirection.Y:{
                curScale=scaleObj.localScale.y;
                break;
            }
            case ScaleDirection.Z:{
                curScale=scaleObj.localScale.z;
                break;
            }
        }
    }
    public void OnMouseDown() {
        pre_pos = Input.mousePosition;
        start_pos = Input.mousePosition;
    }
    private void OnMouseDrag() {
        if(scaleObj.GetComponent<MirrorController>()!=null)
                scaleObj.GetComponent<MirrorController>().SetState(MirrorState.OPERATING);
        if(scaleObj.GetComponent<RoadController>()!=null)
                scaleObj.GetComponent<RoadController>().SetState(RoadState.OPERATING);
        
        offset = (pre_pos-Input.mousePosition)*scaleSpeed;
        pre_pos = Input.mousePosition;
        switch(scaleDirection){
            case ScaleDirection.X:{
                offset=new Vector3(offset.x,0f,0f);
                scaleObj.localScale+=offset;
                curScale+=offset.x;
                if(curScale>maxScale){
                    scaleObj.localScale=new Vector3(maxScale,scaleObj.localScale.y,scaleObj.localScale.z);
                    curScale=maxScale;
                }
                else if(curScale<minScale){
                    scaleObj.localScale=new Vector3(minScale,scaleObj.localScale.y,scaleObj.localScale.z);
                    curScale=minScale;
                }
                break;
            }
            case ScaleDirection.Y:{
                offset=new Vector3(0f,offset.y,0f);
                scaleObj.localScale+=offset;
                curScale+=offset.y;
                if(curScale>maxScale){
                    scaleObj.localScale=new Vector3(scaleObj.localScale.x,maxScale,scaleObj.localScale.z);
                    curScale=maxScale;
                }
                else if(curScale<minScale){
                    scaleObj.localScale=new Vector3(scaleObj.localScale.x,minScale,scaleObj.localScale.z);
                    curScale=minScale;
                }
                break;
            }
            case ScaleDirection.Z:{
                offset=new Vector3(0f,0f,offset.z);
                scaleObj.localScale+=offset;
                curScale+=offset.z;
                if(curScale>maxScale){
                    scaleObj.localScale=new Vector3(scaleObj.localScale.x,scaleObj.localScale.y,maxScale);
                    curScale=maxScale;
                }
                else if(curScale<minScale){
                    scaleObj.localScale=new Vector3(scaleObj.localScale.x,scaleObj.localScale.y,minScale);
                    curScale=minScale;
                }
                break;
            }
        }

    }
    private void OnMouseUp() {
        if(scaleObj.GetComponent<MirrorController>()!=null)
                scaleObj.GetComponent<MirrorController>().SetState(MirrorState.UNCORRECT);
        if(scaleObj.GetComponent<RoadController>()!=null)
            scaleObj.GetComponent<RoadController>().SetState(RoadState.UNCORRECT);
    }
}
