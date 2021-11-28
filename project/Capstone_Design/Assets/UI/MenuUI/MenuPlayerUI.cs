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
        // 이벤트 리스너 지정.
        EventListener.GetEventListener().Binding("BaseSystem", "ChangeData", "MenuUI/Sync : ");
        Sync();
    }
    public void Sync()
    {
        try
        {
            PlayInfo Hp = (PlayInfo) new Message("GetPlayInfo : Player.Stat.Hp").FunctionCall().returnValue[0];
            hpBar.Sync((int)Hp.GetValue(0), (int)Hp.GetRange(0)[1]);

            PlayInfo Sp = (PlayInfo) new Message("GetPlayInfo : Player.Stat.Sp").FunctionCall().returnValue[0];
            spBar.Sync((int)Sp.GetValue(0), (int)Sp.GetRange(0)[1]);
        }
        catch
        {
            Debug.Log("MenuPlayerUI.Sync.error : there is no hp/sp prograssBar");
        }
    }
}

public class StatBar
{
    public string name;
    public int degree;
    public int max;
    public float width;
    public RectTransform panel;
    public Text text;
    public StatBar(string name)
    {
        this.name = name;
    }

    public void Sync(int degree, int max)
    {
        this.degree = degree;
        this.max = max;
        panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, degree * width / 100);
        text.text = $"{degree}/{max}";
    }
}
