using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HintNext : MonoBehaviour
{
    public int nextButtonIndex = -1;

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if(SceneManager.GetActiveScene().buildIndex == 10)
        {
            Debug.Log("SC");
            ButtonHint.instance.currentIndex = nextButtonIndex;
        }
    }
}
