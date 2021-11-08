using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingLoding : MonoBehaviour
{

    public void SceneChange()
    {
        new Message("GameProcessManager/ChangeScene : RacingLodingScene").FunctionCall();
    }

}