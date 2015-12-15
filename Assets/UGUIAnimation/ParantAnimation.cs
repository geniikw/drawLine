using UnityEngine;
using System.Collections;
/// <summary>
/// 애니메이션 대통합 ㅋㅋ;
/// </summary>
public abstract class ParentAnimation : MonoBehaviour {
    public float waitTime = 0f;
    public ParentAnimation next= null;
    public float time = 1f;

    public bool isPlayAtStart = false;


    void Start()
    {
        if (isPlayAtStart)
        {
            StartCoroutine(Play());
        }
    }

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
    protected abstract IEnumerator PlayAnimation();

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
