using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Destination : SimpleButtonBase
{
    [SerializeField]Animator playerAnim;

    private void OnEnable() {
        playerAnim.enabled=true;
        playerAnim.SetTrigger("jump");
    }
}
