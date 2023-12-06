using System.Collections;
using UnityEngine;

namespace ARMG
{
    public class GameSimonTurnState : BaseState<GameStateMachine.GameState>
    {
        GameStateMachine ctx;
        bool isFinishedPattern = false;

        public GameSimonTurnState(GameStateMachine.GameState _key, GameStateMachine _ctx) : base(_key)
        {
            ctx = _ctx;
        }

        public override void EnterState()
        {
            Debug.Log("Entered Simon Turn State");

            isFinishedPattern = false;

            // Reset player turn lights
            for (int i = 0; i < 4; i++)
            {
                ctx.GTC.SendSwitchPlayerLight(i, false);
            }

            // Generate Pattern
            int lightIndex = Random.Range(0, 4);
            ctx.SimonsPattern.Add(lightIndex);

            // Play Pattern
            ctx.StartCoroutine(PlayPattern(ctx.PatternInterval));
        }

        public override void ExitState()
        {
            Debug.Log("Exited Simon Turn State");

            // Increase speed by 10% to a minimum of 0.25 sec
            float percentOfInitial = ctx.InitialPatternInterval * 0.1f;
            ctx.PatternInterval = Mathf.Max(0.25f, ctx.PatternInterval - percentOfInitial);
        }

        public override GameStateMachine.GameState GetNextState()
        {
            if (isFinishedPattern)
            {
                return GameStateMachine.GameState.PlayerTurn;
            }

            return GameStateMachine.GameState.SimonTurn;
        }

        public override void UpdateState() { }

        IEnumerator PlayPattern(float waitTime)
        {
            foreach (int lightIndex in ctx.SimonsPattern)
            {
                ctx.GTC.SendSwitchPatternLight(lightIndex, true);
                yield return new WaitForSeconds(0.75f);
                ctx.GTC.SendSwitchPatternLight(lightIndex, false);
                yield return new WaitForSeconds(waitTime);
            }

            isFinishedPattern = true;
        }
    }
}