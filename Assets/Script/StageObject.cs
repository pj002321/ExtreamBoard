using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    private string userid;
    private int savedStage=0;
    private int coin=0;
    private static StageObject instance;
    public static StageObject Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageObject>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("UserObject");
                    instance = obj.AddComponent<StageObject>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }
    public void SetUserInfo(string userId)
    {
        Debug.Log(userId);
        userid = userId;
    }

    public string GetUserInfo()
    {
        Debug.Log(userid);
        return userid;
    }

    public int SetSavedStage(int stage)
    {
        savedStage = stage;
        return savedStage;
    }
    public int GetCoinIndex()
    {
        Debug.Log("CurCoin" + coin);
        return coin;
    }
    public int SetCoinIndex(int coinindex)
    {
        coin = coinindex;
        return coin;
    }
    public int GetSavedStage()
    {
        return savedStage;
    }


}
