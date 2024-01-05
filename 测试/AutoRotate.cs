using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Vector3 target=new Vector3(0.2f,0.2f,0.2f);
    private void Update() {
        transform.rotation=Quaternion.RotateTowards(this.transform.localRotation,Quaternion.Euler(target),100);
    }
}
