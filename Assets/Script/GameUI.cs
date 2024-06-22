using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    private static GameUI instance;
    public static GameUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameUI>();
            }
            return instance;
        }
    }

    [SerializeField] TextMeshProUGUI coinText;
    public GameObject pauseWindow;
    private bool isGamePaused = false;
    private int previousScreenWidth;
    private int previousScreenHeight;
    private RectTransform Rect;

    private void Awake()
    {
        coinText.gameObject.SetActive(false);
        isGamePaused = false;
        coinText.text = "";
        Rect = GetComponent<RectTransform>();
        previousScreenWidth = Screen.width;
        previousScreenHeight = Screen.height;
        UpdateUI();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        coinText.gameObject.SetActive(false);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        coinText.text = "0";
    }

    void Update()
    {
       
    }

    public void OnPause()
    {
        isGamePaused = !isGamePaused;
        pauseWindow.SetActive(isGamePaused);
        Time.timeScale = isGamePaused ? 0f : 1f;
    }
    public void SetText(int coinindex)
    {
        coinText.gameObject.SetActive(true);
        coinText.text=coinindex.ToString();
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
        if (Rect != null)
        {
            Rect.anchoredPosition = new Vector2(Screen.width / 2, Screen.height / 2);
            Rect.sizeDelta = new Vector2(Screen.width / 2, Screen.height / 2);
        }
    }
}
