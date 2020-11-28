using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDrowned : MonoBehaviour
{
    GameObject player;
    Vector3 initPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        initPos = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            player.transform.position = initPos;
        }
    }
}
