using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyImage : MonoBehaviour
{
    public GameObject snakeImg;
    public GameObject tigerImg;

    InfoManager infoManager = new InfoManager();
    Animator animator;
    bool isPlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        string monsterCode = infoManager.GetSceneStartValue();

        switch(monsterCode)
        {
            case "Snake":
                snakeImg.SetActive(true);
                tigerImg.SetActive(false);
                break;
            case "Tiger":
                snakeImg.SetActive(false);
                tigerImg.SetActive(true);
                break;
        }

        animator = this.gameObject.GetComponent<Animator>();
    }

    public void PlayAttackAnim()
    {
        animator.Play("Attack");
        isPlaying = true;
    }

    public void PlayHitAnim()
    {
        animator.Play("Hit");
        isPlaying = true;
    }

    public void PlayDeadAnim()
    {
        animator.Play("Dead");
        isPlaying = true;
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    void EndAnim()
    {
        isPlaying = false;
    }
}
