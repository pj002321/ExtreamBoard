using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StageCoinData
{
    public int stage;
    public int coins;

    public StageCoinData(int stage, int coins)
    {
        this.stage = stage;
        this.coins = coins;
    }
}

[Serializable]
public class PlayerData
{
    public List<StageCoinData> stageCoins = new List<StageCoinData>();

    public PlayerData()
    {
        for (int i = 1; i <= 3; i++)
        {
            stageCoins.Add(new StageCoinData(i, 0));
        }
    }
    public int savedstage;

}

[CreateAssetMenu(fileName = "New Coin", menuName = "Player System/New Player CoinInfo")]
[JsonObject(MemberSerialization.OptIn)]
public class CoinObject : ScriptableObject
{
    [JsonProperty]
    public PlayerData stageData = new PlayerData();
    public Action<CoinObject> OnChangedCoin;

    public int GetCoinsForStage(int stage)
    {
        StageCoinData stageCoinData = stageData.stageCoins.Find(s => s.stage == stage);
        if (stageCoinData != null)
        {
            return stageCoinData.coins;
        }
        else
        {
            Debug.LogError($"Stage {stage} does not exist.");
            return 0;
        }
    }

    public void SetsavedStageIndex(int stage)
    {
        stageData.savedstage = stage;
       
    }
    public int GetsavedStageIndex()
    {
        return stageData.savedstage;
    }
    public void SetCoinsForStage(int stage, int coins)
    {
        StageCoinData stageCoinData = stageData.stageCoins.Find(s => s.stage == stage);
        if (stageCoinData != null)
        {
            stageCoinData.coins = coins;
            OnChangedCoin?.Invoke(this);
        }
        else
        {
            Debug.LogError($"Stage {stage} does not exist.");
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(stageData, Formatting.Indented);
    }
    public string ToStageJson()
    {
        return JsonConvert.SerializeObject(stageData.savedstage, Formatting.Indented);
    }
    public void FromJson(string jsonString)
    {
        PlayerData newData = JsonConvert.DeserializeObject<PlayerData>(jsonString);
        stageData = newData;
        
        OnChangedCoin?.Invoke(this);
    }
    public void FromStageJson(string jsonString)
    {
        int newData = JsonConvert.DeserializeObject<int>(jsonString);
        stageData.savedstage = newData;
    }
}
