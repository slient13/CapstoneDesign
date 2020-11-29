using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSound : MonoBehaviour
{
    public AudioClip rockSound;

    float hAxis;
    float vAxis;
    GameObject player;
    Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;
    }

    private void OnTriggerStay(Collider other)
    {
        //태그가 NotGround일때 나오는 소리
        if(other.tag != "NotGround" && other.tag != "Player" && moveVec == Vector3.zero)
        {
            this.GetComponent<AudioSource>().clip = rockSound;
            this.GetComponent<AudioSource>().Play();
        }
        else
        {
            this.GetComponent<AudioSource>().Stop();
        }
    }
}
