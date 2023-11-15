using System.Collections;
using System.Linq;
using UnityEngine;

namespace ARMG
{
    public class GamePlayerTurnState : BaseState<GameStateMachine.GameState>
    {
        GameStateMachine ctx;
        bool allTurnsCompleted;
        bool isProcessingInput;
        int currPlayerTurn;
        int currPatternIndex;

        public GamePlayerTurnState(GameStateMachine.GameState _key, GameStateMachine _ctx) : base(_key)
        {
            ctx = _ctx;
        }

        public override void EnterState()
        {
            Debug.Log("Entered Player Turn State");

            allTurnsCompleted = false;
            isProcessingInput = true;

            currPlayerTurn = -1;
            NextPlayer();

            ctx.GTC.OnButtonPressed.AddListener(HandleButtonPress);
        }

        public override void ExitState()
        {
            Debug.Log("Exited Player Turn State");

            ctx.GTC.OnButtonPressed.RemoveListener(HandleButtonPress);
        }

        public override GameStateMachine.GameState GetNextState()
        {
            if (allTurnsCompleted && isProcessingInput)
            {
                bool allPlayersEliminated = ctx.IsPlayerEliminated.All(b => b);
                bool onePlayerRemaining = ctx.IsPlayerEliminated.Length > 1 && ctx.IsPlayerEliminated.Count(b => !b) == 1;

                if (allPlayersEliminated || onePlayerRemaining)
                {
                    return GameStateMachine.GameState.GameOver;
                }

                return GameStateMachine.GameState.SimonTurn;
            }

            return GameStateMachine.GameState.PlayerTurn;
        }

        public override void UpdateState() { }

        void HandleButtonPress(int playerIndex, int buttonIndex)
        {
            if (!isProcessingInput) return;

            bool validPlayerIndex = playerIndex >= 0 && playerIndex < ctx.PlayerCount;
            bool validButtonIndex = buttonIndex >= 0 && buttonIndex < 4;
            bool validPlayer = playerIndex == currPlayerTurn;

            if (!validPlayerIndex || !validButtonIndex || !validPlayer) return;

            // Correct button pressed
            if (buttonIndex == ctx.SimonsPattern[currPatternIndex])
            {
                currPatternIndex++;
                if (currPatternIndex == ctx.SimonsPattern.Count)
                {
                    // TODO: Set all lights to green
                    NextPlayer();
                }

                ctx.StartCoroutine(PulsePatternLight(buttonIndex));
            }
            // Incorrect button pressed
            else
            {
                // TODO: Set all lights to red
                ctx.IsPlayerEliminated[playerIndex] = true;
                NextPlayer();
            }
        }

        IEnumerator PulsePatternLight(int index, float waitTime = 0.75f)
        {
            isProcessingInput = false;

            ctx.GTC.SendSwitchPatternLight(index, true);
            yield return new WaitForSeconds(waitTime);
            ctx.GTC.SendSwitchPatternLight(index, false);

            isProcessingInput = true;
        }

        void NextPlayer()
        {
            bool foundNextPlayer = false;

            for (int i = currPlayerTurn + 1; i < ctx.PlayerCount; i++)
            {
                if (!ctx.IsPlayerEliminated[i])
                {
                    currPlayerTurn = i;
                    currPatternIndex = 0;
                    foundNextPlayer = true;
                    // TODO: Rotate Lazy Susan
                    break;
                }
            }

            if (!foundNextPlayer)
            {
                allTurnsCompleted = true;
            }
        }
    }
}