using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageObject : MonoBehaviour
{
    private static StageObject instance = null;
    private string userid;
    private int savedStage;
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


    public int GetSavedStage()
    {
        return savedStage;
    }
}
