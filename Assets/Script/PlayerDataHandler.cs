using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

public class PlayerDataHandler : MonoBehaviour
{
    private DatabaseReference databaseRef;
    private string UserDataPath => "users";              // -> /users/
    private string StageDataPath => "stagesInfo";        // -> /users/uid/
    private string SavestageDataPath => "savedStage";    // -> /users/uid/coin
    private string StageCoinsPath => "stageCoins";       // -> /users/uid/stagesInfo

    // Save data
    public GameData gameData;
    private static PlayerDataHandler instance = null;
    public static PlayerDataHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerDataHandler>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("UI");
                    instance = obj.AddComponent<PlayerDataHandler>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    void Start()
    {
        databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
    
    }

    public void OnClickCurStageSave()
    {
        var userId = StageObject.Instance.GetUserInfo();
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;
        gameData.SetsavedStageIndex(currentSceneIndex);
        StageObject.Instance.SetSavedStage(currentSceneIndex);
        string stageJson = gameData.ToStageJson();

        SaveStageData(userId, stageJson);
    }

    public void OnClickedSave()
    {
        var userId = StageObject.Instance.GetUserInfo();
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }


        // 업데이트된 데이터를 포함하여 JSON 문자열 생성
        string coinJson = gameData.ToJson();

        SaveUserData(userId, coinJson);
    }

    public void OnClickedLoad()
    {
        var userId = StageObject.Instance.GetUserInfo();
        if (string.IsNullOrEmpty(userId))
        {
            Debug.LogError("User is not logged in.");
            return;
        }

        //LoadUserData(userId);
        LoadSavedStageData(userId);
    }
    private void SaveUserData(string userId, string coinJson)
    {
        string userStageDataPath = $"{UserDataPath}/{userId}/{StageDataPath}";

        databaseRef.Child(userStageDataPath).SetRawJsonValueAsync(coinJson).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Save user data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Save user data encountered an error: " + task.Exception);
                return;
            }
            Debug.LogFormat("Save user data successfully: {0}", userId);
        });
    }


    private void SaveStageData(string userId, string stageJson)
    {
        databaseRef.Child(UserDataPath).Child(userId).Child(StageDataPath)
            .Child(SavestageDataPath).SetRawJsonValueAsync(stageJson).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Save current stage data was canceled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Save current stage data encountered an error: " + task.Exception);
                    return;
                }
                Debug.LogFormat("Save current stage data successfully: {0} {1}", userId, stageJson);
            });
    }

    private void LoadUserData(string userId)
    {
        databaseRef.Child(UserDataPath).Child(userId).Child(StageDataPath).Child(SavestageDataPath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Load saved stage data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Load saved stage data encountered an error: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                // savedStage 데이터만을 가져와서 처리
                int savedStageValue = int.Parse(snapshot.GetRawJsonValue());
                StageObject.Instance.SetSavedStage(savedStageValue);
                Debug.LogFormat("Loaded saved stage value: {0}", savedStageValue);
            }
            else
            {
                Debug.LogWarning("No saved stage data found for the user.");
            }
        });
    }


    private void LoadSavedStageData(string userId)
    {
        databaseRef.Child(UserDataPath).Child(userId).Child(StageDataPath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Load saved stage data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Load saved stage data encountered an error: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                string savedStageJson = snapshot.GetRawJsonValue();
                JObject savedStageObject = JObject.Parse(savedStageJson);

                // Extracting the value of "savedstage"
                int savedStageValue = savedStageObject.Value<int>("savedstage");
                StageObject.Instance.SetSavedStage(savedStageValue);
                Debug.LogFormat("Loaded saved stage value: {0}", savedStageObject);
                Debug.LogFormat("Loaded saved savedStageValue value: {0}", savedStageValue);
            }
            else
            {
                Debug.LogWarning("No saved stage data found for the user.");
            }
        });
    }


    public void UpdateStageCoinData(int stage,int coin)
    {
        var userId = StageObject.Instance.GetUserInfo();
        for (int i = 0; i < gameData.StageData.stageCoins.Count; i++)
        {
            int stageIndex = gameData.StageData.stageCoins[i].stage;
            stageIndex = stage;
            int coins = gameData.StageData.stageCoins[i].coins;
            coins= coin;
            string path = $"{UserDataPath}/{userId}/{StageDataPath}/{StageCoinsPath}/{stageIndex}/coin";
            databaseRef.Child(path).SetValueAsync(coins).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("Save stage coin data was canceled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("Save stage coin data encountered an error: " + task.Exception);
                    return;
                }
                Debug.LogFormat("Saved stage {0} coin data successfully: {1}", stageIndex, coins);
            });

            // gameData 객체에 업데이트된 코인 정보 설정
            var stageCoinData = gameData.StageData.stageCoins.FirstOrDefault(s => s.stage == stageIndex);
            if (stageCoinData != null)
            {
                stageCoinData.coins = coins;
            }
            else
            {
                gameData.StageData.stageCoins.Add(new StageCoinData(stageIndex, coins ));
            }
        }
    }

}
