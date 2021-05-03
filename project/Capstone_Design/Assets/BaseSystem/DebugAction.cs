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
        playInfoManagerDebug.AddMapping("ChangeHp : 10", "_ctrlL, arrowU");
        playInfoManagerDebug.AddMapping("ChangeHp : -10", "_ctrlL, arrowD");
        playInfoManagerDebug.AddMapping("ChangeMoney : 1000", "_altL, arrowU");
        playInfoManagerDebug.AddMapping("ChangeMoney : -1000", "_altL, arrowD");
        playInfoManagerDebug.Enroll();                
        // InventoryManager 관련. 
        MappingInfo inventoryManagerDebug = new MappingInfo("InventoryManager");
        inventoryManagerDebug.AddMapping("SaveInventory : ", "_shiftL, p");
        inventoryManagerDebug.Enroll();
        // GamaProcessManager 관련.
        MappingInfo gameProcessManagerDebug = new MappingInfo("GameProcessManager");
        gameProcessManagerDebug.AddMapping("CloseGame : ", "_ctrlL, _shiftL, q");
        gameProcessManagerDebug.AddMapping("ChangeScene : finalSecen", "_ctrlL, _shiftL, n1");
        gameProcessManagerDebug.AddMapping("ChangeScene : dummyScene ", "_ctrlL, _shiftL, n2");
        gameProcessManagerDebug.Enroll();
    }
}
