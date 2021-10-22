using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField]
    private Image _progressBar;

    // Start is called before the first frame update
    void Start()
    {
        //async 시작
        StartCoroutine(LoadAsyncOperation());
    }

    
    IEnumerator LoadAsyncOperation()
    {
        Message msg = new Message("GameProcessManager/ChangeScene : BattleScene");

        //async 만들기
        AsyncOperation levelProgress = SceneManager.LoadSceneAsync("BattleScene");
        levelProgress.allowSceneActivation = false;

        float timer = 0.0f;
        
        while (levelProgress.progress < 1)
        {
            yield return null;

            if(levelProgress.progress < 0.9f)
            {
                _progressBar.fillAmount = levelProgress.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(_progressBar.fillAmount >= 1.0f)
                {
                    levelProgress.allowSceneActivation = true;
                    yield break;
                }
            }
        }
        //끝나면 씬로딩
    }
}
