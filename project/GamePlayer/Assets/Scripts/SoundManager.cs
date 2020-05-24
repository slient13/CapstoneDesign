using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string soundName; //사운드의 이름
    public AudioClip clip; //오디오클립 = mp3
}
public class SoundManager : MonoBehaviour
{
    //static은 공유 변수로써 어디서든 쉽게 참조 변경이 가능하다.
    public static SoundManager instance;

    // bgm 플레이어
    //소리는 한개 뿐만이 아니라 여러개일수도 있으므로 배열로 생성한다.
    [SerializeField] Sound[] bgmSounds;
    [SerializeField] Sound[] sfxSounds;

    [Header("브금 플레이어")]
    [SerializeField] AudioSource bgmplayer;

    //효과음은 배경음악과 다르게 동시게 여러개가가 생길수도 있으므로 배열로 생성
    [Header("효과음 플레이어")]
    [SerializeField] AudioSource[] sfxplayer;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        //PlayRandomBGM();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySE(string _soundName)
    {
        for(int i=0; i < sfxSounds.Length; i++)
        {
            if(_soundName == sfxSounds[i].soundName)
            {
                for (int x = 0; x < sfxplayer.Length; x++)
                {
                    //x번째의 mp3플레이어가 재생중이지 않다면 만족하는 조건문
                    if (!sfxplayer[x].isPlaying)
                    {
                        //재생중이지 않은 x번쨰 mp3플레이어에 전 조건문에서 찾아낸 i번쨰 mp3를 넣어준다.
                        sfxplayer[x].clip = sfxSounds[i].clip;
                        sfxplayer[x].Play();
                        //return을 써서 불필요한 반복을 없애서 빠져나오게 한다.
                        return;
                    }
                }
                Debug.Log("모든 효과음 플레이어가 사용중입니다!!");
                //리턴값을 주지않으면 효과음을 찾고도 계속 for 문을 돌게된다.
                return;
            }
        }
        Debug.Log("등록된 효과음이 없습니다");
    }
    /*
    public void PlayRandomBGM()
    {
        int random = Random.Range(0, 2);
        bgmplayer.clip = bgmSounds[random].clip;
        bgmplayer.Play();
    }
    */
}
