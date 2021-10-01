using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProcessManager : MonoBehaviour
{
    List<string> sceneNameList;
    // Start is called before the first frame update
    void Start()
    {
        sceneNameList = new List<string>();
        sceneNameList.Add("finalSecen");
        sceneNameList.Add("LodingSecen");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CloseGame(Message message) {
        Debug.Log("GameProcessManager/CloseGame : is called");
        new Message("InventoryManager/SaveInventory :").FunctionCall();
        new Message("PlayInfoManager/SavePlayData : ").FunctionCall();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // 디버그 중 실행 종료.
        #else
                Application.Quit(); // 어플리케이션 종료
        #endif
    }

    public void ChangeScene(Message message) {
        string targetName = (string) message.args[0];
        bool isMatched = false;        
        foreach(string sceneName in sceneNameList) if (sceneName == targetName) isMatched = true;

        if (isMatched == true) {
            new Message("InventoryManager/SaveInventory : ").FunctionCall();
            new Message("PlayInfoManager/SavePlayData : ").FunctionCall();
            SceneManager.LoadScene(targetName);
        }
        else Debug.Log("GameProcessManager/ChangeScene.Error : There is no Scene " + targetName);
    }
}
