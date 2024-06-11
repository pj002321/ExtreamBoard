using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleSheetsToUnity;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Cinemachine;
using TMPro;

namespace Database
{
    public class DataManager : MonoBehaviour
    {

        public static DataManager instance;
        public DataBase data;

        [SerializeField] GameObject playerPrefab;
        
        private CinemachineVirtualCamera cinemachineCam;
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
        #region DataMethods
        void UpdatePlayerStartPosition()
        {
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

            StageData currentStage = new StageData();
            foreach (var stage in data.DataList)
            {
                if (stage.id == currentSceneIndex)
                {
                    currentStage = stage;
                    break;
                }
            }

            GameObject playerInstance = Instantiate(playerPrefab);
            Vector2 startPosition = new Vector2(currentStage.x, currentStage.y);
            playerInstance.transform.position = new Vector3(startPosition.x, startPosition.y, 0);
            cinemachineCam = FindObjectOfType<CinemachineVirtualCamera>();
            if (cinemachineCam != null)
            {
                cinemachineCam.Follow = playerInstance.transform;
            }
        }

     
        public void SaveData()
        {
           
            data.ReturnCurrentScene(SceneManager.GetActiveScene().buildIndex);
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(data);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public void LoadScene()
        {
            SceneManager.LoadScene(data.GetCurstage());
        }
        #endregion DataMethods


    }
}
