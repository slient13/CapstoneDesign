using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        new Message("newPlayInfo : hp, int, 80").functionCall();
        new Message("newPlayInfo : time, float, 0").functionCall();
        new Message("newPlayInfo : money, int, 10000").functionCall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
