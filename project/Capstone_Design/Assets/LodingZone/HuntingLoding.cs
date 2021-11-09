using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HuntingLoding : MonoBehaviour
{

    public void SceneChange()
    {
        new Message("GameProcessManager/ChangeScene : HuntingLodingScene").FunctionCall();
    }

}