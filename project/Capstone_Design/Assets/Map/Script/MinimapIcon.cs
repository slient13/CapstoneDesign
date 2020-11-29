using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIcon : MonoBehaviour
{
    public GameObject target;
    public float iconSize;
    public int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.localScale *= iconSize;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = target.transform.position + new Vector3 (0, 50 + layerMask, 0);
    }
}
