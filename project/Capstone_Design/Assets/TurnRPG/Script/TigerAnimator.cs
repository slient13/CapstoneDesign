using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerAnimator : MonoBehaviour
{
    Animator animator;
    bool isWalk = false;
    bool isRun = false;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

   // Update is called once per frame
   
        //void Update() //실험용 코드
   // {
   //     if(Input.GetKey(KeyCode.Z) == true)
   //     {
   //         PlayWalk();
   //     }

   //     if (Input.GetKey(KeyCode.X) == true)
   //     {
   //         PlayRun();        
   //     }
        
   //     if (Input.GetKey(KeyCode.C) == true)
   //     {
   //         PlayHit();
   //     }
        
   //     if (Input.GetKey(KeyCode.V) == true)
   //     {
   //         PlaySound();        
   //     }
        
   //     if (Input.GetKey(KeyCode.B) == true)
   //     {
   //         PlayIdle();
   //     }

   // }

    public void PlayWalk()
    {
        animator.SetBool("Run",false);
        animator.SetBool("Walk", true);
    }
    public void PlayRun()
    {
        animator.SetBool("Run", true);
        animator.SetBool("Walk", false);
    }
    public void PlaySound()
    {
        PlayIdle();
        animator.SetBool("Sound", true);
    }
    public void PlayHit()
    {
        PlayIdle();
        animator.SetBool("Hit", true);
    }
    public void PlayIdle()
    {
        animator.SetBool("Run", false);
        animator.SetBool("Walk", false);
    }
}
