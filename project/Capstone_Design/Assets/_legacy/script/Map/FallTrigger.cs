using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour
{
    private GameObject gamemanager;


    private void Start()
    {
        gamemanager = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("플레이어 떨어짐!");
            gamemanager.SendMessage("PlayerPosInit");
        }
    }
}
