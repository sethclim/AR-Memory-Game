using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ARMG
{
    [RequireComponent(typeof(PhotonView))]
    public class GameTableController : MonoBehaviourPunCallbacks
    {
        [HideInInspector] public UnityEvent<int, int> OnButtonPressed = new UnityEvent<int, int>();
        [SerializeField] ButtonController[] m_buttonControllers;
        [SerializeField] LightController[] m_patternLights;
        [SerializeField] LightController[] m_playerLights;
        [SerializeField] SpinnerController m_spinnerController;

        public override void OnEnable()
        {
            base.OnEnable();
            for (int i = 0; i < m_buttonControllers.Length; i++)
            {
                int playerIndex = i;
                m_buttonControllers[i].OnButtonPressed.AddListener(
                    (buttonIndex) => HandleButtonPressed(playerIndex, buttonIndex));
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();
            for (int i = 0; i < m_buttonControllers.Length; i++)
            {
                // Using RemoveAllListeners incase RemoveListener is unable
                // to properly remove an arrow function
                m_buttonControllers[i].OnButtonPressed.RemoveAllListeners();
            }
        }

        void HandleButtonPressed(int playerIndex, int buttonIndex)
        {
            photonView.RPC(nameof(RPC_HandleButtonPressed), RpcTarget.AllBuffered, playerIndex, buttonIndex);
        }

        [PunRPC]
        void RPC_HandleButtonPressed(int playerIndex, int buttonIndex)
        {
            OnButtonPressed.Invoke(playerIndex, buttonIndex);
        }

        public void SendSwitchPatternLight(int lightBulbIndex, bool onOff)
        {
            if (lightBulbIndex >= 0 && lightBulbIndex < m_patternLights.Length)
            {
                photonView.RPC(nameof(RPC_UpdateLightController), RpcTarget.AllBuffered, lightBulbIndex, onOff);
            }
        }

        [PunRPC]
        void RPC_UpdateLightController(int lightBulbIndex, bool newValue)
        {
            if (lightBulbIndex >= 0 && lightBulbIndex < m_patternLights.Length)
            {
                m_patternLights[lightBulbIndex].ToggleLight(newValue);
            }
        }

        public void SendSwitchPlayerLight(int lightBulbIndex, bool onOff)
        {
            if (lightBulbIndex >= 0 && lightBulbIndex < m_playerLights.Length)
            {
                photonView.RPC(nameof(RPC_SwitchPlayerLight), RpcTarget.AllBuffered, lightBulbIndex, onOff);
            }
        }

        [PunRPC]
        void RPC_SwitchPlayerLight(int lightBulbIndex, bool newValue)
        {
            if (lightBulbIndex >= 0 && lightBulbIndex < m_playerLights.Length)
            {
                m_playerLights[lightBulbIndex].ToggleLight(newValue);
            }
        }
    }
}
