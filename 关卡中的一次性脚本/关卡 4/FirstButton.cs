using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstButton : SimpleButtonBase
{
    [SerializeField] MeshRenderer plateOne;
    //[SerializeField] GameObject mirror,buttonThree;
    //[SerializeField] Animator roadAnim;

    public override void TriggerSth()
    {
        //roadAnim.enabled=true;
        plateOne.enabled=true;
        //buttonThree.SetActive(true);
        //mirror.SetActive(true);
        base.TriggerSth();
    }

}
