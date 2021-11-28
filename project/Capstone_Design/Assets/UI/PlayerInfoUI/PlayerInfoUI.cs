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

        int Level = (int) new Message("GetPlayInfoValue : Player.Stat.Level").FunctionCall().returnValue[0];
        levelText.text = $"{Level}";
        
        PlayInfo Hp = (PlayInfo) new Message("GetPlayInfo : Player.Stat.Hp").FunctionCall().returnValue[0];
        statHpText.text = $"{Hp.GetValue(0)} / {Hp.GetRange(0)[1]}";
        
        PlayInfo Sp = (PlayInfo) new Message("GetPlayInfo : Player.Stat.Sp").FunctionCall().returnValue[0];
        statSpText.text = $"{Sp.GetValue(0)} / {Sp.GetRange(0)[1]}";
        
        int Attack = (int) new Message("GetPlayInfoValue : Player.Stat.Attack").FunctionCall().returnValue[0];
        statAttackText.text = $"{Attack}";
        
        int Defence = (int) new Message("GetPlayInfoValue : Player.Stat.Defense").FunctionCall().returnValue[0];
        statDefenceText.text = $"{Defence}";
        
        int Exp = (int) new Message("GetPlayInfoValue : Player.Stat.Exp").FunctionCall().returnValue[0];
        statExpText.text = $"{Exp}";
    }
}
