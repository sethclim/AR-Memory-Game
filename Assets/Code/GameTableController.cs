using Photon.Pun;
using UnityEngine;

namespace ARMG
{
    [RequireComponent(typeof(PhotonView))]
    public class GameTableController : MonoBehaviourPunCallbacks
    {
        public static GameTableController instance;
        [SerializeField] LightController[] m_lightControllers;

        void Awake()
        {
            instance = this;
        }

        public void OnLightStateChanged(LightController lightController)
        {
            int lightBulbIndex = System.Array.IndexOf(m_lightControllers, lightController);

            if (lightBulbIndex != -1)
            {
                photonView.RPC("UpdateLightController", RpcTarget.AllBuffered, lightBulbIndex, lightController.GetState());
            }
        }

        [PunRPC]
        void UpdateLightController(int lightBulbIndex, bool newValue)
        {
            m_lightControllers[lightBulbIndex].SwitchLightLocally(newValue);
        }
    }
}
