using Mono.Reflection;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARMG
{
    [RequireComponent(typeof(PhotonView))]
    public class GameTableController : MonoBehaviourPunCallbacks
    {
        public static GameTableController instance;
        [SerializeField] LightController[] m_patternLights;
        [SerializeField] LightController[] m_turnLights;
        [SerializeField] SpinnerController m_spinnerController;

        readonly List<(int light, int onOff)> m_pattern = new();
        private IEnumerator m_playPatternRoutine;
        int prevTurnID = 1;


        void Awake()
        {
            instance = this;
        }

        public void TriggerPlayPattern()
        {
            m_playPatternRoutine = PlayPattern(0.7f);
            StartCoroutine(m_playPatternRoutine);
        }

        public void GeneratePattern()
        {
            int lightIndex = Random.Range(0, 4);

            (int light, int onOff) instruction = (lightIndex, 1);

            m_pattern.Add(instruction);
        }

        public void DisplayNextTurn(int nextTurnID)
        {
            m_spinnerController.Spin();
            SendSwitchPlayerLight(prevTurnID, false);
            SendSwitchPlayerLight(nextTurnID, true);
            prevTurnID = nextTurnID;
        }

        private IEnumerator PlayPattern(float waitTime)
        {
            int index = 0;
            while (true)
            {
                SendSwitchPatternLight(m_pattern[index].light, m_pattern[index].onOff == 1);
                yield return new WaitForSeconds(0.4f);
                SendSwitchPatternLight(m_pattern[index].light, false);
                yield return new WaitForSeconds(waitTime);

                index++;

                if (index == m_pattern.Count)
                {
                    yield break;
                }
            }
        }

        public void SendSwitchPatternLight(int lightBulbIndex, bool onOff)
        {
            if (lightBulbIndex != -1)
            {
                photonView.RPC(nameof(RPC_UpdateLightController), RpcTarget.AllBuffered, lightBulbIndex, onOff);
            }
        }

        [PunRPC]
        void RPC_UpdateLightController(int lightBulbIndex, bool newValue)
        {
            m_patternLights[lightBulbIndex].SwitchLight(newValue);
        }

        public void SendSwitchPlayerLight(int lightBulbIndex, bool onOff)
        {
            if (lightBulbIndex != -1)
            {
                photonView.RPC(nameof(RPC_SwitchPlayerLight), RpcTarget.AllBuffered, lightBulbIndex, onOff);
            }
        }

        [PunRPC]
        void RPC_SwitchPlayerLight(int lightBulbIndex, bool newValue)
        {
            m_patternLights[lightBulbIndex].SwitchLight(newValue);
        }
    }
}
