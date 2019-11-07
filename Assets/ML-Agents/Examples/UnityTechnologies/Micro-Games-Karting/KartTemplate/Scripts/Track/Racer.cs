using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;
using MLAgents;
using KartGame.Track;

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
        

        Rigidbody m_Rigidbody;          //추가
        public GameObject chk1_RigidBody;
        public GameObject chk2_RigidBody;
        public GameObject chk3_RigidBody;
        public GameObject final_RigidBody;       //추가
        IControllable m_KartMovement;
        bool m_IsTimerPaused = true;
        float m_Timer = 0f;
        int m_CurrentLap = 0;
        List<float> m_LapTimes = new List<float> (9);

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

        public override void InitializeAgent()
        {
            base.InitializeAgent();
            


            SetReward(0.0f);
        }
        public override void CollectObservations()
        {
            //차의 속도와 위치 
            AddVectorObs(m_Rigidbody.position);
            AddVectorObs(m_Rigidbody.velocity.x);
            AddVectorObs(m_Rigidbody.velocity.z);

            AddVectorObs(chk1_RigidBody.transform);
            AddVectorObs(chk2_RigidBody.transform);
            AddVectorObs(chk3_RigidBody.transform);
            AddVectorObs(final_RigidBody.transform);
            
            GetReward();
        }

        public override void AgentAction(float[] vectorAction, string textAction)
        {
            //전진과 후진시에 Reward값 부여. 
            if (Input.GetKey(KeyCode.UpArrow))
                AddReward(5.0f / 300.0f);
            else if (Input.GetKey(KeyCode.DownArrow))
                AddReward(-2.0f / 300.0f);

            //회전시에도 Reward값 추가
            if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                AddReward(3.0f / 300.0f);
            else if (!Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.RightArrow))
                AddReward(3.0f / 300.0f);

            if (Input.GetKey(KeyCode.LeftShift))
                AddReward(5.0f / 300.0f);

            Debug.Log(GetReward());
        }
    }
}