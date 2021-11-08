using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    private Image _progressBar;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("BattleScene");
        //async 시작
        StartCoroutine(LoadAsyncOperation());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        Message msg = new Message("GameprocessManager/ChangeScene : " + sceneName).FunctionCall();
    }
    
    IEnumerator LoadAsyncOperation()
    {
        //async 만들기
        AsyncOperation levelProgress = SceneManager.LoadSceneAsync("BattleScene");
        levelProgress.allowSceneActivation = false;

        float timer = 0.0f;
        
        while (levelProgress.progress < 1)
        {
            yield return null;

            if(levelProgress.progress < 0.8f)
            {
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
