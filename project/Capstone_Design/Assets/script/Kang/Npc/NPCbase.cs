using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase : MonoBehaviour
{
    public string npcName;

    // Start is called before the first frame update
    void Start()
    {
        if (npcName == "")
            Debug.Log(this.name + "오브젝트 NPC의 이름이 설정되지 않았습니다!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
