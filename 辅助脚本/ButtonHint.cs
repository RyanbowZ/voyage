using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHint : MonoBehaviour
{
    public static ButtonHint instance;

    public GameObject[] buttons;
    public Material originMat;
    public Material lightMat;

    [HideInInspector]
    public int currentIndex;

    void Awake()
    {
        if(instance == null) instance = this;
    }
    void Start()
    {
        currentIndex = 0;
    }
    void Update()
    {
//        Debug.Log(currentIndex);
        if(Input.GetKeyDown(KeyCode.Space))
        {
           ChangeMat();
           m_isTwinkling = true;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
           ResetMat();
           m_isTwinkling = false;
        }

        Twinkle(buttons[currentIndex].GetComponent<MeshRenderer>().material);
    }
    void ChangeMat()
    {
        if(currentIndex >= 0 && currentIndex < buttons.Length && buttons[currentIndex] != null)
        {
            buttons[currentIndex].GetComponent<MeshRenderer>().material = lightMat;
        }
    }
    void ResetMat()
    {
        if(currentIndex < buttons.Length && buttons[currentIndex] != null)
        {
            buttons[currentIndex].GetComponent<MeshRenderer>().material = originMat;
        }
    }

    #region 闪烁设置
    [Header("闪烁时间设置")]
    public float onceTwinkleTime;                       //单次闪烁花费的时间/秒
    public float totalTwinkleTime;                      //按下提示后闪烁的总时长
    public float twinkleDensityRnage;                   //亮度变化最大值
    float m_twinkleOnceTimeCounter;                     //单次闪烁计时器
    float m_twinkleTotalTimeCounter;                    //总闪烁计时器
    float m_twinkleDensityCounter;                      //闪烁亮度变化记录
    float m_twinkleDensityStart;                        //初始亮度
    bool m_isTwinkling = false;

    void Twinkle(Material targetMaterial)
    {
        m_twinkleDensityStart = 3.7f; //初始亮度

        if((m_twinkleTotalTimeCounter < totalTwinkleTime) && m_isTwinkling)
        {
            ChangeMat();

            m_twinkleDensityCounter = TwinkleOnceTimeChange(m_twinkleOnceTimeCounter);
            
            lightMat.SetFloat("_AlphaPower",m_twinkleDensityStart + m_twinkleDensityCounter);

            m_twinkleTotalTimeCounter += Time.deltaTime;
            m_twinkleOnceTimeCounter = m_twinkleTotalTimeCounter % onceTwinkleTime;
        }
        else
        {
            ResetMat();

            m_twinkleTotalTimeCounter = 0;
            m_isTwinkling = false;
        }
    }
    float TwinkleOnceTimeChange(float currentTime) //模拟单次闪烁的亮度周期
    {
        currentTime = Mathf.Clamp(currentTime, 0f, onceTwinkleTime);
        return Mathf.Sin(currentTime / onceTwinkleTime * 2f *Mathf.PI) * twinkleDensityRnage;
    }
    #endregion


}
