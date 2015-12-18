using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextAnimation : MonoBehaviour {

    Text m_text;
    Text text { get { return m_text ?? (m_text = GetComponent<Text>()); } }

    



}
