using System;
using System.Collections.Generic;
using System.Collections;
using KartGame.Track;
using UnityEngine;
using MLAgents;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace KartGame.KartSystems
{
    /// <summary>
    /// A basic keyboard implementation of the IInput interface for all the input information a kart needs.
    /// </summary>
    public class KeyboardInput : MonoBehaviour, IInput
    {
        public float Acceleration
        {
            get { return m_Acceleration; }
        }
        public float Steering
        {
            get { return m_Steering; }
        }
        public bool BoostPressed
        {
            get { return m_BoostPressed; }
        }
        public bool FirePressed
        {
            get { return m_FirePressed; }
        }
        public bool HopPressed
        {
            get { return m_HopPressed; }
        }
        public bool HopHeld
        {
            get { return m_HopHeld; }
        }


        public float m_Acceleration;        //초기화좀 돼라 씨빨
        public float m_Steering;            //초기화좀 돼라 씨빨
        public bool m_HopPressed;
        public bool m_HopHeld;
        public bool m_BoostPressed;
        public bool m_FirePressed;

        public bool m_FixedUpdateHappened;
        
        void Update ()
        {
            if (Input.GetKey (KeyCode.UpArrow))
                m_Acceleration = 1f;
            else if (Input.GetKey (KeyCode.DownArrow))
                m_Acceleration = -1f;
            else
                m_Acceleration = 0f;

            if (Input.GetKey (KeyCode.LeftArrow) && !Input.GetKey (KeyCode.RightArrow))
                m_Steering = -1f;
            else if (!Input.GetKey (KeyCode.LeftArrow) && Input.GetKey (KeyCode.RightArrow))
                m_Steering = 1f;
            else
                m_Steering = 0f;

            m_HopHeld = Input.GetKey (KeyCode.LeftShift);

            if (m_FixedUpdateHappened)
            {
                m_FixedUpdateHappened = false;

                m_HopPressed = false;
                m_BoostPressed = false;
                m_FirePressed = false;
            }

            m_HopPressed |= Input.GetKeyDown (KeyCode.LeftShift);
           // m_BoostPressed |= Input.GetKeyDown (KeyCode.LeftShift);
            m_FirePressed |= Input.GetKeyDown (KeyCode.RightControl);
        }

        void FixedUpdate ()
        {
            m_FixedUpdateHappened = true;
        }
    }
}