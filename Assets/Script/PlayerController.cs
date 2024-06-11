using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    RotatePlayer(1f);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    RotatePlayer(-1f);
                }
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    RespondToBoost(true);
                }
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
        #endregion PlayerMethods
    }

}