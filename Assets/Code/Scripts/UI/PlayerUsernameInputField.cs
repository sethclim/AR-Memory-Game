using UnityEngine;
using TMPro;
using Photon.Pun;

namespace ARMG.UI
{
    [RequireComponent(typeof(TMP_InputField))]
    public class PlayerUsernameInputField : MonoBehaviour
    {
        #region Private Constants

        const string m_playerUsernamePrefKey = "PlayerUsername";

        #endregion

        #region MonoBehaviour Callbacks

        void Start()
        {
            string defaultName = string.Empty;
            TMP_InputField _inputField = GetComponent<TMP_InputField>();
            if (_inputField && PlayerPrefs.HasKey(m_playerUsernamePrefKey))
            {
                defaultName = PlayerPrefs.GetString(m_playerUsernamePrefKey);
                _inputField.text = defaultName;
            }

            PhotonNetwork.NickName = defaultName;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the name of the player, and save it in the PlayerPrefs for future sessions.
        /// </summary>
        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty"); return;
            }

            PhotonNetwork.NickName = value;
            PlayerPrefs.SetString(m_playerUsernamePrefKey, value);
        }

        #endregion
    }
}
