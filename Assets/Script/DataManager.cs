using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class DataManager : MonoBehaviour
{
    public TextAsset data;
    private AllData datas;

    public GameObject player;

    public void Awake()
    {
        datas = JsonUtility.FromJson<AllData>(data.text);
        Debug.Log("Current Scene Index: " + SceneManager.GetActiveScene().buildIndex);
    }

    // Start is called before the first frame update
    void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        StageData currentStage = null;
        foreach (var stage in datas.stage)
        {
            if (stage.stageID == currentSceneIndex)
            {
                currentStage = stage;
                break;
            }
        }

        if (currentStage != null)
        {
            // Set player's start position for the stage
            Vector2 startPosition = new Vector2(currentStage.x, currentStage.y);
            var playerInstance = Instantiate(player);
            playerInstance.transform.position = new Vector3(startPosition.x, startPosition.y, 0);

            CinemachineVirtualCamera cinemachineCam = FindObjectOfType<CinemachineVirtualCamera>();
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

    // Update is called once per frame
    void Update()
    {

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
}
