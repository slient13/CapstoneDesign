using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAudioPack : MonoBehaviour
{
    public AudioSource menuSelectSound;
    public AudioSource menuConfirmSound;
    public AudioSource errorSound;
    public AudioSource nextTexSound;

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
