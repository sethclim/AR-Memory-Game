using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using ARMG.Player;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;

namespace ARMG
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Private Serialized Fields

        [SerializeField] GameObject m_playerPrefab;
        [SerializeField] GameObject m_stationPrefab;

        #endregion

        #region MonoBehaviour CallBacks

        void Start()
        {
            // In case we started with the wrong scene being active, simply load the menu scene
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene(0);
                return;
            }

            if (PhotonNetwork.InRoom && GameTableController.instance == null && PhotonNetwork.IsMasterClient)
            {
                Debug.Log("We are Instantiating LocalPlayer");
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                // PhotonNetwork.Instantiate(m_playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                PhotonNetwork.Instantiate(m_stationPrefab.name, Vector3.zero, Quaternion.identity, 0);
            }
        }

        #endregion

        #region Photon Callbacks

        public override void OnJoinedRoom()
        {
            // Note: it is possible that this monobehaviour is not created (or active) when OnJoinedRoom happens
            // due to that the Start() method also checks if the local player character was network instantiated!
            // if (PlayerManager.LocalPlayerInstance == null)
            // {
            //     Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

            //     // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            //     PhotonNetwork.Instantiate(m_playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            // }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        }

        #endregion

        #region Public Methods

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void StartGame()
        {
            GeneratePattern();
        }



        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        void GeneratePattern()
        {
            List<int> pattern = new();
            for (int i = 0; i < 4; i++)
            {
                int randomNumber = UnityEngine.Random.Range(0, 1);
                pattern.Add(randomNumber);
            }

            Debug.Log(pattern);
        }

        #endregion
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameManager))]
    public class GameManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GameManager gm = (GameManager)target;
            if (GUILayout.Button("Start Game"))
            {
                gm.StartGame();
            }
        }
    }
#endif
}
