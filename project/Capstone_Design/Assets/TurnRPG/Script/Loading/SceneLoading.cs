using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    [SerializeField]
    public Image _progressBar;

    // Start is called before the first frame update
    void Start()
    {
        //SceneManager.LoadScene("BattleScene");
        //async 시작
        StartCoroutine(LoadAsyncOperation());
    }

    public static void LoadScene(string sceneName)
    {
        Message msg = new Message("GameprocessManager/ChangeScene : " + sceneName).FunctionCall();
    }
    
    IEnumerator LoadAsyncOperation()
    {
        yield return null;
        //게임 저장
        Message msg = new Message("GameprocessManager/Save : ").FunctionCall();

        //async 만들기
        AsyncOperation levelProgress = SceneManager.LoadSceneAsync("BattleScene");
        levelProgress.allowSceneActivation = false;

        float timer = 0.0f;
        
        while (!levelProgress.isDone)
        {
            yield return null;

            if(levelProgress.progress < 0.8f)
            {
                //_progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, 0.9f, Time.deltaTime);
                _progressBar.fillAmount = levelProgress.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                _progressBar.fillAmount = Mathf.Lerp(0.8f, 1f, timer);
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
