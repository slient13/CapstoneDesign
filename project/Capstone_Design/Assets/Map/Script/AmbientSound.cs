using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientSound : MonoBehaviour
{
    public AudioSource WaveAmbient;
    public float soundRadius;
    public float multiply;

    public float distance;

    // Start is called before the first frame update
    void Start()
    {
        soundRadius /= 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Water")
        {
            WaveAmbient.Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Water")
        {
            distance = Vector3.Distance(other.transform.position, this.transform.position);

            WaveAmbient.volume = soundRadius / distance * multiply;
        }
    }
}
