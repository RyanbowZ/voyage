using UnityEngine;

public enum RotateDirection{HORIZONTAL,VERTICAL,RIGHT}
public class RotatorController : MonoBehaviour
{
    [Header("关联的游戏物体")]
    [SerializeField] GameObject rotatableObj;
    [Header("旋转方向")]
    public RotateDirection rotateAxix;//旋转轴
    [Header("相应参数")]
    public bool canRotate = true;//是否可以进行旋转操作
    public float rotateSpeed=1f;
    Vector3 startPos, prePos, curPos, axisPos;

    bool clockWise = true;
    bool cBegin = false;
    float fadeSpeed = 180f;
    float continueTime = 1f;
    float continueTimeCount = 0f;
    float rotateVelocity = 0.0f;
    bool cTrigger = false;
    float preRotation = 0.0f;

     private void Start() {
        startPos= Camera.main.WorldToScreenPoint(this.transform.position);
     }
    void Update()
    {
        /*
        curPos = Input.mousePosition;

        if(cBegin)
        {
            if(rotatableObj.GetComponent<MirrorController>()!=null)
                rotatableObj.GetComponent<MirrorController>().SetState(MirrorState.OPERATING);
            if(rotatableObj.GetComponent<RoadController>()!=null)
                rotatableObj.GetComponent<RoadController>().SetState(RoadState.OPERATING);
            if(continueTimeCount < continueTime && (rotateVelocity > 0.7f || cTrigger))
            {
                cTrigger = true;
                continueTimeCount += Time.deltaTime;

                float angleTo = fadeSpeed * Mathf.Sin((Mathf.PI / 2) + (Mathf.PI / 2) * continueTimeCount) * Time.deltaTime;
                if(!clockWise) angleTo*=-1;
                switch(rotateAxix)
                {
                    case RotateDirection.VERTICAL:
                        rotatableObj.transform.Rotate(new Vector3(0, 0, angleTo), Space.World);
                        this.transform.Rotate(new Vector3(0, 0, angleTo), Space.World);
                        break;
                    case RotateDirection.HORIZONTAL:
                        rotatableObj.transform.Rotate(new Vector3(0, angleTo, 0), Space.World);
                        this.transform.Rotate(new Vector3(0, angleTo, 0), Space.World);
                        break;
                    case RotateDirection.RIGHT:
                        rotatableObj.transform.Rotate(new Vector3(-angleTo ,0 , 0), Space.World);
                        this.transform.Rotate(new Vector3(-angleTo ,0 , 0), Space.World);
                        break;
                }
            }
            else
            {
                if(rotatableObj.GetComponent<MirrorController>()!=null)
                    rotatableObj.GetComponent<MirrorController>().SetState(MirrorState.UNCORRECT);
                if(rotatableObj.GetComponent<RoadController>()!=null)
                    rotatableObj.GetComponent<RoadController>().SetState(RoadState.UNCORRECT);

                continueTimeCount = 0;
                cBegin = false;
                cTrigger = false;
            }
        }

        switch(rotateAxix)
        {
            case RotateDirection.VERTICAL:
                rotateVelocity = (this.transform.localRotation.z - preRotation) / Time.deltaTime;
                preRotation = this.transform.localRotation.z;
                break;
            case RotateDirection.HORIZONTAL:
                rotateVelocity = (this.transform.localRotation.y - preRotation) / Time.deltaTime;
                preRotation = this.transform.localRotation.y;
                break;
            case RotateDirection.RIGHT:
                rotateVelocity = (this.transform.localRotation.x - preRotation) / Time.deltaTime;
                preRotation = this.transform.localRotation.x;
                break;
        }
        */
    }

    #region 旋转操作
    public void OnMouseDown()
    {
        AudioManager.PlayOperateHandleMusic();
        //startPos = Input.mousePosition;
        prePos = Input.mousePosition;
        curPos = Input.mousePosition;
    }
    public void OnMouseDrag()
    {

        if(canRotate)
        {
            
            if(rotatableObj.GetComponent<MirrorController>()!=null)
                rotatableObj.GetComponent<MirrorController>().SetState(MirrorState.OPERATING);
            if(rotatableObj.GetComponent<RoadController>()!=null)
                rotatableObj.GetComponent<RoadController>().SetState(RoadState.OPERATING);
            /*

        curPos = Input.mousePosition;
        // Debug.Log("startPos : " + Camera.main.WorldToScreenPoint(this.transform.position));

        bool ver = (RotateDirection.VERTICAL == rotateAxix);
        // 根据鼠标位置变化判断旋转操作的顺时针或逆时针
        float angleTo = 0;
        if((curPos.x > prePos.x && curPos.y > startPos.y && prePos.y > startPos.y) || (curPos.x < prePos.x && curPos.y < startPos.y && prePos.y < startPos.y) && !ver)
        {
            // angleTo = Vector2.Angle(preLine, curLine);
            angleTo = Vector2.Distance(curPos, prePos);
            clockWise = true;
        }
        else if((curPos.x > prePos.x && curPos.y < startPos.y && prePos.y < startPos.y) || (curPos.x < prePos.x && curPos.y > startPos.y && prePos.y > startPos.y) && !ver)
        {
            // angleTo = -Vector2.Angle(preLine, curLine);
            angleTo = -Vector2.Distance(curPos, prePos);
            clockWise = false;
        }
        else if(ver)
        {
            angleTo = prePos.y - curPos.y;
        }
        //Debug.Log("angleTo: " + angleTo);

        angleTo*=rotateSpeed;

        switch(rotateAxix)
        {
            case RotateDirection.VERTICAL:
                rotatableObj.transform.Rotate(new Vector3(0, 0, angleTo), Space.World);
                this.transform.Rotate(new Vector3(0, 0, angleTo), Space.World);
                break;
            case RotateDirection.HORIZONTAL:
                rotatableObj.transform.Rotate(new Vector3(0, angleTo, 0), Space.World);
                this.transform.Rotate(new Vector3(0, angleTo, 0), Space.World);
                break;
            case RotateDirection.RIGHT:
                rotatableObj.transform.Rotate(new Vector3(-angleTo ,0 , 0), Space.World);
                this.transform.Rotate(new Vector3(-angleTo ,0 , 0), Space.World);
                break;
        }
                    prePos = Input.mousePosition;
                    */
            Vector3 offset = (Input.mousePosition - prePos);// rotateSpeed;
            //Vector3 offset = (Vector2.Angle(axisPos, Input.mousePosition) - Vector2.Angle(axisPos, prePos)) * 2;
            //Debug.Log(offset);
            //Debug.Log("当前轴心点：" + Camera.main.WorldToScreenPoint(this.transform.position));
            axisPos = Camera.main.WorldToScreenPoint(this.transform.position);
            //Debug.Log("前者角度："+Vector2.Angle(axisPos,prePos));
            var qz = Mathf.Atan2((axisPos.y - prePos.y) ,(axisPos.x - prePos.x))*57.3f+180f;
            var hz = Mathf.Atan2((axisPos.y - Input.mousePosition.y) , (axisPos.x - Input.mousePosition.x)) * 57.3f + 180f;
            Debug.Log("前者角度："+ qz);
            //Debug.Log("后者角度：" + Vector2.Angle(axisPos, Input.mousePosition));
            Debug.Log("后者角度：" + hz);
            var deltaz = qz - hz;
            Debug.Log("角度之差：" + deltaz);
            //var ld = prePos-axisPos; Vector3 ldn;
            //Debug.Log("cos"+Mathf.Cos(45));
            //ld = new Vector3(Mathf.Cos(rotatableObj.transform.eulerAngles.y), Mathf.Sin(rotatableObj.transform.eulerAngles.y), 0);
            
            //ldn = new Vector3(-ld.y, ld.x, 0).normalized;
            
           
            
            //var offsetmp=Vector3.Dot(Input.mousePosition - prePos, ldn) * ldn* rotateSpeed;
            //offsetmp = new Vector3(-offsetmp.y, offsetmp.x);
            //Debug.Log("offsetmp:"+offsetmp*100 +"ldn:"+ld+ " offsetmp.magnitude:"+ offsetmp.magnitude*100);
            //Debug.Log("offset:" + offset * 100);
            if (deltaz > 300) deltaz -= 360;
            else if (deltaz < -300) deltaz += 360;
            var rotateAngle = deltaz;
            //var tmp = (Vector2.Angle(axisPos, Input.mousePosition) - Vector2.Angle(axisPos, prePos)) * 2f;
            //Debug.Log("tmp=" + tmp);
            prePos = Input.mousePosition;
            //Debug.Log("当前鼠标点击：" + prePos);
            Vector3 dis = new Vector3(0f, 0f, 0f);
            switch (rotateAxix){
                case RotateDirection.VERTICAL:
                    // dis = new Vector3(0, offset.y, 0);
                    rotatableObj.transform.Rotate(Vector3.forward*offset.y*-1, offset.magnitude* rotateSpeed, Space.World);
                    this.transform.Rotate(Vector3.forward * offset.y * -1, offset.magnitude* rotateSpeed, Space.World);
                    Debug.Log("RotateDirection.VERTICAL");
                    //rotatableObj.transform.Rotate(Vector3.forward, rotateAngle, Space.World);
                    //this.transform.Rotate(rotatableObj.transform.forward, rotateAngle, Space.World);
                    break;
                case RotateDirection.HORIZONTAL:{
                        // dis = new Vector3(offset.x, 0, 0);
                        //rotatableObj.transform.Rotate(Vector3.up*offset.x*-1  , offset.magnitude, Space.World);
                    Debug.Log("RotateDirection.HORIZONTAL");
                    rotatableObj.transform.Rotate(Vector3.up  , rotateAngle* rotateSpeed, Space.World);
                    this.transform.Rotate(this.transform.up, rotateAngle* rotateSpeed, Space.World);
                    break;
                }
                case RotateDirection.RIGHT:{
                        //dis = new Vector3(offset.x, 0, 0);
                        //rotatableObjFollowing.Rotate(Vector3.Cross(dis, Vector3.forward), offset2.magnitude, Space.World);
                        Debug.Log("RotateDirection.RIGHT");
                        rotatableObj.transform.Rotate(Vector3.right * -1, rotateAngle* rotateSpeed, Space.World);
                        this.transform.Rotate(this.transform.up , rotateAngle* rotateSpeed, Space.World);
                        //rotatableObj.transform.Rotate(Vector3.right*offset.x*-1  , offset.magnitude, Space.World);
                        //this.transform.Rotate(this.transform.up*offset.x*-1, offset.magnitude, Space.World);
                        break;
                }
            }
            /*

            switch(rotateAxix){
                case rotateAxix.RIGHT:{
                    this.transform.Rotate(Vector3.up*offset.x*-1, offset.magnitude, Space.World);
                    break;
                }
                case rotateAxix.FORWARD:{
                    this.transform.Rotate(Vector3.forward*offset.y*-1, offset.magnitude, Space.World);
                    break;
                }
                case rotateAxix.UP:{
                    this.transform.Rotate(Vector3.up*offset.x*-1, offset.magnitude, Space.World);
                    break;
                }
            }*/
        }
    }
    public void OnMouseUp()
    {
        AudioManager.PauseOperateHandleMusic();
        if(rotatableObj.GetComponent<MirrorController>()!=null)
                rotatableObj.GetComponent<MirrorController>().SetState(MirrorState.UNCORRECT);
        if(rotatableObj.GetComponent<RoadController>()!=null)
            rotatableObj.GetComponent<RoadController>().SetState(RoadState.UNCORRECT);

        cBegin = true;
        Debug.Log("up");
    } 
    #endregion

    #region 与外界通信
    public void SetCanRotate(bool b){
        canRotate=b;
    }
    #endregion
}