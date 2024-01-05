using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{

    int i=0;

    void Start(){
        StartCoroutine(lll());
    }

    private void Update() {
        i++;
        Debug.Log("第一次"+i);
        i++;
        Debug.Log("第二次"+i);
    }//
    IEnumerator lll(){
        while(true){
            ++i;
            Debug.Log("协程"+i);
            yield return new WaitForEndOfFrame();
        }
    }
}
