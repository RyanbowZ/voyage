using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorHint : MonoBehaviour
{
    [Header("MirrorComponent")]
    // public GameObject mirror;
    public Transform m_mirrorTransform;
    public MeshRenderer m_mirrorMeshRenderer;
    public MirrorController m_mirrorController;
    Material m_mirrorMaterial;
    [Header("HintSeeting")]
    [SerializeField] float maxRotateHintDistance = 30f;
    [SerializeField] float maxTranslateHintDistance = 0.5f;
    void Awake()
    {
        m_mirrorMaterial = m_mirrorMeshRenderer.material;
    }
    void Update()
    {
        if((m_mirrorTransform != null) && (m_mirrorMeshRenderer != null) && (m_mirrorController != null))
        {
            if(m_mirrorController.correctRotation.Length != 0 && m_mirrorController.correctTranslation.Length != 0)
            {
                // 计算与正确旋转角度的最小绝对差值
                float rotateDistanceToCorrect = Quaternion.Angle(m_mirrorTransform.rotation, Quaternion.Euler(m_mirrorController.correctRotation[0]));
                foreach(var m in m_mirrorController.correctRotation)
                {
                    rotateDistanceToCorrect = Quaternion.Angle(m_mirrorTransform.rotation, Quaternion.Euler(m)) < rotateDistanceToCorrect
                                            ? Quaternion.Angle(m_mirrorTransform.rotation, Quaternion.Euler(m))
                                            : rotateDistanceToCorrect;
                }
                // 计算与正确移动距离的绝对差值
                float translateDistanceToCorrect = Vector3.Distance(m_mirrorTransform.position, m_mirrorController.correctTranslation[0]);
                foreach(var m in m_mirrorController.correctTranslation)
                {
                    translateDistanceToCorrect = Vector3.Distance(m_mirrorTransform.position, m) < translateDistanceToCorrect
                                               ? Vector3.Distance(m_mirrorTransform.position, m)
                                               : translateDistanceToCorrect;
                }

                if(maxRotateHintDistance > rotateDistanceToCorrect && maxTranslateHintDistance > translateDistanceToCorrect)
                {
                    float hintPercent = 0.5f * (maxTranslateHintDistance - Mathf.Lerp(translateDistanceToCorrect, maxTranslateHintDistance, 0.05f)) / maxTranslateHintDistance
                                      + 0.5f * (maxRotateHintDistance - Mathf.Lerp(rotateDistanceToCorrect, maxRotateHintDistance, 0.05f)) / maxRotateHintDistance;
                    m_mirrorMaterial.SetFloat("_RimLength", (0.5f - 0.1f * hintPercent));
                }
                else
                m_mirrorMaterial.SetFloat("_RimLength", 0.5f);
            }
            if(m_mirrorController.correctRotation.Length == 0 && m_mirrorController.correctTranslation.Length != 0)
            {
                // 计算与正确移动距离的绝对差值
                float translateDistanceToCorrect = Vector3.Distance(m_mirrorTransform.position, m_mirrorController.correctTranslation[0]);
                foreach(var m in m_mirrorController.correctTranslation)
                {
                    translateDistanceToCorrect = Vector3.Distance(m_mirrorTransform.position, m) < translateDistanceToCorrect
                                               ? Vector3.Distance(m_mirrorTransform.position, m)
                                               : translateDistanceToCorrect;
                }
                // 若在产生提示的最大范围内则按接近的百分比程度提高镜子边框亮度
                if(maxTranslateHintDistance > translateDistanceToCorrect)
                {
                    float hintPercent = (maxTranslateHintDistance - Mathf.Lerp(translateDistanceToCorrect, maxTranslateHintDistance, 0.05f)) / maxTranslateHintDistance;
                    m_mirrorMaterial.SetFloat("_RimLength", (0.5f - 0.1f * hintPercent));
                }
                else
                m_mirrorMaterial.SetFloat("_RimLength", 0.5f);
            }
            if(m_mirrorController.correctRotation.Length != 0 && m_mirrorController.correctTranslation.Length == 0)
            {
                // 计算与正确旋转角度的最小绝对差值
                float rotateDistanceToCorrect = Quaternion.Angle(m_mirrorTransform.rotation, Quaternion.Euler(m_mirrorController.correctRotation[0]));
                foreach(var m in m_mirrorController.correctRotation)
                {
                    rotateDistanceToCorrect = Quaternion.Angle(m_mirrorTransform.rotation, Quaternion.Euler(m)) < rotateDistanceToCorrect
                                            ? Quaternion.Angle(m_mirrorTransform.rotation, Quaternion.Euler(m))
                                            : rotateDistanceToCorrect;
                }
                // 若在产生提示的最大范围内则按接近的百分比程度提高镜子边框亮度
                if(maxRotateHintDistance > rotateDistanceToCorrect)
                {
                    float hintPercent = (maxRotateHintDistance - Mathf.Lerp(rotateDistanceToCorrect, maxRotateHintDistance, 0.05f)) / maxRotateHintDistance;
                    m_mirrorMaterial.SetFloat("_RimLength", (0.5f - 0.1f * hintPercent));
                }
                else
                m_mirrorMaterial.SetFloat("_RimLength", 0.5f);
            }
        }
    }
}
