﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitPanel : MonoBehaviour
{
    public string selectorText = "> ";
    public Text[] command;
    public GameObject gameManager;

    int textCount = 0;
    public int selectorIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("BattleManager");
        GetChild();

        //최초 설렉터 생성
        InitSelector();
    }

    private void Update()
    {
        Selector();
    }

    void GetChild()
    {
        //차일드 숫자 파악
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Text"))
                textCount++;
        }

        //차일드 대입
        command = new Text[textCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name.Contains("Text"))
                command[i] = transform.GetChild(i).GetComponent<Text>();
        }
    }

    void Selector()
    {
        if(Input.GetButtonDown("Vertical"))
        {
            float axis = Input.GetAxisRaw("Vertical") * -1;

            //인덱스 값 변경
            if (axis > 0 && selectorIndex < command.Length - 1)
                selectorIndex++;
            else if (axis < 0 && selectorIndex > 0)
                selectorIndex--;

            //설렉터 텍스트 제거
            for(int i = 0; i < command.Length; i++)
                command[i].text = command[i].text.Replace(selectorText, "");

            //설렉터 추가
            command[selectorIndex].text = selectorText + command[selectorIndex].text;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            ConfirmSelector();
        }
    }

    /// <summary>
    /// 설렉터 초기화
    /// </summary>
    void InitSelector()
    {
        selectorIndex = 0;

        //설렉터 추가
        command[selectorIndex].text = selectorText + command[selectorIndex].text;
    }

    /// <summary>
    /// 선택한 메뉴 인덱스값 출력
    /// </summary>
    /// <returns></returns>
    public string ConfirmSelector()
    {
        Debug.Log(selectorIndex + "번 메뉴 선택, " + command[selectorIndex].text);
        return command[selectorIndex].text;
    }

    /// <summary>
    /// InitPanel 정지
    /// </summary>
    public void StopPanel()
    {
        this.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// InitPanl 시작
    /// </summary>
    public void StartPanel()
    {
        this.gameObject.SetActive(true);
    }
}
