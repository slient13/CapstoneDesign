using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandPanel : MonoBehaviour
{
    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 활성화 설정
    /// </summary>
    /// <param name="input"></param>
    public void SetActive(bool input)
    {
        this.gameObject.SetActive(input);
    }
}
