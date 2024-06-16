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
    public GameObject authWindow;
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
        SceneManager.LoadScene(2);
    }

    public void OnLoadInfoWindow()
    {
        Debug.Log(StageObject.Instance.GetSavedStage());
        stageText.text = "Stage" + (StageObject.Instance.GetSavedStage());
        isloadwindow = !isloadwindow;
        loadinfoWindow.SetActive(isloadwindow);
    }
    public void OnExitAuthWindow()
    {
        authWindow.SetActive(false);
    }
    public void OnEnterAuthWindow()
    {
        authWindow.SetActive(true);
    }
    public void OnLoadStage()
    {
        Debug.Log(StageObject.Instance.GetSavedStage() + 1);
        SceneManager.LoadScene(StageObject.Instance.GetSavedStage()+1);
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
        SceneManager.LoadScene(1);
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
