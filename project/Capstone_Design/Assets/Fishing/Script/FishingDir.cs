using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingDir : MonoBehaviour
{
    RaycastHit hit;
    public float MaxRayRange = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * MaxRayRange, Color.red, 0.3f);
    }

    public bool GetWaterHit()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxRayRange))
        {
            if (hit.collider.tag == "Water")
                return true;
            else
                return false;
        }
        else
            return false;
    }
    
    public Vector3 GetWaterPos()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, MaxRayRange))
            return hit.point;
        else
            return Vector3.zero;
    }
}
