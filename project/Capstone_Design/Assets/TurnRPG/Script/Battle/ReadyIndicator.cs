using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyIndicator : MonoBehaviour
{
    public GameObject battleManagerObj;
    public BattleManager battleManager;

    private void Start()
    {
        battleManager = battleManagerObj.GetComponent<BattleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            battleManager.NextState();
        }
    }
}
