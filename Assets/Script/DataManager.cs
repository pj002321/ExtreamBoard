using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Cinemachine;
using System.IO;
using TMPro;
using Timer;
namespace Database
{
    public class DataManager : MonoBehaviour
    {
        private AllData datas;
        public static DataManager instance;
        private TimeUI timer;
        [SerializeField] TextAsset data;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] TextMeshProUGUI stageText;

        private CinemachineVirtualCamera cinemachineCam;
        string path;
        string filename = "Save";
        int currentSceneIndex;
        
        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }
            path = Application.persistentDataPath + '/';
            datas = JsonUtility.FromJson<AllData>(data.text);
            LoadData();
            UpdatePlayerStartPosition();
        }

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            UpdatePlayerStartPosition();
        }
        
        void UpdatePlayerStartPosition()
        {
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            StageData currentStage = null;
            foreach (var stage in datas.stage)
            {
                if (stage.stageID == currentSceneIndex)
                {
                    currentStage = stage;
                   
                    break;
                }
            }

            Debug.Log(datas.stage[0].CurrenStage);

            if (currentStage != null)
            {
                GameObject playerInstance = Instantiate(playerPrefab);
                Vector2 startPosition = new Vector2(currentStage.x, currentStage.y);
                Debug.Log(startPosition);
                playerInstance.transform.position = new Vector3(startPosition.x, startPosition.y, 0);
                cinemachineCam = FindObjectOfType<CinemachineVirtualCamera>();
                if (cinemachineCam != null)
                {
                    cinemachineCam.Follow = playerInstance.transform;
                }
                else
                {
                    Debug.LogError("CinemachineVirtualCamera not found in the scene.");
                }
            }
            else
            {
                Debug.LogError("Stage data not found for stage ID: " + currentSceneIndex);
            }
        }
       
        public void SaveData()
        {
            datas.stage[0].CurrenStage = currentSceneIndex;
            stageText.text = "Stage " + datas.stage[0].CurrenStage;
            string data = JsonUtility.ToJson(datas);
            File.WriteAllText(path + filename, data);
        }

        public void LoadData()
        {
            stageText.text = "Stage " + datas.stage[0].CurrenStage;
            string data = File.ReadAllText(path + filename);
            datas = JsonUtility.FromJson<AllData>(data);
        }

        public void LoadScene()
        {
            SceneManager.LoadScene(datas.stage[0].CurrenStage);
        }
    }

    [Serializable]
    public class AllData
    {
        public StageData[] stage;
    }

    [Serializable]
    public class StageData
    {
        public int stageID;
        public string stageName;
        public int x;
        public int y;
        public int CurrenStage;
    }

}