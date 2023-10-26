using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using Photon.Realtime;

namespace ARMG.Player
{
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody))]
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region Public Fields

        public static GameObject LocalPlayerInstance;

        #endregion

        #region Private Fields

        PlayerInput m_playerInput;
        Rigidbody m_rigidBody;
        bool m_jumpPressed;

        #endregion

        #region MonoBehaviour Callbacks

        void Awake()
        {
            m_playerInput = GetComponent<PlayerInput>();
            m_rigidBody = GetComponent<Rigidbody>();

            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = gameObject;
            }

            // Disable user input for clones
            m_playerInput.enabled = photonView.IsMine;

            DontDestroyOnLoad(gameObject);
        }

        void FixedUpdate()
        {
            if (m_jumpPressed)
            {
                m_jumpPressed = false;

                m_rigidBody.AddForce(Vector3.up * 5.0f, ForceMode.VelocityChange);
            }
        }

        #endregion

        #region IPunObservable implementation

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(m_jumpPressed);
            }
            else
            {
                m_jumpPressed = (bool)stream.ReceiveNext();
            }
        }

        #endregion

        #region Public Methods

        public void OnInteract(InputValue value)
        {
            m_jumpPressed = true;
        }

        #endregion
    }
}
