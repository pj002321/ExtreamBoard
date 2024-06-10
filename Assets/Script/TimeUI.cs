using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace Timer
{
    public class TimeUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI timerText;
        [SerializeField] TextMeshProUGUI gameoverText;
        public static TimeUI instance;
        public static float elapsedTime = 25f; // 타이머를 20초로 초기화
        int minutes;
        int seconds;

        private void Awake()
        {
            resetTime();
        }

        public void resetTime()
        {
            elapsedTime =25f; 
        }

        private void Update()
        {
            if (elapsedTime <= 0)
            {
                Debug.Log("TimeOver");
                timerText.text = string.Format("{0:00}:{0:00}", 0, 0);
                gameoverText.gameObject.SetActive(true);
                elapsedTime = 0;
                GetComponent<AudioSource>().Play();
                Invoke("LoadOpeningScene", 2f);
            }
            else
            {
                gameoverText.gameObject.SetActive(false);
                elapsedTime -= Time.deltaTime;
                minutes = Mathf.FloorToInt(elapsedTime / 60);
                seconds = Mathf.FloorToInt(elapsedTime % 60);
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }

        void LoadOpeningScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}
