using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Database;
using TMPro;

public class UIManager : MonoBehaviour
{
    private bool isGamePaused = false;
    private bool isloadwindow = false;
    private DataManager db;
    public GameObject loadinfoWindow;
    public GameObject pauseWindow;
    [SerializeField] TextMeshProUGUI stageText;
    private RectTransform loadinfoWindowRect;
    private int previousScreenWidth;
    private int previousScreenHeight;
    public DataBase data;
    void Start()
    {
        db = DataManager.instance;
        loadinfoWindowRect = loadinfoWindow.GetComponent<RectTransform>();

    
        previousScreenWidth = Screen.width;
        previousScreenHeight = Screen.height;

    
        UpdateUI();
    }

    void Update()
    {
   
        if (Screen.width != previousScreenWidth || Screen.height != previousScreenHeight)
        {
            UpdateUI();
            previousScreenWidth = Screen.width;
            previousScreenHeight = Screen.height;
        }
    }
    #region EventMethods
    public void OnNewGameStart()
    {
        isGamePaused = false;
        SceneManager.LoadScene(1);
    }

    public void OnLoadInfoWindow()
    {
        stageText.text = "Stage" + data.GetCurstage();
        isloadwindow = !isloadwindow;
        loadinfoWindow.SetActive(isloadwindow);
    }

    public void OnLoadStage()
    {
        db.LoadScene();
    }

    public void OnSave()
    {
        db.SaveData();
    }

    public void OnPause()
    {
        isGamePaused = !isGamePaused;
        pauseWindow.SetActive(isGamePaused);
        Time.timeScale = isGamePaused ? 0f : 1f;
    }
    public void OnLoadLobby()
    {
        SceneManager.LoadScene(0);
    }
    public void OnExit()
    {
        Application.Quit();
    }

    private void UpdateUI()
    {
        if (loadinfoWindowRect != null)
        {
            loadinfoWindowRect.anchoredPosition = new Vector2(Screen.width / 2, Screen.height / 2);
            loadinfoWindowRect.sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);
        }
    }
    #endregion EventMethods
}
