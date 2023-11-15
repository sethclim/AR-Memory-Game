using System.Linq;
using UnityEngine;

namespace ARMG
{
    public class GameReadyUpState : BaseState<GameStateMachine.GameState>
    {
        GameStateMachine ctx;
        bool[] readyFlags;

        public GameReadyUpState(GameStateMachine.GameState _key, GameStateMachine _ctx) : base(_key)
        {
            ctx = _ctx;
        }

        public override void EnterState()
        {
            Debug.Log("Entered Ready Up State");

            readyFlags = new bool[ctx.PlayerCount];
            ctx.GTC.OnButtonPressed.AddListener(HandleButtonPress);
        }

        public override void ExitState()
        {
            Debug.Log("Exited Ready Up State");

            // Turn off all Player lights
            for (int i = 0; i < readyFlags.Length; i++)
            {
                ctx.GTC.SendSwitchPlayerLight(i, false);
            }

            ctx.SimonsPattern.Clear();
            ctx.IsPlayerEliminated = new bool[ctx.PlayerCount];
            ctx.PatternInterval = ctx.InitialPatternInterval;

            ctx.GTC.OnButtonPressed.RemoveListener(HandleButtonPress);
        }

        public override GameStateMachine.GameState GetNextState()
        {
            if (readyFlags.All(b => b))
            {
                // Everyone is ready!
                return GameStateMachine.GameState.SimonTurn;
            }

            return GameStateMachine.GameState.ReadyUp;
        }

        public override void UpdateState() { }

        void HandleButtonPress(int playerIndex, int buttonIndex)
        {
            if (playerIndex >= 0 && playerIndex < readyFlags.Length && !readyFlags[playerIndex])
            {
                readyFlags[playerIndex] = true;
                ctx.GTC.SendSwitchPlayerLight(playerIndex, true);
            }
        }
    }
}