using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/*
외부 파일과의 통신을 위한 시스템.
*/
public class ExternalFileSystem 
{
    const int BLANK_LIMIT = 20;
    private static ExternalFileSystem single_instance = null;
    public static ExternalFileSystem SingleTon() {
        if (single_instance == null) single_instance = new ExternalFileSystem();
        return single_instance;
    }

    // Resource 폴더에 있는 파일을 읽어와 문자열을 전달하는 내부 메소드.
    List<string> fileLoader(string fileDir, bool blankRemove = true) {

        TextAsset data = Resources.Load(fileDir, typeof(TextAsset)) as TextAsset;
        StringReader sr = new StringReader(data.text);
        
        string line = "-";
        List<string> lineList = new List<string>();

        while(true)
        {
            line = sr.ReadLine();
            if (line == null) break;
            if (line == "" && blankRemove == true) continue;
            lineList.Add(line);
            // Debug.Log("ExternalFileSystem/fileLoader.line : " + line);
        }
        return lineList;
    }
    // Resource 폴더 내에 지정한 경로로 파일을 기록하는 메소드.
    bool fileWriter(string fileDir, List<string> contents, bool isAppend = false) {
        // // 파일 스트림 생성.
        // FileStream  f = new FileStream("Assets/Resources/" + fileDir + ".txt", FileMode.Append, FileAccess.Write);
        // 출력 스트림 생성. 인코딩 유니코드.
        StreamWriter writer = new StreamWriter("Assets/Resources/" + fileDir + ".txt", isAppend, System.Text.Encoding.Unicode);
        // 한 줄씩 쓰기
        foreach(string strData in contents) {
            writer.WriteLine(strData);
        }
        // 출력 스트림 닫기.
        writer.Close();
        return true;
    }
    public List<string> GetItemInfo() {
        return fileLoader("Inventory/ItemInfo");
    }

    public bool SaveInventory(List<ItemBox> itemBoxList) {        
        List<string> output = new List<string>();
        foreach (ItemBox item in itemBoxList) {
            output.Add(item.itemCode + ", " + item.itemNumber + "\n");
        }
        fileWriter("Inventory/InventoryInfo", output);
        return true;
    }

    public List<string> LoadInventory() {
        return fileLoader("Inventory/InventoryInfo");
    }

    public ShopInfo GetShopInfo(string shopName) {
        List<string> shopItemList = fileLoader("Shop/Info/" + shopName);
        ShopInfo shopInfo = new ShopInfo(shopName);
        foreach(string shopItem in shopItemList) {
            // 문자열 분리, 트림.
            string[] itemInfoes = shopItem.Split(',');
            // 좌, 우 공백 제거.
            for (int i = 0; i < itemInfoes.Length; i++) {
                itemInfoes[i] = itemInfoes[i].Trim();
            }
            // 구매 등록.
            if (itemInfoes[0] == "b") shopInfo.buyList.Add(itemInfoes[1], Convert.ToInt32(itemInfoes[2]));
            // 판매 등록.
            else if (itemInfoes[0] == "s") shopInfo.sellList.Add(itemInfoes[1], Convert.ToInt32(itemInfoes[2]));
        }

        return shopInfo;
    }

    public List<string> GetTalkInfo(string talkName) {
        return fileLoader("Talk/TalkScript/" + talkName);
    }
}
