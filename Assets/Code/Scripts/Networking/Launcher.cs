using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace ARMG.Networking
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        /// <summary>
        /// The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created.
        /// </summary>
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
        [SerializeField] byte m_maxPlayersPerRoom = 4;

        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField] GameObject m_controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField] GameObject m_progressLabel;

        #endregion

        #region Private Fields

        /// <summary>
        /// This client's version number. Users are separated from each other by gameVersion (which allows you to make breaking changes).
        /// </summary>
        readonly string m_gameVersion = "1";

        /// <summary>
        /// Keep track of the current process. Since connection is asynchronous and is based on several callbacks from Photon,
        /// we need to keep track of this to properly adjust the behavior when we receive call back by Photon.
        /// Typically this is used for the OnConnectedToMaster() callback.
        /// </summary>
        bool m_isConnecting;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            // #Critical
            // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Start()
        {
            m_progressLabel.SetActive(false);
            m_controlPanel.SetActive(true);
        }

        #endregion

        #region Public Methods

        public void Connect()
        {
            m_progressLabel.SetActive(true);
            m_controlPanel.SetActive(false);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                m_isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = m_gameVersion;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            if (m_isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
                m_isConnecting = false;
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            m_progressLabel.SetActive(false);
            m_controlPanel.SetActive(true);

            m_isConnecting = false;
            Debug.LogWarningFormat("[Client]: Disconnected with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("[Client]: No random room available, creating one...");

            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = m_maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("[Client]: Successfully joined room.");
            PhotonNetwork.LoadLevel("Room for 1");
        }

        #endregion
    }
}
