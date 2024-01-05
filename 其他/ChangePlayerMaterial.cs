using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerMaterial : MonoBehaviour
{
    public bool isTest;
    public static ChangePlayerMaterial instance;
    public static Transform[] childs;
    public static Material[] materials;
    public static float Zvalue=1;
    
    void Awake(){

        if(instance!=null) Destroy(this);
        instance=this;        
    }

    void Start(){
        childs=GetComponentsInChildren<Transform>();
        materials=new Material[1];

        //因为获取子物体时会把父物体本身获取到，所以得是 i+1
        for(int i=0;i<1;++i) {
            //Debug.Log(childs[i+1].gameObject);
            materials[i]=childs[i+1].GetComponent<MeshRenderer>().material;
        }
    }

    public static void ResetZ(){
        foreach(Material material in materials)
            material.SetFloat("_ZTestAddValue",0);
        if(instance.isTest) Debug.Log("重置材质深度");
    }

    public static void SetZ(){
        foreach(Material material in materials)
            material.SetFloat("_ZTestAddValue",Zvalue);
        if(instance.isTest) Debug.Log("设置材质深度");
    }

    //让玩家变透明
    public static void BeginMakeTransparent(){
        instance.StartCoroutine(instance.MakeTransparent());
    }

     IEnumerator MakeTransparent(){
        for(int i=0;i<10;++i){
            foreach(Material material in materials)
                material.SetFloat("_AlphaScale",material.GetFloat("_AlphaScale")-0.1f);
            Debug.Log("变透明");
            yield return new WaitForSeconds(0.1f);
        }
    }
    public static void BeginMakeUntransparent(){
        instance.StartCoroutine(instance.MakeUntransparent());
    }

     IEnumerator MakeUntransparent(){
        for(int i=0;i<10;++i){
            foreach(Material material in materials)
                material.SetFloat("_AlphaScale",material.GetFloat("_AlphaScale")+0.1f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
