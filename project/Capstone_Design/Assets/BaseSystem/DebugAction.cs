using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 각종 디버그용 키매핑을 모아둔 클래스.
public class DebugAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // PlayInfoManager 관련.
        MappingInfo playInfoManagerDebug = new MappingInfo("PlayInfoManager");
        playInfoManagerDebug.AddMapping("ChangeData : Hp, 10", "_ctrlL, arrowU");
        playInfoManagerDebug.AddMapping("ChangeData : Hp, -10", "_ctrlL, arrowD");
        playInfoManagerDebug.AddMapping("ChangeData : Money, 1000", "_altL, arrowU");
        playInfoManagerDebug.AddMapping("ChangeData : Money, -1000", "_altL, arrowD");
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

        EventListener.GetEventListener().Binding("BaseSystem", "None", "DebugAction/EventTest : 123");
        new Message("BaseSystem/None : 456, 789").FunctionCall();
    }

    public void EventTest(Message message) {
        List<int> values = new List<int>();
        values.Add((int) message.args[0]);
        ArrayList temp = (ArrayList) message.args[1];
        values.Add((int) temp[0]);
        values.Add((int) temp[1]);
        Debug.Log($"DebugAction/EventTest.values : {values[0]}, {values[1]}, {values[2]}");
    }
}
