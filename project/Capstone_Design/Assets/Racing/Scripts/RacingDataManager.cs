using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacingDataManager
{
    int carCount = 5;

    public List<bool> GetUnlockList()
    {
        List<bool> unlock_list = ExternalFileSystem.SingleTon().LoadCarUnlockList();

        if (unlock_list.Count == 0)
        {
            for(int i = 0; i < carCount; ++i)
                unlock_list.Add(false);
            SaveUnlockList(unlock_list);
        }

        return unlock_list;
    }

    public void SaveUnlockList(List<bool> unlockList)
    {
        ExternalFileSystem.SingleTon().SaveCarUnlockList(unlockList);
    }
}