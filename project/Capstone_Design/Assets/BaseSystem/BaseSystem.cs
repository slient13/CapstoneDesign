using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSystem : MonoBehaviour
{
    public GameObject target;
    public PlayInfo root_playInfo = new PlayInfo("@root");

    void Start()
    {
    }

    // 임시로 아무런 동작을 요하지 않는 message 객체가 필요할 때 사용.
    public void None(Message message)
    {
        Debug.Log("BaseSystem.none is called.");
    }

    public void FunctionCaller(Message msg)
    {
        // Debug.Log(msg.args[0]);
        target = GameObject.Find(msg.targetName);
        // 타겟 포착 실패시 메세지 호출 후 함수 종료
        if (target == null)
        {
            Debug.Log("BaseSystem.functionCaller.error : Could not find object. targetName = " + msg.targetName);
            return;
        }
        else
        {
            target.SendMessage(msg.functionName, msg);
            EventListener.GetEventListener().EventCall(msg);
        }
        // Debug.Log(msg.returnValue[0]);
    }

    // 새 플레이 정보 등록 (외부용)
    public void CreatePlayInfo(Message msg)
    {
        string name = (string)msg.args[0];
        object value = msg.args[1];
        int min = 0;
        int max = 0;
        if (msg.args.Count == 4)
        {
            min = (int)msg.args[2];
            max = (int)msg.args[3];
        }

        string[] splited_name = name.Split('.');
        int level = 0;
        PlayInfo target_playinfo = this.root_playInfo;
        for (level = 0; level < splited_name.Length; ++level)
        {
            bool target_exist = false;
            List<PlayInfo> temp_playInfo_list = target_playinfo.GetSubList();
            for (int index = 0; index < temp_playInfo_list.Count; ++index)
            {
                PlayInfo playinfo = temp_playInfo_list[index];
                if (playinfo.name == splited_name[level])
                {
                    target_exist = true;
                    target_playinfo = playinfo;
                }
            }

            if (!target_exist)
            {
                PlayInfo temp_playinfo = new PlayInfo(splited_name[level]);
                target_playinfo.AddSubPlayInfo(temp_playinfo);
                target_playinfo = temp_playinfo;
            }
        }

        PlayInfo playinfo_data = null;

        if (msg.args.Count == 4)
        {
            if (value is int) playinfo_data = new PlayInfo((int)value, min, max);
            else if (value is float) playinfo_data = new PlayInfo((float)value, min, max);
        }
        else
        {
            if (value is int) playinfo_data = new PlayInfo((int)value);
            else if (value is float) playinfo_data = new PlayInfo((float)value);
            else if (value is string) playinfo_data = new PlayInfo((string)value, true);
        }

        target_playinfo.AddData(playinfo_data);

        Debug.Log($"BaseSystem.CreatePlayInfo.debug : name = {name}");
    }

    public void CreateInstance(Message message)
    {
        string name = (string)message.args[0];
        ulong input_id = 0;
        if (message.args.Count == 2)
        {
            input_id = (ulong)message.args[1];
        }

        PlayInfo origin_info = findInfo(name);
        ulong id_counter = input_id;
        foreach (PlayInfo instance in origin_info.GetInstanceList())
        {
            if (instance.id >= id_counter) id_counter = instance.id + 1;
        }
        PlayInfo instance_info = new PlayInfo(origin_info, id_counter);
        origin_info.AddInstance(instance_info);

        message.returnValue.Add(instance_info);
    }

    public void RemoveInstance(Message message)
    {
        string name = (string)message.args[0];
        int index = (int)message.args[1];

        PlayInfo target_info = findInfo(name);
        target_info.RemoveInstance(index);
    }
    public void SetData(Message msg)
    {
        string name = (string)msg.args[0];  // 변경 대상 이름.
        object value = msg.args[1];             // 변경 값.
        int index = 0;
        if (msg.args.Count == 3) index = (int)msg.args[2];

        PlayInfo target = findInfo(name, isSet: true);
        if (target == null) Debug.Log("BaseSystem/SetData.error : Invaild name = " + name);
        else
        {
            target.GetDataList()[index].SetValue(value);
        }
    }
    public void ChangeData(Message msg)
    {
        string name = (string)msg.args[0];  // 변경 대상 이름.
        object value = msg.args[1];             // 변경 값.
        int index = 0;
        if (msg.args.Count == 3) index = (int)msg.args[2];

        PlayInfo target = findInfo(name);
        if (target == null) Debug.Log("BaseSystem/ChangeData.error : Invaild name = " + name);
        else
        {
            // Debug.Log($"Basesystem.ChangeData.debug : name = {name}, value = {(int)value}");
            target.GetDataList()[index].ModifyValue(value);
        }
    }

    PlayInfo findInfo(string name, PlayInfo searching_start = null, bool isSet = false)
    {
        if (searching_start == null) searching_start = this.root_playInfo;

        string[] splited_name = name.Split('.');

        PlayInfo searching_target = searching_start;
        for (int level = 0; level < splited_name.Length; ++level)
        {
            // Debug.Log($"BaseSystem.findInfo.debug : name = {name}, searching_target.name = {searching_target.name}, .id = {searching_target.id}");
            string[] temp_string_list = splited_name[level].Split('[');
            string target_name = temp_string_list[0];
            ulong id = 0;
            if (temp_string_list.Length == 2)
                id = Convert.ToUInt64(temp_string_list[1].Substring(0, 1));

            List<PlayInfo> searching_target_subList = searching_target.GetSubList();
            List<PlayInfo> searching_target_instanceList = searching_target.GetInstanceList();
            bool findCheck = false;
            if (id == 0) for (int index = 0; index < searching_target_subList.Count; ++index)
                {
                    if (splited_name[level] == searching_target_subList[index].name)
                    {
                        findCheck = true;
                        searching_target = searching_target_subList[index];
                        break;
                    }
                }
            else if (id > 0)
            {
                foreach (PlayInfo instance in searching_target_instanceList)
                {
                    if (instance.id == id)
                    {
                        searching_target = instance;
                        break;
                    }
                }
                if (findCheck == false && isSet == true)
                {
                    Message createInstance = new Message($"CreateInstance : {searching_target.name}, {id}");
                    createInstance.FunctionCall();
                    searching_target = (PlayInfo)createInstance.returnValue[0];
                }
            }

            if (findCheck == false)
            {
                Debug.Log($"BaseSystem.findInfo.debug : findCheck = null. name = {name}, level = {level}");
                return null;
            }
        }

        return searching_target;
    }

    public void GetPlayInfo(Message msg)
    {
        if (msg.args.Count == 0) msg.returnValue.Add(this.root_playInfo);
        else
        {
            string name = (string)msg.args[0]; // 정보를 원하는 대상 이름.

            PlayInfo target = findInfo(name);
            msg.returnValue.Add(target);
        }
    }

    public void GetPlayInfoValue(Message message)
    {
        Message getPlayInfo = new Message($"GetPlayInfo : {(string)message.args[0]}");
        PlayInfo target = (PlayInfo)getPlayInfo.FunctionCall().returnValue[0];
        PlayInfo target_data = target.GetDataList()[0];
        message.returnValue.Add(target_data.GetValue());
    }
}

