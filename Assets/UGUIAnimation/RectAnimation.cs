using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// RectTransform 를 조정하는 Animation
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class RectAnimation : ParentAnimation
{
    public RectTransform rectTransform { get { return GetComponent<RectTransform>(); } }
  
    public Vector2 vectorPosition = Vector2.zero;
    
    [Header("일단 0~180 사이를 넣도록 하자")][Tooltip("clamp(0~180) 되는 문제와 360가 넘어갔을때를 구현하지 못함")]
    public Vector3 eulerAngle = Vector3.zero;//이건... 음...; //한바퀴 이상 돌때의 표현을 어떻게 하느냐가 관건인듯
    public Vector2 targetScale = Vector2.one;

    /// <summary>
    /// 만약에 똑같은 루틴이 같은 rectTransform에 대하여 작동한다면 오작동을함...(사실 그럴일이 없긴한데...)
    /// 고치려면 고칠수 있긴한데.... anchoredPosition에 보간식을 직접 대입하는 것 말고
    /// Delta.Time동안 이동하는 거리를 그래프에서 찾아 Translate로 이동시키면 됨.
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator PlayAnimation()
    {
        float t = 0f;

        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 endPosition = startPosition + vectorPosition;
        Quaternion startQuaternion = rectTransform.localRotation;
        Quaternion endQuaternion = startQuaternion * Quaternion.Euler(eulerAngle);

        Vector2 startScale = rectTransform.localScale;
        while (t < 1f)
        {
            float curveTime = curve.Evaluate(t);

            rectTransform.anchoredPosition = startPosition * (1f - curveTime) + endPosition * curveTime;
            rectTransform.localRotation = Quaternion.Lerp(startQuaternion, endQuaternion, curve.Evaluate(t));
            rectTransform.localScale = startScale * (1f - curveTime) + targetScale * curveTime;

            t += Time.deltaTime / time;
            yield return null;
        }

    }
}

