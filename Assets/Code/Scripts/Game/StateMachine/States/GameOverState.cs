using System.Collections;
using System.Linq;
using UnityEngine;

namespace ARMG
{
    public class GameOverState : BaseState<GameStateMachine.GameState>
    {
        GameStateMachine ctx;

        bool shouldReset;

        public GameOverState(GameStateMachine.GameState _key, GameStateMachine _ctx) : base(_key)
        {
            ctx = _ctx;
        }

        public override void EnterState()
        {
            Debug.Log("Entered Game Over State");

            shouldReset = false;

            // Reset lights
            for (int i = 0; i < 4; i++)
            {
                ctx.GTC.SendSwitchPatternLight(i, false);
                ctx.GTC.SendSwitchPlayerLight(i, false);
            }

            for (int i = 0; i < ctx.IsPlayerEliminated.Length; i++)
            {
                if (!ctx.IsPlayerEliminated[i])
                {
                    // WINNER
                    ctx.StartCoroutine(CelebrateWinner(i));
                    return;
                }
            }

            // NO WINNER
            ctx.StartCoroutine(NoWinner());
        }

        public override void ExitState()
        {
            Debug.Log("Exited Game Over State");
        }

        public override GameStateMachine.GameState GetNextState()
        {
            if (shouldReset)
            {
                return GameStateMachine.GameState.ReadyUp;

            }

            return GameStateMachine.GameState.GameOver;
        }

        public override void UpdateState() { }

        float pulseDelay = 0.5f;
        IEnumerator CelebrateWinner(int winnerIndex)
        {
            ctx.GTC.SendSwitchPlayerLight(winnerIndex, true);

            // Pulse 3 times
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    ctx.GTC.SendSwitchPatternLight(j, true);
                }
                yield return new WaitForSeconds(pulseDelay);
                for (int j = 0; j < 4; j++)
                {
                    ctx.GTC.SendSwitchPatternLight(j, false);
                }
                yield return new WaitForSeconds(pulseDelay);
            }

            shouldReset = true;
        }

        IEnumerator NoWinner()
        {
            for (int j = 0; j < 4; j++)
            {
                ctx.GTC.SendSwitchPatternLight(j, true);
            }
            yield return new WaitForSeconds(pulseDelay * 3);

            shouldReset = true;
        }
    }
}
