using Firebase;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Leaderboader
{
    public class UserScoreArgs : EventArgs
    {
        public UserScore score;
        public string message;

        public UserScoreArgs(UserScore score, string message)
        {
            this.score = score;
            this.message = message;
        }
    }

    public class LeaderboardArgs : EventArgs
    {
        public DateTime startData;
        public DateTime endData;
        public List<UserScore> scores;
    }

    [Serializable]
    public class UDateTime
    {
        public DateTime dateTime;

        public UDateTime()
        {
            dateTime = DateTime.UtcNow;
        }

        public UDateTime(DateTime dateTime)
        {
            this.dateTime = dateTime;
        }

        public override string ToString()
        {
            return dateTime.ToString("o"); // ISO 8601 format
        }
    }

    public class LeaderboardController : MonoBehaviour
    {
        #region Fields

        private bool initialized = false;
        private bool readyToInitialize = false;
        private event EventHandler Oninitialized;

        private DatabaseReference databaseRef;

        private bool sendAddedScoreEvent = false;
        private bool addingUserScore = false;
        private UserScoreArgs addedScoreArgs;
        public event EventHandler<UserScoreArgs> OnAddedScore;

        private bool gettingTopScores = false;
        private bool sendRetrievedScoreEvent = false;
        private UserScoreArgs retrievedScoreArgs;
        public event EventHandler<UserScoreArgs> OnRetrievedScore;

        private bool getUserScoreCallQueued = false;
        private bool gettingUserScore = false;

        private bool sendUpdatedScoreEvent = false;
        private UserScoreArgs updatedScoreArgs;
        public event EventHandler<UserScoreArgs> OnUpdatedScore;

        private bool sendUpdatedLeaderboardEvent = false;
        public event EventHandler<LeaderboardArgs> OnUpdatedLeaderboard;

        public UDateTime startDateTime;
        private long StartTimeTicks => startDateTime.dateTime.Ticks / TimeSpan.TicksPerSecond;

        public UDateTime endDateTime;
        private long EndTimeTicks
        {
            get
            {
                long endTimeTicks = endDateTime.dateTime.Ticks / TimeSpan.TicksPerSecond;
                if (endTimeTicks <= 0)
                {
                    endTimeTicks = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
                }

                return endTimeTicks;
            }
        }

        private List<UserScore> topScores = new List<UserScore>();
        public List<UserScore> TopScores => topScores;
        private Dictionary<string, UserScore> userScores = new Dictionary<string, UserScore>();

        #endregion Fields

        #region Firebase Database Path

        private string internalAllScoreDataPath = "all_scores";
        public string AllScoreDataPath => internalAllScoreDataPath;

        #endregion Firebase Database Path

        #region Unity Methods

        private void Start()
        {
            FirebaseInitializer.Initialize(dependencyStatus =>
            {
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    readyToInitialize = true;
                    InitializeDatabase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }

        void Update()
        {
            if (sendAddedScoreEvent)
            {
                sendAddedScoreEvent = false;
                OnAddedScore?.Invoke(this, addedScoreArgs);
            }

            if (sendRetrievedScoreEvent)
            {
                sendRetrievedScoreEvent = false;
                OnRetrievedScore?.Invoke(this, retrievedScoreArgs);
            }

            if (sendUpdatedLeaderboardEvent)
            {
                sendUpdatedLeaderboardEvent = false;
                OnUpdatedLeaderboard?.Invoke(this, new LeaderboardArgs
                {
                    scores = topScores,
                    startData = startDateTime.dateTime,
                    endData = endDateTime.dateTime
                });
            }
        }

        #endregion Unity Methods

        #region Methods

        private void InitializeDatabase()
        {
            if (initialized)
            {
                return;
            }

            FirebaseApp app = FirebaseApp.DefaultInstance;
            databaseRef = FirebaseDatabase.DefaultInstance.RootReference;
            initialized = true;
            readyToInitialize = false;
            Oninitialized?.Invoke(this, null);
        }

        public async Task AddScore(string userId, string userName, long score, long timestamp = -1L, Dictionary<string, object> otherdata = null)
        {
            if (timestamp <= 0)
            {
                timestamp = DateTime.UtcNow.Ticks / TimeSpan.TicksPerSecond;
            }

            var userScore = new UserScore(userId, userName, score, timestamp, otherdata);
            await AddScore(userScore);
        }

        public async Task AddScore(UserScore userScore)
        {
            if (addingUserScore)
            {
                Debug.LogError("Running add User Score task");
                return;
            }

            var scoreDictionary = userScore.ToDictionary();
            addingUserScore = true;

            try
            {
                var newEntry = databaseRef.Child(AllScoreDataPath).Push();
                await newEntry.SetValueAsync(scoreDictionary);

                addedScoreArgs = new UserScoreArgs(userScore, userScore.ToString() + " Added!");
                sendAddedScoreEvent = true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning("Exception adding score: " + ex);
            }
            finally
            {
                addingUserScore = false;
            }
        }

        public void GetUserScore(string userId)
        {
            if (gettingUserScore) return;

            gettingUserScore = true;
            StartCoroutine(GetUserScoreCoroutine(userId));
        }

        private IEnumerator GetUserScoreCoroutine(string userId)
        {
            var task = databaseRef.Child(AllScoreDataPath)
                .OrderByChild(UserScore.userIdPath)
                .StartAt(userId)
                .EndAt(userId)
                .GetValueAsync();

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception.ToString());
                gettingUserScore = false;
                yield break;
            }

            if (task.Result.ChildrenCount == 0)
            {
                retrievedScoreArgs = new UserScoreArgs(null, string.Format("No Scores for User {0}", userId));
            }
            else
            {
                var scores = ParseValidUserScoreRecords(task.Result, -1, -1).ToList();
                if (scores.Count == 0)
                {
                    retrievedScoreArgs = new UserScoreArgs(null, string.Format("No score for User {0} Within", userId));
                }
                else
                {
                    var orderedScores = scores.OrderBy(s => s.score);
                    var userScore = orderedScores.Last();

                    retrievedScoreArgs = new UserScoreArgs(userScore, userScore.userId + " Retrieve!");
                }
            }

            gettingUserScore = false;
            sendRetrievedScoreEvent = true;
        }

        private List<UserScore> ParseValidUserScoreRecords(DataSnapshot snapshot, long startTS, long endTS)
        {
            return snapshot.Children
                .Select(scoreRecord => UserScore.CreateScoreFromRecord(scoreRecord))
                .Where(score => score != null && score.timestamp > startTS && score.timestamp <= endTS)
                .Reverse()
                .ToList();
        }

        public void GetInitialTopScores(long batchEnd)
        {
            if (gettingTopScores) return;

            gettingTopScores = true;
            StartCoroutine(GetInitialTopScoresCoroutine(batchEnd));
        }

        private IEnumerator GetInitialTopScoresCoroutine(long batchEnd)
        {
            var query = databaseRef.Child(AllScoreDataPath).OrderByChild("score").LimitToLast(20);
            var task = query.GetValueAsync();

            yield return new WaitUntil(() => task.IsCompleted);

            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception.ToString());
                gettingTopScores = false;
                yield break;
            }

            if (!task.Result.HasChildren)
            {
                Debug.LogWarning("Failed to get top scores.");
                gettingTopScores = false;
                yield break;
            }

            var scores = ParseValidUserScoreRecords(task.Result, -1, -1);
            foreach (var userScore in scores)
            {
                if (!userScores.ContainsKey(userScore.userId))
                {
                    userScores[userScore.userId] = userScore;
                }
                else
                {
                    if (userScores[userScore.userId].score < userScore.score)
                    {
                        userScores[userScore.userId] = userScore;
                    }
                }
            }

            SetTopScores();
        }

        private void SetTopScores()
        {
            topScores.Clear();
            topScores.AddRange(userScores.Values.OrderByDescending(score => score.score));
            sendUpdatedLeaderboardEvent = true;
            gettingTopScores = false;
        }

        #endregion Methods
    }
}
