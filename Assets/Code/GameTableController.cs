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

        readonly List<(int light, int onOff)> m_pattern = new();
        private IEnumerator m_playPatternRoutine;


        void Awake()
        {
            instance = this;
        }

        public void PlayPattern()
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

        private IEnumerator PlayPattern(float waitTime)
        {
            int index = 0;
            while (true)
            {
                RPC_LightSwitch(m_pattern[index].light, m_pattern[index].onOff == 1);
                yield return new WaitForSeconds(0.4f);
                RPC_LightSwitch(m_pattern[index].light, false);
                yield return new WaitForSeconds(waitTime);

                index++;

                if (index == m_pattern.Count)
                {
                    yield break;
                }
            }
        }

        public void RPC_LightSwitch(int lightBulbIndex, bool onOff)
        {
            if (lightBulbIndex != -1)
            {
                photonView.RPC(nameof(UpdateLightController), RpcTarget.AllBuffered, lightBulbIndex, onOff);
            }
        }

        [PunRPC]
        void UpdateLightController(int lightBulbIndex, bool newValue)
        {
            m_patternLights[lightBulbIndex].SwitchLight(newValue);
        }
    }
}
