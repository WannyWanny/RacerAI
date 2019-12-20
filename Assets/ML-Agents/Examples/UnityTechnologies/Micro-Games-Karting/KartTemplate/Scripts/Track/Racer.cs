using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;
using MLAgents;

namespace KartGame.Track
{
    /// <summary>
    /// The default implementation of the IRacer interface.  This is a representation of all the timing information for a particular kart as it goes through a race.
    /// </summary>
    public class Racer : Agent, IRacer, IControllable
    {
        [Tooltip ("A reference to the IControllable for the kart.  Normally this is the KartMovement script.")]
        
        [RequireInterface (typeof(IControllable))]
        public Object kartMovement;
        public Object input;
        
        private Rigidbody rigid_kart;

        KartMovement kMove;
        KeyboardInput kInput;
        IControllable m_KartMovement;
        bool m_IsTimerPaused = true;
        float m_Timer = 0f;
        int m_CurrentLap = 0;
        List<float> m_LapTimes = new List<float> (9);

        public float kMovement = 0f;
        public float kSteer = 0f;
        bool m_HopPressed;
        bool m_HopHeld;
        bool m_BoostPressed;
        bool m_FirePressed;

        void Awake ()
        {
            m_KartMovement = kartMovement as IControllable;
        }

        void Update ()
        {   
            if (m_CurrentLap > 0 && !m_IsTimerPaused)
                m_Timer += Time.deltaTime;
        }

        public void PauseTimer ()
        {
            m_IsTimerPaused = true;
        }

        public void UnpauseTimer ()
        {
            m_IsTimerPaused = false;
        }

        public void HitStartFinishLine ()
        {
            AgentReset();
            if (m_CurrentLap > 0)
            {
                m_LapTimes.Add (m_Timer);
                m_Timer = 0f;
                AddReward(50f);
            }       
            m_CurrentLap++;           
        }

        public int GetCurrentLap ()
        {
            return m_CurrentLap;
        }

        public List<float> GetLapTimes ()
        {
            return m_LapTimes;
        }

        public float GetLapTime ()
        {
            return m_Timer;
        }

        public float GetRaceTime ()
        {
            float raceTime = m_Timer;
            for (int i = 0; i < m_LapTimes.Count; i++)
            {
                raceTime += m_LapTimes[i];
            }

            return raceTime;
        }

        public void EnableControl ()
        {
            m_KartMovement.EnableControl ();
        }

        public void DisableControl ()
        {
            m_KartMovement.DisableControl ();
        }

        public bool IsControlled ()
        {
            return m_KartMovement.IsControlled ();
        }

        public string GetName ()
        {
            return name;
        }

        public float getAccel
        {
            get { return kMovement; }
        }

        public float getSteer
        {
            get { return kSteer; }
        }

        public override void InitializeAgent()
        {
            rigid_kart = GetComponent<Rigidbody>();
            kMovement = Random.Range(-1.0f, 1.0f);
            kSteer = Random.Range(-0.3f, 0.1f);
            m_HopPressed = false;
            m_HopHeld = false;
        }

        public override void CollectObservations()
        {
            AddVectorObs(rigid_kart.transform);
            AddVectorObs(rigid_kart.velocity.x);
            AddVectorObs(rigid_kart.velocity.z);
        }
        public override void AgentAction(float[] vectorAction, string textAction)
        {
            AddReward(-1f/3000f);
            MoveAgent(vectorAction);
        }
        public void MoveAgent(float[] act)
        {
            var action = Mathf.FloorToInt(act[0]);
            switch(action)
            {
                case 1:         //직진
                    AddReward(0.1f);
                    break;
                case 2:         //좌회전
                    AddReward(1.0f);
                    break;
            }
        }
        public override void AgentReset()
        {
            InitializeAgent();
            rigid_kart.transform.localPosition = new Vector3(-20.63f, 0.8f, -43.13f);
            rigid_kart.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            kMovement = Random.Range(-1.0f, 1.0f);
            Debug.Log("가속도 값:" + kMovement);
            kSteer = Random.Range(-0.3f, 0.1f);
            Debug.Log("회전 각도:" + kSteer);
            AddReward(50.0f);
        }

        public override void AgentOnDone()
        {
            AddReward(5.0f);                //골 지점에 도달 했을 시에 리워즈값 추가
            Done();
        }       
    }
}
