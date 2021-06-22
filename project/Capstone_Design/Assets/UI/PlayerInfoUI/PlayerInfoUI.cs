using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    public GameObject panel;
    public Text levelText;
    public Text playerNameText;
    public Text playerCalledText;
    public Text statHpText;
    public Text statSpText;
    public Text statAttackText;
    public Text statDefenceText;
    public Text statExpText;
    bool isActive = false;

    private void Start()
    {
        panel = transform.GetChild(0).gameObject;
        panel.SetActive(isActive);
        // 상세 텍스트 매핑.
        levelText        = panel.transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>();
        playerNameText   = panel.transform.GetChild(1).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>();
        playerCalledText = panel.transform.GetChild(1).GetChild(1).GetChild(2).GetChild(0).GetComponent<Text>();
        statHpText       = panel.transform.GetChild(3).GetChild(0).GetChild(5).GetChild(0).GetComponent<Text>();
        statSpText       = panel.transform.GetChild(3).GetChild(0).GetChild(6).GetChild(0).GetComponent<Text>();
        statAttackText   = panel.transform.GetChild(3).GetChild(0).GetChild(7).GetChild(0).GetComponent<Text>();
        statDefenceText  = panel.transform.GetChild(3).GetChild(0).GetChild(8).GetChild(0).GetComponent<Text>();
        statExpText      = panel.transform.GetChild(3).GetChild(0).GetChild(9).GetChild(0).GetComponent<Text>();
        // 키 매핑
        MappingInfo mapping = new MappingInfo("PlayerInfoUI");
        mapping.AddMapping("CloseUI : ", "esc");
        mapping.Enroll("PlayerInfoUI");
    }


    private void Update()
    {
    }
    
    public void OpenUI(Message message) {
        isActive = true;
        panel.SetActive(isActive);
        Sync();
    }

    public void CloseUI(Message message) {
        isActive = false;
        panel.SetActive(isActive);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void Sync() {
        playerNameText.text = "테스트 플레이어";
        playerCalledText.text = "테스터";

        Message getLevel = new Message("PlayInfoManager/GetData : Level").FunctionCall();
        levelText.text = "" + (int) getLevel.returnValue[0];
        
        Message getHp = new Message("PlayInfoManager/GetData : Hp").FunctionCall();
        statHpText.text = $"{(int) getHp.returnValue[0]} / {(int)getHp.returnValue[3]}";
        
        Message getSp = new Message("PlayInfoManager/GetData : Sp").FunctionCall();
        statSpText.text = $"{(int) getSp.returnValue[0]} / {(int)getSp.returnValue[3]}";
        
        Message getAttack = new Message("PlayInfoManager/GetData : Attack").FunctionCall();
        statAttackText.text = "" + (int) getAttack.returnValue[0];
        
        Message getDefence = new Message("PlayInfoManager/GetData : Defense").FunctionCall();
        statDefenceText.text = "" + (int) getDefence.returnValue[0];
        
        Message getExp = new Message("PlayInfoManager/GetData : Exp").FunctionCall();
        statExpText.text = "" + (int) getExp.returnValue[0];        
    }
}
