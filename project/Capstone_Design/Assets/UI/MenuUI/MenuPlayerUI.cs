using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPlayerUI : MonoBehaviour
{
    public StatBar hpBar;
    public StatBar spBar;
    // Start is called before the first frame update
    void Start()
    {
        hpBar = new StatBar("hpBar");
        hpBar.width = transform.GetChild(1).gameObject.GetComponent<RectTransform>().rect.width;
        hpBar.panel = transform.GetChild(1).GetChild(0).gameObject.GetComponent<RectTransform>();
        hpBar.text = transform.GetChild(1).GetChild(1).gameObject.GetComponent<Text>();
        spBar = new StatBar("spBar");
        spBar.width = transform.GetChild(2).gameObject.GetComponent<RectTransform>().rect.width;
        spBar.panel = transform.GetChild(2).GetChild(0).gameObject.GetComponent<RectTransform>();
        spBar.text = transform.GetChild(2).GetChild(1).gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Message getHp = new Message("PlayInfoManager/GetData : Hp").FunctionCall();
        hpBar.Sync(
            (int) getHp.returnValue[0],  // value
            (int) getHp.returnValue[3]); // max
        Message getStamina = new Message("PlayInfoManager/GetData : Sp").FunctionCall();
        spBar.Sync(
            (int) getStamina.returnValue[0],  // value
            (int) getStamina.returnValue[3]); // max
    }
}

public class StatBar {
    public string name;
    public int degree;
    public int max;
    public float width;
    public RectTransform panel;
    public Text text;
    public StatBar(string name) {
        this.name = name;
    }    

    public void Sync(int degree, int max) {
        this.degree = degree;
        this.max = max;
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, degree * width / 100);
        text.text = $"{degree}/{max}";
    }
}
