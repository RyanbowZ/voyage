using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
脚本功能：进入渔村小屋
编写者：孙秋林
初次编写时间：2022/2/6
上次修改时间：2022/2/6
*/

public class OpenDoor : MonoBehaviour
{
    public int easterEggIndex;
    public void OnTrigger(){        
        FadeScene.LoadNxtScene(easterEggIndex);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player")
        OnTrigger();
    }
}
