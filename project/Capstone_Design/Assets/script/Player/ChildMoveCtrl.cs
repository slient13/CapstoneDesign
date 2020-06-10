using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildMoveCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(0.0f, -90.0f * Time.deltaTime, 0.0f);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f);
        }
    }
}
