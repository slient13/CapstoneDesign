using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReplacer : MonoBehaviour
{
    public Dictionary<string, Vector3> SceneAndPosMappingInfoList;
    public GameObject player;
    void Start()
    {
        SceneAndPosMappingInfoList = new Dictionary<string, Vector3>();
        Vector3 tempPosOfRacingZone = GameObject.Find("RacingZone").transform.position + new Vector3(-3, 0, 0);
        Vector3 tempPosOfHuntingZone = GameObject.Find("HuntingZone").transform.position + new Vector3(-3, 0, 0);
        SceneAndPosMappingInfoList.Add("Racing", tempPosOfRacingZone);
        SceneAndPosMappingInfoList.Add("HuntingField", tempPosOfHuntingZone);

        player = GameObject.Find("Player");

        string lastSceneName = (string)new Message($"GetPlayInfoValue : System.Process.LastSceneName").FunctionCall().returnValue[0];

        foreach(KeyValuePair<string, Vector3> mapping in this.SceneAndPosMappingInfoList)
        {
            Debug.Log($"PlayerReplacer.debug : lastSceneName = {lastSceneName}, mapping.key = {mapping.Key}");
            if (lastSceneName == mapping.Key)
            {
                player.transform.position = mapping.Value;
                break;
            } 
        }
    }
}