using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Database;

public class UIManager : MonoBehaviour
{
    private bool isGamePaused = false;
    private bool isloadwindow = false;
    private DataManager db;
    public GameObject loadinfoWindow;

    void Start()
    {
        db = DataManager.instance;
    }
    public void OnNewGameStart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnLoadInfoWindow()
    {
        db.LoadData();
        isloadwindow = !isloadwindow;
        loadinfoWindow.SetActive(isloadwindow ? true : false);
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
        Time.timeScale = isGamePaused ? 0f : 1f;
    }
    public void OnExit()
    {
        Application.Quit();
    }

}
