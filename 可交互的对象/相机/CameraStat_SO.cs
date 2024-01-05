using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Cam Data",menuName ="New Data/New Obj Data/New Cam Data")]
public class CameraStat_SO : ScriptableObject
{
    public CameraStat_SO nxtCameraStat;
    public bool moreCamera;
    public int solutionAmount;
    public Vector3[] correctRotations;
}
