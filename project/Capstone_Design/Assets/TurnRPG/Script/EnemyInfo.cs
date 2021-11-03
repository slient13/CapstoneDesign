using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
         // 테스트용 코드. 실 사용시 주석 처리 할 것
        { // 몹 정보 불러오기 테스트
            enemy = ExternalFileSystem.SingleTon().GetEnemyInfo("Bear");
            Debug.Log($"enemy : type = {enemy.type}, code = {enemy.code}, name = {enemy.name}, hp = {enemy.hp}, attack = {enemy.attack}, defence = {enemy.defence}");
            string temp_skill_string = "";
            foreach(Enemy.Skill skill in enemy.skillList)
            {
                if (temp_skill_string == "") temp_skill_string += $"enemy : skill = {skill.code}, {skill.name}, {skill.effect}";
                else temp_skill_string += $" / {skill.code}, {skill.name}, {skill.effect}";
            }
            Debug.Log(temp_skill_string);
            string temp_drop_string = "";
            foreach(Enemy.Drop drop in enemy.dropList)
            {
                if (temp_drop_string == "") temp_drop_string += $"enemy : drop = {drop.dropItemCode}, {drop.rate}";
                else temp_drop_string += $" / {drop.dropItemCode}, {drop.rate}";
            }
            Debug.Log(temp_drop_string);
        }
        { // 스킬 사용 및 아이템 드랍 테스트
            Enemy enemy = ExternalFileSystem.SingleTon().GetEnemyInfo("Bear");
            for (int i = 0; i < 10; ++i)
            {
                Enemy.Skill skill = enemy.GetRandomSkill();
                Debug.Log($"EnemyInfo.debug.SkillandDrop : skill name = {skill.name}, skill coefficient = {skill.effect}, skill timing = {skill.timing}");
            }

            for (int i = 0; i < 10; ++i)
            {
                List<string> drop_list = enemy.GetDrops();
                string drops_string = "";
                foreach(string drop_name in drop_list)
                {
                    if (drops_string == "") drops_string += drop_name;
                    else drops_string += $", {drop_name}";
                }

                if (drops_string == "") drops_string = "none";
                Debug.Log($"EnemyInfo.debug.SkillandDrop : drop list = {drops_string}.");
            }
        }
        { // infoManager 관련 코드 테스트
            InfoManager im = new InfoManager();
            Debug.Log($"infoManager.debug : Hp = {im.GetHp()}");
            Debug.Log($"infoManager.debug : Sp = {im.GetSp()}");
            Debug.Log($"infoManager.debug : Atk = {im.GetAtk()}");
            Debug.Log($"infoManager.debug : Def = {im.GetDef()}");
            Debug.Log($"infoManager.debug : Evasion = {im.GetEvasion()}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnemyInfo(Enemy val)
    {
        enemy = val;
    }

    Enemy GetEnemyInfo()
    {
        return enemy;
    }

    void OnDie()
    {

    }
}

public class Enemy {
    public string type {get;}
    public string code {get;}
    public string name {get;}
    public int hp {get;}
    public int attack {get;}
    public int defence {get;}
    public List<Skill> skillList;
    public List<Drop> dropList;
    public int total_weight = 0;

    public struct Skill {
        public string code {get;}
        public string name {get;}
        public double effect {get;}
        public int timing {get;}
        public Skill(string code, string name, double effect, int timing) {
            this.code = code;
            this.name = name;
            this.effect = effect;
            this.timing = timing;
        }
    }

    public struct Drop {
        public string dropItemCode {get;}
        public double rate {get;}
        public Drop(string dropItemCode, double rate) {
            this.dropItemCode = dropItemCode;
            this.rate = rate;
        }
    }

    public Enemy(string type = "None", string code = "None", string name = "None"
                    , string hp = "0", string attack = "0", string defence = "0"
                    , List<string> skillStringList = null, List<string> dropStringList = null) {
        this.type = type;
        this.code = code;
        this.name = name;
        // 숫자 자료형 할당.
        this.hp = Convert.ToInt32(hp);
        this.attack = Convert.ToInt32(attack);
        this.defence = Convert.ToInt32(defence);
        // 메모리 할당.
        skillList = new List<Skill>();
        dropList = new List<Drop>();
        // 스킬 입력
        foreach(string skillString in skillStringList) {
            string[] temp = skillString.Split(',');
            string skillCode = temp[0].Trim();
            string skillName = temp[1].Trim();
            double skillEffect = Convert.ToDouble(temp[2].Trim());
            int skillWeight = Convert.ToInt32(temp[3].Trim());
            skillList.Add(new Skill(skillCode, skillName, skillEffect, skillWeight + total_weight));
            total_weight += skillWeight;
        }
        // 드롭 입력
        foreach(string dropString in dropStringList) {
            string[] temp = dropString.Split(',');
            string dropItemCode = temp[0].Trim();
            double rate = Convert.ToDouble(temp[1].Trim());
            dropList.Add(new Drop(dropItemCode, rate));            
        }
    }

    public Enemy.Skill GetRandomSkill()
    {
        Enemy.Skill output = this.skillList[0];

        for (int i = this.skillList.Count-1; i >= 0; --i)
        {
            Skill skill = this.skillList[i];
            int timing = UnityEngine.Random.Range(1, this.total_weight + 1);
            // Debug.Log($"Enemy.GetRandomSkill.debug : timing = {timing}, total_weight = {this.total_weight}, skill timing = {skill.timing}");
            if (skill.timing <= timing)
            {
                output = skill;
                break;
            }
        }

        return output;
    }

    public List<string> GetDrops()
    {
        List<string> output = new List<string>();

        foreach(Drop drop in this.dropList)
        {
            float rate = UnityEngine.Random.Range(0.0f, 1.0f);
            if ((double) rate <= drop.rate)
            {
                new Message($"InventoryManager/ModifyItem : {drop.dropItemCode}, 1").FunctionCall();
                string name = ((Item) new Message($"InventoryManager/GetItem : {drop.dropItemCode}").FunctionCall().returnValue[0]).GetItemName();
                output.Add(name);
            }
        }

        return output;
    }
}