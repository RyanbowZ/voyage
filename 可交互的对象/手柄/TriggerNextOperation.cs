using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//点击该对象时，镜子会重新出现以进行下一次操作
public class TriggerNextOperation : MonoBehaviour
{
    [SerializeField] MirrorController[] mirrors;
    private void OnMouseDown() {
        foreach(var mirror in mirrors)
            mirror.SetState(MirrorState.UNCORRECT);
    }
}
