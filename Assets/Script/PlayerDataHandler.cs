using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Newtonsoft.Json.Linq;

public class PlayerDataHandler : MonoBehaviour
{
    private DatabaseReference databaseRef;
    private string UserDataPath => "users";             // -> /users/
    private string StageDataPath => "stagesInfo";        // -> /users/uid/coin
    private string SavestageDataPath => "savedStage";    // -> /users/uid/coin
    private string stageKey = "currentSceneIndex";

    // Save data
    public CoinObject playerCoin;

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
        playerCoin.SetsavedStageIndex(currentSceneIndex);
        StageObject.Instance.SetSavedStage(currentSceneIndex);
        string stageJson = playerCoin.ToStageJson();

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

        string coinJson = playerCoin.ToJson();
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

        LoadUserData(userId);
        LoadSavedStageData(userId);
    }

    private void SaveUserData(string userId, string coinJson)
    {
        databaseRef.Child(UserDataPath).Child(userId).Child(StageDataPath).SetRawJsonValueAsync(coinJson).ContinueWith(task =>
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
            Debug.LogFormat("Save user data successfully: {0} {1}", userId, coinJson);
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
        databaseRef.Child(UserDataPath).Child(userId).Child(StageDataPath).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("Load coin data was canceled");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("Load coin data encountered an error: " + task.Exception);
                return;
            }

            DataSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                playerCoin.FromJson(snapshot.GetRawJsonValue());
          
                Debug.LogFormat("Load coin data successfully: {0} {1}", userId, snapshot.GetRawJsonValue());
            }
            else
            {
                Debug.LogWarning("No coin data found for the user.");
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
                int savedStageValue = savedStageObject.Value<int>("savedStage");
                StageObject.Instance.SetSavedStage(savedStageValue);
                Debug.LogFormat("Loaded saved stage value: {0}", savedStageValue);
            }
            else
            {
                Debug.LogWarning("No saved stage data found for the user.");
            }
        });
    }
}
