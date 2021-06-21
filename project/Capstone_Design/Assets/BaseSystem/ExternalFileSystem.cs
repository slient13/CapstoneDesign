using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/*
외부 파일과의 통신을 위한 시스템.
인터페이스의 이름은 다음과 같은 의미를 가짐.
Get* : 아이템 정보 등 고정된 정보를 불러옴.                     경로 : Resource 폴더 아래.
Load* : 인벤토리 정보 등 지속적으로 변경되는 정보를 불러옴.     경로 : AppData 아래
Save* : 변경되는 정보를 기록함.                                 경로 : AppData 아래
*/
public class ExternalFileSystem 
{
    private static ExternalFileSystem single_instance = null;
    public static ExternalFileSystem SingleTon() {
        if (single_instance == null) single_instance = new ExternalFileSystem();
        return single_instance;
    }

    // 입력된 경로에서 폴더 경로만 남기는 함수.
    string extractDirectoryPath(string fullPath) {
        string[] pathParts = fullPath.Split('/');
        string output = "";
        for (int i = 0; i < pathParts.Length - 1; i++) {
            output += pathParts[i];
            if (i < pathParts.Length - 2) output += "/";
        }
        return output;
    }

    // Resource 폴더에 있는 파일을 읽어와 문자열을 전달하는 내부 메소드.
    List<string> fileReader(string fileDir, bool blankRemove = true, bool isUserData = false) {

        string filePath = "";
        if (isUserData == true) filePath = Application.persistentDataPath + "/" + fileDir + ".txt";
        else filePath = "Assets/Resources/" + fileDir + ".txt";
        // 파일 로드 확인
        Debug.Log("ExternalFileSystem.fileReader.filePath : " + filePath);
        // 파일 존재 여부 확인. 없으면 새로 만들고 재귀 호출.
        if (File.Exists(filePath) == false) {
            Debug.Log("ExternalFileSystem.fileReader.error : There is no file. Create new one.");
            string directoryPath = extractDirectoryPath(filePath);
            var directory = new DirectoryInfo(directoryPath);
            directory.Create();
            var file = File.CreateText(filePath);
            file.Close();
            return fileReader(fileDir, blankRemove, isUserData);
        }
        // 파일 읽기 스트림 생성.
        StreamReader reader = new StreamReader(filePath, System.Text.Encoding.UTF8);
        
        string line = "-";
        List<string> lineList = new List<string>();

        while(true)
        {
            line = reader.ReadLine();
            if (line == null) break;
            if (line == "" && blankRemove == true) continue;
            lineList.Add(line);
            // Debug.Log("ExternalFileSystem/fileReader.line : " + line);
        }
        
        reader.Close();
        return lineList;
    }
    // Resource 폴더 내에 지정한 경로로 파일을 기록하는 메소드.
    // isAppend = 이어쓰기 여부.
    // isUserData = 외부에 작성하는지 여부.
    bool fileWriter(string fileDir, List<string> contents, bool isAppend = false, bool isUserData = true) {
        
        string filePath = "";
        if (isUserData == true) filePath = Application.persistentDataPath + "/" + fileDir + ".txt";
        else filePath = "Assets/Resources/" + fileDir + ".txt";
        // 파일 로드 확인
        Debug.Log("ExternalFileSystem.fileWriter.filePath : " + filePath);
        // 파일 존재 여부 확인. 없으면 새로 만들고 재귀 호출.
        if (File.Exists(filePath) == false) {
            Debug.Log("ExternalFileSystem.fileWriter.error : There is no file. Create new one.");
            string directoryPath = extractDirectoryPath(filePath);
            var directory = new DirectoryInfo(directoryPath);
            directory.Create();
            var file = File.CreateText(filePath);
            file.Close();
            return fileWriter(fileDir, contents, isAppend, isUserData);
        }
        // // 파일 스트림 생성.
        // FileStream  f = new FileStream("Assets/Resources/" + fileDir + ".txt", FileMode.Append, FileAccess.Write);
        // 출력 스트림 생성. 인코딩 유니코드.
        StreamWriter writer = new StreamWriter(filePath, isAppend, System.Text.Encoding.UTF8);
        // 한 줄씩 쓰기
        foreach(string strData in contents) {
            writer.WriteLine(strData);
        }
        // 출력 스트림 닫기.
        writer.Close();
        return true;
    }
    public List<string> GetItemInfo(string[] itemInfoPathList) {
        List<string> output = new List<string>();
        foreach(string itemInfoPath in itemInfoPathList) {
            output.AddRange(fileReader(itemInfoPath));
        }
        return output;
    }

    public bool SaveInventory(List<ItemBox> itemBoxList) {        
        List<string> output = new List<string>();
        foreach (ItemBox item in itemBoxList) {
            output.Add(item.itemCode + ", " + item.itemNumber);
        }
        fileWriter("Inventory/InventoryInfo", output, isUserData:true);
        return true;
    }

    public List<string> LoadInventory() {
        return fileReader("Inventory/InventoryInfo", isUserData:true);
    }

    public ShopInfo GetShopInfo(string shopName) {
        List<string> shopItemList = fileReader("Shop/Info/" + shopName);
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
        return fileReader("Talk/TalkScript/" + talkName);
    }

    public List<string> GetPlayInfo(string fileName) {
        // 파일에서 읽어온 한줄 단위 문자열 리스트.    
        List<string> fileContents = fileReader(fileName);
        // 반환할 리스트
        List<string> outputList = new List<string>();
        // 루프문 내부에서 사용할 변수들
        string code = "";
        string type = "";
        string value = "";
        string min = "";
        string max = "";
        string outputString = "";
        // 한 줄씩 읽어가며 출력용 양식으로 가공, 리스트에 추가.
        foreach(string infoString in fileContents) {
            string[] temp = infoString.Split('=');
            string mode = temp[0].Trim();
            string content;
            if (temp.Length == 2) content = temp[1].Trim();
            else content = "";
            // 숏컷은 바로 입력.
            if (mode == "short") outputList.Add(content);
            // 그 외는 조합해서 입력.
            else {
                // 배치
                if (mode == "code") code = content;
                else if (mode == "type") type = content;
                else if (mode == "value") value = content;
                else if (mode == "min") min = content;
                else if (mode == "max") max = content;
                else if (mode == "end") {
                    outputString += $"{type}, {code}, {value}";
                    if (min != "" && max != "") outputString += $", {min}, {min}";
                    outputList.Add(outputString);
                    // 다음 값을 받기 위해 초기화.
                    code = "";
                    type = "";
                    value = "";
                    min = "";
                    max = "";
                    outputString = "";
                }
            }
        }
        return outputList;
    }
    public List<string> LoadPlayData() {
        return fileReader("PlayInfo/Total", isUserData:true);
    }
    public void SavePlayData(List<PlayInfo> playInfoList) {
        List<string> output = new List<string>();
        string infoName;
        string infoType;
        object infoValue;
        foreach(PlayInfo playInfo in playInfoList) {
            infoName = playInfo.GetInfoName();
            infoType = playInfo.GetInfoType();
            infoValue = playInfo.GetInfoValue();
            output.Add(infoName + ", " + infoValue);
        }

        fileWriter("PlayInfo/Total", output, isAppend:false, isUserData:true);
    }

    public Dictionary<string, Quest> LoadQeust(List<string> questNameList) {
        List<string> questStringList;        
        Dictionary<string, Quest> questList = new Dictionary<string, Quest>();
        foreach(string questName in questNameList) {
            questStringList = fileReader("Quest/Info/" + questName);            
            Quest quest = new Quest(questName);
            foreach(string questString in questStringList) {
                // mode 분리.
                string[] temp = questString.Split(':');
                string mode = temp[0].Trim();
                string info = temp[1];
                // info 분리.
                temp = info.Split(',');
                string type = temp[0].Trim();
                string code = temp[1].Trim();
                string value = temp[2].Trim();
                if (mode == "goal") quest.goalList.Add(new QuestInfo(type, code, value));
                else if (mode == "price") quest.priceList.Add(new QuestInfo(type, code, value));
                else if (mode == "reward") quest.rewardList.Add(new QuestInfo(type, code, value));
            }            
            questList.Add(quest.name, quest);
        }
        return questList;
    }

    public List<Creature> GetCreatureInfo (string fileName) {
        List<string> temp = fileReader(fileName);       // 임시
        List<Creature> output = new List<Creature>();   // 출력용
        // 선언
        string type = "None";
        string code = "None";
        string name = "None";
        string hp = "0";
        string attack = "0";
        string defense = "0";
        List<string> skill = new List<string>();
        List<string> drop = new List<string>();
        foreach(string tempString in temp) {
            // 모드 분리.
            string[] tempSplitedString = tempString.Split('=');
            string mode = tempSplitedString[0].Trim();
            // 대입.
            if (mode == "end") {
                output.Add(new Creature(type, code, name, hp, attack, defense, skill, drop));
                // 다음 입력을 위한 초기화.
                type = "None";
                code = "None";
                name = "None";
                hp = "0";
                attack = "0";
                defense = "0";
                skill = new List<string>();
                drop = new List<string>();
            }
            else {
                string values = tempSplitedString[1].Trim();
                if (mode == "type")         type = values;
                else if (mode == "code")    code = values;
                else if (mode == "name")    name = values;
                else if (mode == "hp")      hp = values;
                else if (mode == "attack")  attack = values;
                else if (mode == "defense") defense = values;
                else if (mode == "skill")   skill.Add(values);
                else if (mode == "drop")    drop.Add(values);
            }
            
        }
        return output;
    }
}
