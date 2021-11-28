using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayInfo
{

    public string name { get; }
    public string type { get; }
    int value_int;
    float value_float;
    string value_string;
    public int id { get; }

    int min;
    int max;

    List<PlayInfo> data_list;
    List<PlayInfo> sub_playinfo_list;
    List<PlayInfo> instance_list;

    public PlayInfo(string name)
    {
        this.name = name;
        this.id = 0;
        this.type = "wrapper";
        this.value_int = 0;
        this.value_float = 0.0f;
        this.value_string = "";
        this.min = int.MinValue;
        this.max = int.MaxValue;
        this.data_list = new List<PlayInfo>();
        this.sub_playinfo_list = new List<PlayInfo>();
        this.instance_list = new List<PlayInfo>();
    }
    public PlayInfo(int value) : this("data")
    {
        this.name = name;
        this.type = "int";
        this.value_int = value;
    }
    public PlayInfo(int value, int min, int max) : this(value)
    {
        this.min = min;
        if (max >= min)
            this.max = max;
        else
            this.max = this.min;
    }
    public PlayInfo(float value) : this("data")
    {
        this.name = name;
        this.type = "float";
        this.value_float = value;
    }
    public PlayInfo(float value, int min, int max) : this(value)
    {
        this.min = min;
        if (max >= min)
            this.max = max;
        else
            this.max = this.min;
    }
    public PlayInfo(string value, bool isData) : this("data")
    {
        this.name = name;
        this.type = "string";
        this.value_string = value;
        this.min = 0;
    }

    public PlayInfo(PlayInfo origin, int id)
    {
        this.id = id;
        this.name = origin.name;
        this.type = origin.type;
        this.value_int = origin.value_int;
        this.value_float = origin.value_float;
        this.value_string = origin.value_string;
        this.min = origin.min;
        this.max = origin.max;
        foreach (PlayInfo data in origin.data_list)
        {
            PlayInfo copied_data = new PlayInfo(data, 0);
            this.data_list.Add(copied_data);
        }
        foreach (PlayInfo subPlayinfo in origin.sub_playinfo_list)
        {
            PlayInfo copied_subPlayinfo = new PlayInfo(subPlayinfo, 0);
            this.sub_playinfo_list.Add(copied_subPlayinfo);
        }
    }
    public object GetValue()
    {
        if (type == "int") return value_int;
        else if (type == "float") return value_float;
        else if (type == "string") return value_string;
        else return null;
    }
    public object GetValue(int index)
    {
        return this.data_list[index].GetValue();
    }

    public void SetValue(object value)
    {
        if (type == "int") this.value_int = (int)value;
        else if (type == "float")
        {
            try { this.value_float = (float)value; }
            catch { this.value_float = (int)value; }
        }
        else if (type == "string") this.value_string = (string)value;
        fixRangeOut();
    }

    public void ModifyValue(object value)
    {
        if (type == "int") this.value_int += (int)value;
        else if (type == "float")
        {
            try { this.value_float += (float)value; }
            catch { this.value_float += (int)value; }
        }
        else if (type == "string") this.value_string += (string)value;
        fixRangeOut();
    }

    void fixRangeOut()
    {
        if (value_int < this.min) value_int = this.min;
        else if (value_int > this.max) value_int = this.max;
        if (value_float < this.min) value_float = this.min;
        else if (value_float > this.max) value_float = this.max;
    }

    public List<PlayInfo> GetDataList()
    {
        return this.data_list;
    }

    public void AddData(PlayInfo data)
    {
        this.data_list.Add(data);
    }

    public List<PlayInfo> GetSubList()
    {
        return this.sub_playinfo_list;
    }

    public void AddSubPlayInfo(PlayInfo subPlayInfo)
    {
        this.sub_playinfo_list.Add(subPlayInfo);
    }

    public void ClearSubList()
    {
        this.sub_playinfo_list.Clear();
    }

    public void AddInstance(PlayInfo instance)
    {
        this.instance_list.Add(instance);
    }

    public List<PlayInfo> GetInstanceList()
    {
        return this.instance_list;
    }

    public void RemoveInstance(int index)
    {
        this.instance_list.RemoveAt(index);
    }

    public int[] GetRange()
    {
        return new int[] { min, max };
    }

    public int[] GetRange(int index)
    {
        return this.data_list[index].GetRange();
    }

    public void SetRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public void SetRange(int max)
    {
        this.min = 0;
        this.max = max;
    }
}