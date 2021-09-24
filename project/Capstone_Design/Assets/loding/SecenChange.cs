using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SecenChange : MonoBehaviour
{
    public void SceneChange()
    {
        new Message("GameProcessManager/ChangeScene : finalsecen").FunctionCall();
    }
}