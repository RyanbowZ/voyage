using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScene : MonoBehaviour {
    public static FadeScene instance;
    public Texture blackTexture;
    public float alpha = 1.0f;
    public float fadespeed = 0.2f;
    public int fadeDir = -1;

    void Awake(){
        if(instance!=null) Destroy(this);
        instance=this;
    }

    void OnGUI()
    {
        if(alpha>=0||(alpha<=0&&fadeDir>0)){
            alpha += fadeDir * fadespeed * Time.deltaTime;
            GUI.color = new Color (GUI.color .r ,GUI.color .g ,GUI.color .b,alpha);
            GUI.DrawTexture (new Rect (0,0,Screen .width ,Screen .height), blackTexture);

        }
    }
    //计算渐入渐出时间
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        Debug.Log("开始渐入渐出");
        return 1 / fadespeed;
    }

    void OnLevelWasLoaded()
    {
        Debug.Log ("场景加载完毕！");
        BeginFade (-1);  
    }

    public static void LoadNxtScene(int index){
        PlayerPrefs.SetInt("PreScene",SceneManager.GetActiveScene().buildIndex);
        AudioManager.bgmGoAway();
        instance.StartCoroutine(FadeLoadScene(index));
    }
    
    public static IEnumerator FadeLoadScene(int index)
    {
        float time = instance.BeginFade (1);
        //AsyncOperation operation=
        //operation.allowSceneActivation=false;
        yield return new WaitForSeconds (time/2);
        SceneManager.LoadSceneAsync (index);
       // operation.allowSceneActivation=true;
    }

}