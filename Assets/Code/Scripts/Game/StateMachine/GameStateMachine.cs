using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace ARMG
{
    [RequireComponent(typeof(GameTableController))]
    public class GameStateMachine : StateMachine<GameStateMachine.GameState>
    {
        public enum GameState
        {
            ReadyUp,
            SimonTurn,
            PlayerTurn,
            GameOver
        }

        public GameTableController GTC { get => m_gtc; }
        public List<int> SimonsPattern { get; } = new();
        public float InitialPatternInterval { get; } = 1.0f; // Seconds
        public float PatternInterval { get; set; } // Seconds
        public bool[] IsPlayerEliminated { get; set; }
        [Range(1, 4)] public int PlayerCount = 1; // TODO: Update this via network

        GameTableController m_gtc;

        void Awake()
        {
            m_gtc = GetComponent<GameTableController>();

            States[GameState.ReadyUp] = new GameReadyUpState(GameState.ReadyUp, this);
            States[GameState.SimonTurn] = new GameSimonTurnState(GameState.SimonTurn, this);
            States[GameState.PlayerTurn] = new GamePlayerTurnState(GameState.PlayerTurn, this);
            States[GameState.GameOver] = new GameOverState(GameState.GameOver, this);

            CurrentState = States[GameState.ReadyUp];
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
