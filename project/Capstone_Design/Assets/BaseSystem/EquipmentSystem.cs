using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSystem : MonoBehaviour
{
    List<Equipment> equipment_info_list;
    List<Equipment> equiped_list;

    const int equip_slot_count = 4;

    void Start()
    {
        equipment_info_list = new List<Equipment>();
        equiped_list = new List<Equipment>();
        this.getEquipmentInfo();
        this.loadEquipmentState();
    }

    void getEquipmentInfo()
    {
        string[] targetFileNameList = {
            "Test/Equipment"
        };

        foreach (string targetFileName in targetFileNameList)
        {
            List<Equipment> tempList = ExternalFileSystem.SingleTon().GetEquipmentInfo(targetFileName);
            this.equipment_info_list.AddRange(tempList);
        }
    }
    void loadEquipmentState()
    {
        List<string> equiped_state_code_list = ExternalFileSystem.SingleTon().LoadEquipState();

        foreach (string equiped_state_code in equiped_state_code_list)
        {
            Equipment temp_equipment = null;
            foreach (Equipment equipment in this.equipment_info_list)
            {
                if (equipment.code == equiped_state_code)
                {
                    temp_equipment = equipment;
                    break;
                }
            }
            if (temp_equipment != null) this.equiped_list.Add(temp_equipment);
        }
    }
    void saveEquipState()
    {
        ExternalFileSystem.SingleTon().SaveEquipState(this.equiped_list);
    }
    public void Equip(Message message)
    {
        string equipment_code = (string)message.args[0];

        if (this.equiped_list.Count >= equip_slot_count)
        {
            Debug.Log("EquipmentSystem/Equip.error : slot is already full.");
            new Message($"InventoryManager/ModifyItem : {equipment_code}, 1");
            return;
        }

        Equipment target_equipment = null;
        foreach (Equipment equipment in this.equipment_info_list)
        {
            if (equipment.code == equipment_code)
            {
                target_equipment = equipment;
                break;
            }
        }

        if (target_equipment == null) Debug.Log($"EquipmentSystem/Equip.error : There is no matched equipment. input = {equipment_code}.");

        this.equiped_list.Add(target_equipment);
        foreach (EquipmentEffect effect in target_equipment.effect_list)
        {
            new Message($"ChangeData : {effect.target_code}, {effect.degree}").FunctionCall();
        }
    }
    public void Unequip(Message message)
    {
        int targetIndex = (int)message.args[0];

        if (targetIndex >= this.equiped_list.Count || targetIndex < 0)
        {
            Debug.Log($"EquipmentSystem/Unequip.error : out of range. input = {targetIndex}, count = {this.equiped_list.Count}.");
            return;
        }

        Equipment temp_equipment = this.equiped_list[targetIndex];

        foreach (EquipmentEffect effect in temp_equipment.effect_list)
        {
            new Message($"ChangeData : {effect.target_code}, {effect.degree * -1}");
        }

        // Debug.Log($"EquipmentSystem/Unequip.debug : temp_equipement.code = {temp_equipment.code}");

        new Message($"InventoryManager/ModifyItem : {temp_equipment.code}, 1").FunctionCall();
        this.equiped_list.RemoveAt(targetIndex);
    }
    public void GetEquipState(Message message)
    {
        message.returnValue.Add(this.equiped_list);
    }
    public void IsEquip(Message message)
    {
        string equipment_name = (string)message.args[0];
        Equipment target_equipment = null;
        foreach (Equipment equipment in this.equipment_info_list)
        {
            if (equipment.name == equipment_name)
            {
                target_equipment = equipment;
                break;
            }
        }
        if (target_equipment != null) message.returnValue.Add(true);
        else message.returnValue.Add(false);
    }
}

public class Equipment
{
    public string code { get; }
    public string name { get; }
    public List<EquipmentEffect> effect_list { get; }
    public Sprite img { get; }

    public Equipment(string code, string name, List<EquipmentEffect> effect_list, Sprite img)
    {
        this.code = code;
        this.name = name;
        this.effect_list = effect_list;
        this.img = img;
    }
    public Equipment(string code, string name, List<EquipmentEffect> effect_list) : this(code, name, effect_list, null) { }

    public string GetDesc()
    {
        string output = "";
        foreach (EquipmentEffect effect in effect_list)
        {
            output += effect.GetDesc() + "\n";
        }

        return output;
    }
}

public class EquipmentEffect
{
    public string target_code { get; }
    public int degree { get; }

    public EquipmentEffect(string target_code, int degree)
    {
        this.target_code = target_code;
        this.degree = degree;
    }

    public string GetDesc()
    {
        return $"{target_code} : {degree}";
    }
}