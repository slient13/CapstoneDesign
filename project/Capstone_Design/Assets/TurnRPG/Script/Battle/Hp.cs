using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    public float hpPercentage = 1f;
    public float hpAmount = 0f;

    Image hpImage;
    float initHp;

    // Start is called before the first frame update
    void Start()
    {
        hpImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        hpPercentage = initHp / hpAmount;
        hpImage.fillAmount = hpPercentage;
    }

    /// <summary>
    /// 'float'만큼 체력 증가
    /// </summary>
    /// <param name="value"></param>
    void AddHp(float value)
    {
        hpAmount += value;
    }

    /// <summary>
    /// 'float'만큼의 체력을 설정
    /// </summary>
    /// <param name="value"></param>
    public void SetHpSize(float value)
    {
        hpAmount = value;
        initHp = hpAmount;
    }
}
