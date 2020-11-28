using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public List<GameObject> UILayer = new List<GameObject>();    
    public GameObject canvas;
    int currentLayer;

    void Start() {
        canvas = GameObject.Find("Canvas");
        
    }
    void Update() {
        currentLayer = UILayer.Count - 1;
    }

}