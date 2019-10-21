using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;
using MLAgents;

namespace KartGame.Track
{
    /// <summary>
    /// The default implementation of the IRacer interface.  This is a representation of all the timing information for a particular kart as it goes through a race.
    /// </summary>
    public class Racer : Agent, IRacer
    {
        [Tooltip ("A reference to the IControllable for the kart.  Normally this is the KartMovement script.")]
        [RequireInterface (typeof(IControllable))]
        public Object kartMovement;
        private Object Goal;
        private RacerAcademy m_MyAcademy;           //아카데미 변수
        IControllable m_KartMovement;
        bool m_IsTimerPaused = true;
        float m_Timer = 0f;
        int m_CurrentLap = 0;
        List<float> m_LapTimes = new List<float> (9);

        void Awake ()
        {
            m_KartMovement = kartMovement as IControllable;
        }

        public override void InitializeAgent()              //초기화 하래서 일단 작성은 해보는중
        {
            m_MyAcademy = GameObject.Find("Academy").GetComponent<RacerAcademy>();
        }
        public override void CollectObservations()          //이 함수를 잘 작성하는 것이 속도에 큰 영향을 줄듯
        {
            AddVectorObs(Goal);
            AddVectorObs(gameObject.transform.rotation.x);          
            AddVectorObs(gameObject.transform.rotation.z);
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
            if (m_CurrentLap > 0)
            {
                m_LapTimes.Add (m_Timer);
                m_Timer = 0f;
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
    }
}