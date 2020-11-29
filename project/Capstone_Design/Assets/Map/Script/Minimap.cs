using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public float minimapSize;


    GameObject player;
    Camera minimapCamera;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        minimapCamera = GameObject.Find("MinimapCamera").GetComponent<Camera>();

        minimapCamera.orthographicSize = minimapSize;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = player.transform.position + new Vector3(0, 100, 0);
    }
}
