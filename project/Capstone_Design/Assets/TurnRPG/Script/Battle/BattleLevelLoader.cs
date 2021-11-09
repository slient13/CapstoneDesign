using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLevelLoader : MonoBehaviour
{
    public BattleManager battleManager;

    void EndPlay()
    {
        battleManager.StartGame();
    }
}
