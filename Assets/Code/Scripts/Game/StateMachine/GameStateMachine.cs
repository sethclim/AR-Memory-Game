using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ARMG
{
    [RequireComponent(typeof(GameTableController), typeof(AudioSource))]
    public class GameStateMachine : StateMachine<GameStateMachine.GameState>
    {
        public enum GameState
        {
            ReadyUp,
            SimonTurn,
            PlayerTurn,
            GameOver
        }

        [Header("Network Settings")]
        [Range(1, 4)] public int PlayerCount = 1; // TODO: Update this via network

        [Header("Game Sounds")]
        [SerializeField] AudioClip[] m_audioClips;

        [SerializeField] public SpinnerController SpinnerController;

        public GameTableController GTC { get => m_gtc; }
        public List<int> SimonsPattern { get; } = new();
        public float InitialPatternInterval { get; } = 1.0f; // Seconds
        public float PatternInterval { get; set; } // Seconds
        public bool[] IsPlayerEliminated { get; set; }

        GameTableController m_gtc;
        AudioSource m_audioSource;


        public void PlaySound(int index)
        {
            if (index >= 0 && index < m_audioClips.Length)
            {
                m_audioSource.clip = m_audioClips[index];
                m_audioSource.Play();
            }
        }

        void Awake()
        {
            m_gtc = GetComponent<GameTableController>();
            m_audioSource = GetComponent<AudioSource>();

            States[GameState.ReadyUp] = new GameReadyUpState(GameState.ReadyUp, this);
            States[GameState.SimonTurn] = new GameSimonTurnState(GameState.SimonTurn, this);
            States[GameState.PlayerTurn] = new GamePlayerTurnState(GameState.PlayerTurn, this);
            States[GameState.GameOver] = new GameOverState(GameState.GameOver, this);

            CurrentState = States[GameState.ReadyUp];
        }

        public override void Start()
        {
            base.Start();
            // In case we started with the wrong scene being active, simply load the menu scene
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene(0);
                return;
            }
        }

        public override void TransitionToState(GameState statekey)
        {
            photonView.RPC(nameof(RPC_TransitionState), RpcTarget.AllBuffered, statekey);
        }

        [PunRPC]
        void RPC_TransitionState(GameState statekey)
        {
            CurrentState.ExitState();
            CurrentState = States[statekey];
            CurrentState.EnterState();
        }
    }
}
