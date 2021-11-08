﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameProcessManager : MonoBehaviour
{
    public List<string> sceneNameList;
    // Start is called before the first frame update
    void Start()
    {
        sceneNameList = new List<string>();
        sceneNameList.Add("finalSecen");
        sceneNameList.Add("dummyScene");
        sceneNameList.Add("Rpg_Intro");
        sceneNameList.Add("Racing");
        sceneNameList.Add("LodingSecen");
        sceneNameList.Add("LodingSecen2");
        sceneNameList.Add("CoverSecen");
        sceneNameList.Add("TutorialSecen");
        sceneNameList.Add("BattleScene");
        sceneNameList.Add("HuntingField");
        sceneNameList.Add("RacingLodingScene");
        sceneNameList.Add("HuntingLodingScene");
  
        string LastSceneName = (string) new Message($"GetPlayInfoValue : System.Process.LastSceneName").FunctionCall().returnValue[0];
        Debug.Log($"GameProcessManager.debug : LastSceneName = {LastSceneName}");
    }
    public void Save()
    {
        new Message("InventoryManager/SaveInventory :").FunctionCall();
        new Message("PlayInfoManager/SavePlayData : ").FunctionCall();
        new Message("EquipmentSystem/SaveEquipState : ").FunctionCall();
        new Message("QuestManager/SaveQuestProcess : ").FunctionCall();
    }

    public void CloseGame(Message message)
    {
        Debug.Log("GameProcessManager/CloseGame : is called");
        this.Save();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // 디버그 중 실행 종료.
        #else
                Application.Quit(); // 어플리케이션 종료
        #endif
    }

    public void ChangeScene(Message message)
    {
        string targetName = (string)message.args[0];
        string passString = "none";
        if (message.args.Count == 2) passString = (string)message.args[1];
        new Message($"SetData : System.Process.SceneChangeValue, {passString}").FunctionCall();

        Scene scene = SceneManager.GetActiveScene();
        new Message($"SetData : System.Process.LastSceneName, {scene.name}").FunctionCall();

        bool isMatched = false;
        foreach (string sceneName in sceneNameList) if (sceneName == targetName) isMatched = true;

        if (isMatched == true)
        {
            this.Save();
            SceneManager.LoadScene(targetName);
        }
        else Debug.Log("GameProcessManager/ChangeScene.Error : There is no Scene " + targetName);
    }
}
