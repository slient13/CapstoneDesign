using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandPanel : MonoBehaviour
{
    public GameObject initPanel;
    public GameObject attackPanel;

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

    public void SetInitPanel(bool input)
    {
        initPanel.gameObject.SetActive(input);
    }

    public void SetAttackPanel(bool input)
    {
        attackPanel.gameObject.SetActive(input);
    }
}
