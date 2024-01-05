using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SixthButton : SimpleButtonBase
{
    [SerializeField] RoadDownAndUp rdu;
    [SerializeField] GameObject destination;
    public RotateCamera cam;

    public override void TriggerSth()
    {
        cam.rotateTarget=Quaternion.Euler(0,90,0);
        cam.rotateIndex=2;
        destination.SetActive(true);
        //rdu.TriggerSth();
        Destroy(this);
    }
}
