using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            Debug.Log(other.gameObject.name + "들어옴");
            GameObject.Find("GameManager").SendMessage("GetGameObjName", other.gameObject.name);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Object")
        {
            Debug.Log(other.gameObject.name + "나감");
            GameObject.Find("GameManager").SendMessage("GetGameObjName", "DefaultObj");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
