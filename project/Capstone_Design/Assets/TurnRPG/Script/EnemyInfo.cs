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
        // 테스트용 코드.
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

    public struct Skill {
        public string code {get;}
        public string name {get;}
        public double effect {get;}
        public Skill(string code, string name, double effect) {
            this.code = code;
            this.name = name;
            this.effect = effect;
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
            skillList.Add(new Skill(skillCode, skillName, skillEffect));
        }
        // 드롭 입력
        foreach(string dropString in dropStringList) {
            string[] temp = dropString.Split(',');
            string dropItemCode = temp[0].Trim();
            double rate = Convert.ToDouble(temp[1].Trim());
            dropList.Add(new Drop(dropItemCode, rate));            
        }
    }
}