using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Database;
using TMPro;

public class UIManager : MonoBehaviour
{
    private bool isloadwindow = false;

    public GameObject loadinfoWindow;
    public GameObject authWindow;
    [SerializeField] TextMeshProUGUI stageText;
    private RectTransform Rect;
    private int previousScreenWidth;
    private int previousScreenHeight;
  
    void Start()
    {

        Rect = GetComponent<RectTransform>();
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
        if(SceneManager.GetActiveScene().buildIndex==1)
            stageText.text = "Stage" + StageObject.Instance.GetSavedStage();
    }
    #region EventMethods
    public void OnNewGameStart()
    {
      
        SceneManager.LoadScene(2);
    }

    public void OnLoadInfoWindow()
    {
        isloadwindow = !isloadwindow;
        loadinfoWindow.SetActive(isloadwindow);

        StartCoroutine(DelayedUpdateStageText());
    }

    IEnumerator DelayedUpdateStageText()
    {
        yield return new WaitForSeconds(1f);
        stageText.text = "Stage" + StageObject.Instance.GetSavedStage();
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
        SceneManager.LoadScene(StageObject.Instance.GetSavedStage() + 1);
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
    #endregion EventMethods
}
