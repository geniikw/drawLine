using UnityEngine;
using System.Collections;
/// <summary>
/// 애니메이션 대통합 ㅋㅋ;
/// </summary>
public abstract class ParentAnimation : MonoBehaviour {
    public float waitTime = 0f;
    public ParentAnimation next= null;
    public float time = 1f;
    public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
    public bool isPlayAtStart = false;

    void Start()
    {
        Init();
        if (isPlayAtStart)
        {
            StartCoroutine(Play());
        }
    }

    public virtual void Init() { }
 
    public IEnumerator Play()
    {
        yield return StartCoroutine(Wait());
        yield return StartCoroutine(PlayAnimation());
        if(next != null)
        {
            yield return StartCoroutine(Next());
        }
    }

    IEnumerator Next()
    {
        yield return StartCoroutine(next.Play());
    }

    protected virtual void Setup() { }
    protected virtual void Lerp(float t) { }

    protected virtual IEnumerator PlayAnimation()
    {
        float t = 0f;
        Setup();
        while (t < 1f)
        {
            t += Time.deltaTime/time;
            float curveTime = curve.Evaluate(t);
            Lerp(curveTime);
            yield return null;
        }
    }
    IEnumerator Wait()
    {
        float t = 0f;
        while(t < 1f)
        {
            t += Time.deltaTime / waitTime;
            yield return null;
        }
    }
    
}
