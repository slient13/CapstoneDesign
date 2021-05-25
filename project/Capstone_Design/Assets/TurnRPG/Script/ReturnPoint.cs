using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPoint : MonoBehaviour
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
            mover.SendMessage("SetReturning", false);
            mover.SendMessage("EndMove");
            mover.SendMessage("SetEnable",true);
            Debug.Log(npc + "가 원래자리로 돌아옴");
        }
    }
}
