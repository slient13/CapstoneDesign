using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    GameObject player;
    bool fishGameStart;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        fishGameStart = false;
    }

    public void SetFishGame(bool isStart)
    {
        fishGameStart = isStart;
    }

    public bool GetFishGame()
    {
        return fishGameStart;
    }
}
