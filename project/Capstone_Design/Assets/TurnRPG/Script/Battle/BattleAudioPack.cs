using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioPack : MonoBehaviour
{
    public AudioSource menuSelectSound;
    public AudioSource menuConfirmSound;
    public AudioSource errorSound;
    public AudioSource nextTexSound;
    public AudioSource enemyHit;
    public AudioSource playerHit;
    public AudioSource textSound;
    public AudioSource deadSound;
    public AudioSource itemUse;

    AudioSource[] sources;

    //public bool testBool;

    private void Start()
    {
        sources = new AudioSource[this.gameObject.transform.childCount];
    }

    ////테스트목적, 필히 삭제하고 사용
    //private void Update()
    //{
    //    testBool = IsPlaying();
    //}

    public void PlaySelectSound()
    {
        if (menuSelectSound != null)
            menuSelectSound.GetComponent<AudioSource>().Play();
    }

    public void PlayConfirmSound()
    {
        if (menuConfirmSound != null)
            menuConfirmSound.GetComponent<AudioSource>().Play();
    }

    public void PlayErrorSound()
    {
        if (errorSound != null)
            errorSound.GetComponent<AudioSource>().Play();
    }

    public void PlayNextText()
    {
        if (nextTexSound != null)
            nextTexSound.GetComponent<AudioSource>().Play();
    }

    public void PlayEnemyHit()
    {
        if (enemyHit != null)
            enemyHit.GetComponent<AudioSource>().Play();
    }

    public void PlayPlayerHit()
    {
        PlaySound(playerHit);
    }

    public void PlayTextSound()
    {
        PlaySound(textSound);
    }

    public void PlayDeadSound()
    {
        PlaySound(deadSound);
    }

    public void PlayItemUse()
    {
        PlaySound(itemUse);
    }

    /// <summary>
    /// 사운드 재생 예외처리
    /// </summary>
    /// <param name="obj"></param>
    void PlaySound(AudioSource obj)
    {
        if (obj != null)
            obj.Play();
    }

    /// <summary>
    /// BGM인덱스가 0번째여야 할것
    /// </summary>
    /// <returns></returns>
    public bool IsPlaying()
    {
        //BGM인덱스가 첫번째여야 할것

        bool isPlaying = false;

        for (int i = 1; i < this.gameObject.transform.childCount; i++)
        {
            sources[i] = this.gameObject.transform.GetChild(i).GetComponent<AudioSource>();
            if(sources[i].isPlaying)
            {
                isPlaying = true;
            }
        }

        return isPlaying;
    }
}
