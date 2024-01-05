using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerExtraOperation : MonoBehaviour
{
    Animator playerAnim;

    private void Awake() {
        playerAnim=GetComponent<Animator>();
    }

    void CloseAnim(){
        playerAnim.enabled=false;
    }    
}
