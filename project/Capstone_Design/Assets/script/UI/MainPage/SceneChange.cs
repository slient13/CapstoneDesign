using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    private string targetScene = "GamePlay";
    public void ChangeScene() {
        SceneManager.LoadScene(targetScene);
    }

    public void ExitGame() {
        Application.Quit();
    }
}
