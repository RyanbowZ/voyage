using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSecondRotate : MonoBehaviour
{
    [SerializeField] MirrorController mirror;
    private void OnMouseDown() {
        mirror.SetState(MirrorState.UNCORRECT);
    }
}
