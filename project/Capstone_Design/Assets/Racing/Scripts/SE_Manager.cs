//효과음 매니저 스크립트 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE_Manager : MonoBehaviour
{
  //SE_Manager도 인스턴스화 시킨다. => 다른 스크립트에서 이 스크립트에 접근이 용이
  public static SE_Manager instance;
  AudioSource sound; //오디오 소스 자리

  [Header("AudioClip")]
  public AudioClip btn; //버튼 효과음
  public AudioClip goal; //결승점 통과 효과음
  public AudioClip lap; //한바퀴 돌떄마다 효과음
  public AudioClip[] count; //카운트 세는 효과음들 


  private void Awake()
  {
      if(instance == null)
          instance = this;

       //게임을 시작하면, AudioSource를 가져온다.
       sound = GetComponent<AudioSource>();
  }

  //재생기능
  public void PlaySound(AudioClip clip)
  {
      //clip의 효과음을 PlayOneShot으로 재생시킨다.
      sound.PlayOneShot(clip);
  }
}
