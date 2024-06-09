using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{

    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float torqueAmount = 1f;
        [SerializeField] float boostSpeed = 30f;
        [SerializeField] float baseSpeed = 20f;

        private Rigidbody2D rb;
        SurfaceEffector2D surfaceEffector2D;

        bool canMove = true;

       
        // Start is called before the first frame update
        void Start()
        {
           
            rb = GetComponent<Rigidbody2D>();
            surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (canMove)
            {
             
            }
            else
            {
            }
        }

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
             rb.AddTorque(reverse*torqueAmount);
        }

        public void DisableControls()
        {
            canMove = false;
        }
    }

}