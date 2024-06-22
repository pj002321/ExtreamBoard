using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [SerializeField] float torqueAmount = 1f;
        [SerializeField] float boostSpeed = 30f;
        [SerializeField] float baseSpeed = 20f;

        private Rigidbody2D rb;
        SurfaceEffector2D surfaceEffector2D;
        public AudioSource getCoinSound;
        bool canMove = true;
        #endregion Variables

        #region UnityMethods
        void Start()
        {

            rb = GetComponent<Rigidbody2D>();
            surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
       
        }
        void Update()
        {
            if (canMove)
            {
            }
            else
            {
                DisableControls();
            }
        }
        #endregion UnityMethods

        #region PlayerMethods
        public void RespondToBoost(bool state)
        {
            if (state)
            {
                surfaceEffector2D.speed = boostSpeed;
            }
            else
            {
                surfaceEffector2D.speed = baseSpeed;
            }
        }

        public void RotatePlayer(float reverse)
        {
            rb.AddTorque(reverse * torqueAmount);
        }

        public void DisableControls()
        {
            canMove = false;
        }

        int getCoinIdex = 0;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Coin")
            {
                PlayerDataHandler.Instance.UpdateStageCoinData(SceneManager.GetActiveScene().buildIndex, getCoinIdex++);
                GameUI.Instance.SetText(getCoinIdex);
                getCoinSound.Play();
            }

        }  
        #endregion PlayerMethods
    }

}