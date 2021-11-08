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
    
    public void OpenUI() {
        isActive = true;
        panel.SetActive(isActive);
        Sync();
    }

    public void CloseUI() {
        isActive = false;
        panel.SetActive(isActive);
        new Message("ControlManager/LayerChanger : general").FunctionCall();
    }

    public void Sync() {
        playerNameText.text = "테스트 플레이어";
        playerCalledText.text = "테스터";

        Message getLevel = new Message("GetPlayInfoValue : Player.Stat.Level").FunctionCall();
        levelText.text = "" + (int) getLevel.returnValue[0];
        
        Message getHp = new Message("GetPlayInfo : Player.Stat.Hp").FunctionCall();
        PlayInfo playInfo_Hp = (PlayInfo) getHp.returnValue[0];
        PlayInfo playInfo_Hp_data = playInfo_Hp.GetDataList()[0];
        int hp_value = (int) playInfo_Hp_data.GetValue();
        int hp_max = playInfo_Hp_data.GetRange()[1];
        statHpText.text = $"{hp_value} / {hp_max}";
        
        Message getSp = new Message("GetPlayInfo : Player.Stat.Sp").FunctionCall();
        PlayInfo playInfo_Sp = (PlayInfo) getSp.returnValue[0];
        PlayInfo playInfo_Sp_data = playInfo_Sp.GetDataList()[0];
        int sp_value = (int) playInfo_Sp_data.GetValue();
        int sp_max = playInfo_Sp_data.GetRange()[1];
        statSpText.text = $"{sp_value} / {sp_max}";
        
        Message getAttack = new Message("GetPlayInfoValue : Player.Stat.Attack").FunctionCall();
        statAttackText.text = "" + (int) getAttack.returnValue[0];
        
        Message getDefence = new Message("GetPlayInfoValue : Player.Stat.Defense").FunctionCall();
        statDefenceText.text = "" + (int) getDefence.returnValue[0];
        
        Message getExp = new Message("GetPlayInfoValue : Player.Stat.Exp").FunctionCall();
        statExpText.text = "" + (int) getExp.returnValue[0];        
    }
}
