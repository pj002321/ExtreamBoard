using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UserScore
{
    #region Variables
    public static string userIdPath = "user_id";
    public static string userNamePath = "username";
    public static string scorePath = "score";
    public static string timestampPath = "timestamp";
    public static string otherDatePath = "data";


    public string userId;
    public string userName;
    public long score;
    public long timestamp;
    public Dictionary<string, object> otherData;
    #endregion Variables


    #region Properties
    public string ShortDateString
    {
        get
        {
            var scoreData = new DateTimeOffset(new DateTime(timestamp * TimeSpan.TicksPerSecond, DateTimeKind.Utc)).LocalDateTime;
            return scoreData.ToShortDateString() + " " + scoreData.ToShortTimeString();
        }
    }
    #endregion Properties

    #region Methods
    public UserScore(string userId, string userName, long score, long timestamp, Dictionary<string, object> otherdata = null)
    {
        this.userId = userId;
        this.userName = userName;
        this.score = score;
        this.timestamp = timestamp;
        this.otherData = otherdata;
    }

    // 데이터베이스에서에서의 데이터 항목
    public UserScore(DataSnapshot record)
    {
        userId = record.Child(userIdPath).Value.ToString();
        if (record.Child(userNamePath).Exists)
        {
            userName = record.Child(userNamePath).Value.ToString();
        }

        long score;
        if (Int64.TryParse(record.Child(scorePath).Value.ToString(), out score))
        {
            this.score = score;
        }
        else
        {
            this.score = Int64.MinValue;
        }

        long timestamp;
        if (Int64.TryParse(record.Child(timestampPath).Value.ToString(), out timestamp))
        {
            this.timestamp = timestamp;
        }

        if (record.Child(otherDatePath).Exists && record.Child(otherDatePath).HasChildren)
        {
            this.otherData = new Dictionary<string, object>();
            foreach (var keyvalue in record.Child(otherDatePath).Children)
            {
                otherData[keyvalue.Key] = keyvalue.Value;
            }
        }
    }

    public static UserScore CreateScoreFromRecord(DataSnapshot record)
    {
        if (record == null)
        {
            Debug.LogWarning("NUll DataSnapShot record in UserScore.CreateScoreFromRecord");
            return null;
        }

        if (record.Child(userIdPath).Exists && record.Child(scorePath).Exists && record.Child(timestampPath).Exists)
        {
            return new UserScore(record);
        }

        Debug.LogWarning("Invalid record format in UserScore.CreateScoreFromRecord");
        return null;
    }

    public Dictionary<string, object> ToDictionary()
    {
        return new Dictionary<string, object>() {
            { userIdPath,userId},
            { userNamePath,userName},
            { scorePath,score},
            { timestampPath,timestamp},
            { otherDatePath,otherData}
        };

    }
    #endregion Methods
}
