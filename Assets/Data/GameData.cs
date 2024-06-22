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
    [JsonProperty]
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
public class GameData : ScriptableObject
{
    [JsonProperty]
    public PlayerData StageData{ get; set; } = new PlayerData(); 
    public Action<GameData> OnChangedData;

    public int GetCoinsForStage(int stage)
    {
        StageCoinData stageCoinData = StageData.stageCoins.Find(s => s.stage == stage);
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
        StageData.savedstage = stage;
       
    }
    public int GetsavedStageIndex()
    {
        return StageData.savedstage;
    }
    public void SetCoinsForStage(int stage, int coins)
    {
        StageCoinData stageCoinData = StageData.stageCoins.Find(s => s.stage == stage);
        if (stageCoinData != null)
        {
            stageCoinData.coins = coins;
            OnChangedData?.Invoke(this);
        }
        else
        {
            Debug.LogError($"Stage {stage} does not exist.");
        }
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(StageData, Formatting.Indented);
    }
    public string ToStageJson()
    {
        return JsonConvert.SerializeObject(StageData.savedstage, Formatting.Indented);
    }
    public void FromJson(string jsonString)
    {
        StageData = JsonConvert.DeserializeObject<PlayerData>(jsonString);
    }
    public void FromStageJson(string jsonString)
    {
        StageData.savedstage = JsonConvert.DeserializeObject<int>(jsonString);
    }
}
