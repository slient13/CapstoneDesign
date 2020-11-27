using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingFloat : MonoBehaviour
{
    public bool isbaited;

    Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        initialPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(isbaited)
        {
            //찌가 물에 잠김
            this.transform.position = initialPos;
        }
    }

    public void SetPos(Vector3 pos)
    {
        this.transform.position = pos;
    }

    public void InitPos()
    {
        this.transform.position = initialPos;
    }

}
