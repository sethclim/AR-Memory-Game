using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using ARMG.Player;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;
using static UnityEngine.GraphicsBuffer;
using UnityEditor;
using System.Collections;

namespace ARMG
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Private Serialized Fields

        [SerializeField] GameObject m_playerPrefab;
        [SerializeField] GameObject m_stationPrefab;

        [SerializeField] LightController m_Light1;
        [SerializeField] LightController m_Light2;
        [SerializeField] LightController m_Light3;
        [SerializeField] LightController m_Light4;

        #endregion

        #region Private Fields

        private IEnumerator m_playPatternRoutine;

        List<LightController> m_lights = new();
 

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

            m_lights.Add(m_Light1);
            m_lights.Add(m_Light2);
            m_lights.Add(m_Light3);
            m_lights.Add(m_Light4);
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

        List<int> pattern = new();

        public void StartGame()
        {
            pattern = GeneratePattern();

            m_playPatternRoutine = PlayPattern(0.5f);
            StartCoroutine(m_playPatternRoutine);
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

        List<int> GeneratePattern()
        {
            List<int> pattern = new();
            for (int i = 0; i < 4; i++)
            {
                int randomNumber = UnityEngine.Random.Range(0, 2);
                pattern.Add(randomNumber);
            }

            foreach (var x in pattern)
            {
                Debug.Log(x.ToString());
            }

            return pattern;
        }

        private IEnumerator PlayPattern(float waitTime)
        {
            int index = 0;
            while (true)
            {
                m_lights[index].SwitchLight(pattern[index] == 1);
                yield return new WaitForSeconds(waitTime);
                m_lights[index].SwitchLight(false);

                print("WaitAndPrint " + Time.time);

                if(index == m_lights.Count -1)
                {
                    yield break;
                }

                index++;
            }
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
