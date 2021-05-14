using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RallyPoint : MonoBehaviour
{
    public GameObject npc;
    RandomMover mover;

    private void Start()
    {
        mover = npc.GetComponent<RandomMover>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == npc)
        {
            transform.localPosition = new Vector3(0, -1000, 0);
            mover.SendMessage("EndMove");
            Debug.Log("Collide" + other.name);
        } 
    }
}
