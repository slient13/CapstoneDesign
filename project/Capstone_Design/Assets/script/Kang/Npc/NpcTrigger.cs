using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcTrigger : MonoBehaviour
{
    public GameObject manager;
    public GameObject npc;
    public bool playerNotice = false;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
        manager = GameObject.Find("GameManager");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNotice = true;
            manager.SendMessage("NpcPlayerNotice", other.gameObject);
            animator.SetTrigger("Wave");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerNotice = true;
            manager.SendMessage("NpcPlayerIgnore", other.gameObject);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
