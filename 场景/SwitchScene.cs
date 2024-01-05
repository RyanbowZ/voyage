using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
脚本功能：进入下一关
编写者：孙秋林
初次编写时间：2022/2/6
上次修改时间：2022/2/6
*/
public class SwitchScene : MonoBehaviour
{
    public int index=-1;
    //
    public void Ontrigger(){
       // Debug.Log(SceneManager.GetActiveScene().buildIndex);
        AudioManager.PlayPassLevelMusic();
        if(index==-1) FadeScene.LoadNxtScene(SceneManager.GetActiveScene().buildIndex+1);
        else    FadeScene.LoadNxtScene(index);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Player")
            Ontrigger();
    }
}
