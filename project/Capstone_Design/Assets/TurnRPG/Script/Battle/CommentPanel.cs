using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommentPanel : MonoBehaviour
{
    public GameObject textPanel;
    Text mainText;

    // Start is called before the first frame update
    void Start()
    {
        textPanel = transform.Find("MainText").gameObject;
    }

    /// <summary>
    /// 텍스트를 가져오기
    /// </summary>
    /// <param name="text"></param>
    void GetText(string text)
    {
        mainText.text = text;
    }

    /// <summary>
    /// 텍스트 업데이트
    /// </summary>
    void UpdateText()
    {
        Text originalText = textPanel.GetComponent<Text>();

        originalText.text = mainText.text;
        originalText.color = mainText.color;
        originalText.fontSize = mainText.fontSize;
    }
}
