using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextAnimation : MonoBehaviour {

    Text m_text;
    Text text { get { return m_text ?? (m_text = GetComponent<Text>()); } }

    float typingSpeed = 0.01f;

      
    public void Play()
    {
        StartCoroutine_Auto(PlayCoroutine());
    }

    IEnumerator PlayCoroutine()
    {
        string end = text.text;
        text.text = "";

        while(text.text == end)
        {
            int n = 0;
            text.text += end[n];
            yield return null;
        }


    }
    
}
