using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 각종 디버그용 키매핑을 모아둔 클래스.
public class DebugAction : MonoBehaviour
{
    public GameObject debugConsole;
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            // 디버그용 콘솔 생성.
            debugConsole = GameObject.Find("DefaultSetting/Canvas/DebugConsole");
            debugConsole.SetActive(false);
            // 디버그 콘솔 관련 매핑.
            // 열기
            MappingInfo openDebugConsole = new MappingInfo("DebugAction");
            openDebugConsole.AddMapping("OpenDebugConsole : ", "slash");
            openDebugConsole.Enroll();
            // 닫기
            MappingInfo closeDebugConsole = new MappingInfo("DebugAction");
            closeDebugConsole.AddMapping("CloseDebugConsole : 0", "esc");
            closeDebugConsole.AddMapping("CloseDebugConsole : 1", "enter");
            closeDebugConsole.Enroll("Debug");
        }
        catch
        {
            Debug.Log("DebugAction.error : There is no debug console.");
        }
        // PlayInfoManager 관련.
        MappingInfo playInfoManagerDebug = new MappingInfo("BaseSystem");
        playInfoManagerDebug.AddMapping("ChangeData : Player.Stat.Hp, 10", "_ctrlL, arrowU");
        playInfoManagerDebug.AddMapping("ChangeData : Player.Stat.Hp, -10", "_ctrlL, arrowD");
        playInfoManagerDebug.AddMapping("ChangeData : Player.Stat.Money, 1000", "_altL, arrowU");
        playInfoManagerDebug.AddMapping("ChangeData : Player.Stat.Money, -1000", "_altL, arrowD");
        playInfoManagerDebug.Enroll();
        // InventoryManager 관련. 
        MappingInfo inventoryManagerDebug = new MappingInfo("InventoryManager");
        inventoryManagerDebug.AddMapping("SaveInventory : ", "_shiftL, p");
        inventoryManagerDebug.AddMapping("ModifyItem : Test, 1", "_shiftL, n1");
        inventoryManagerDebug.Enroll();
        // GamaProcessManager 관련.
        MappingInfo gameProcessManagerDebug = new MappingInfo("GameProcessManager");
        gameProcessManagerDebug.AddMapping("CloseGame : ", "_ctrlL, _shiftL, q");
        gameProcessManagerDebug.AddMapping("ChangeScene : finalSecen", "_ctrlL, _shiftL, n1");
        gameProcessManagerDebug.AddMapping("ChangeScene : dummyScene ", "_ctrlL, _shiftL, n2");
        gameProcessManagerDebug.Enroll();

        // EventListener.GetEventListener().Binding("BaseSystem", "None", "DebugAction/EventTest : 123");
        // new Message("BaseSystem/None : 456, 789").FunctionCall();
    }

    // public void EventTest(Message message) {
    //     List<int> values = new List<int>();
    //     values.Add((int) message.args[0]);
    //     ArrayList temp = (ArrayList) message.args[1];
    //     values.Add((int) temp[0]);
    //     values.Add((int) temp[1]);
    //     Debug.Log($"DebugAction/EventTest.values : {values[0]}, {values[1]}, {values[2]}");
    // }

    public void OpenDebugConsole()
    {
        debugConsole.SetActive(true);
        InputField temp = debugConsole.transform.GetChild(0).gameObject.GetComponent<InputField>();
        temp.ActivateInputField();
        new Message("ControlManager/LayerChanger : Debug").FunctionCall();
    }

    public void CloseDebugConsole(Message message)
    {
        int isModify = (int)message.args[0];
        if (isModify == 1)
        {
            string command = debugConsole.transform.GetChild(0).gameObject.GetComponent<InputField>().text;
            Debug.Log($"DebugAction/CloseDebugConsole.command : {command}");
            new Message(command).FunctionCall();
        }
        new Message("ControlManager/LayerChanger : general").FunctionCall();
        debugConsole.SetActive(false);
    }
}
