using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class TextAnimation : ParentAnimation {

    Text m_text;
    Text text { get { return m_text ?? (m_text = GetComponent<Text>()); } }
    string buffer = "";

    bool sideEffect = false;

    public override void Init()
    {
        buffer = text.text;
        text.text = "";
    }

    protected override IEnumerator PlayAnimation()
    {
        int n = 0;
        float t = 0f;
        while (text.text != buffer)
        {
            t += Time.deltaTime;
            if(t > 1f* n * time)
            {
                text.text += buffer[n++];
            }
            yield return null;
        }
    }
}
