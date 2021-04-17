using System.Collections;
using System.Collections.Generic;

/*
외부 파일과의 통신을 위한 시스템.
*/
public class ExternalFileSystem 
{
    private static ExternalFileSystem single_instance = null;
    public static ExternalFileSystem SingleTon() {
        if (single_instance == null) single_instance = new ExternalFileSystem();
        return single_instance;
    }
    public ArrayList GetItemInfo() {
        // 사용법 간략화.
        ArrayList output = new ArrayList();

        output.Add("Health, 체 력, 체력이다, PlayInfoManager/hpChange : 10");
        output.Add("Bug, 미 끼, 물고기를 잡기위한 미끼다, - ");

        return output;
    }

    public bool SaveInventory(ArrayList inventoryInfo) {
        // 테스트용 임시 코드.
        return true;
    }

    public bool LoadInventory() {
        // 테스트용 임시 코드.
        return true;
    }
}
