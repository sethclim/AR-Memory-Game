using UnityEngine;

namespace ARMG
{
    public class GameOverState : BaseState<GameStateMachine.GameState>
    {
        GameStateMachine ctx;

        public GameOverState(GameStateMachine.GameState _key, GameStateMachine _ctx) : base(_key)
        {
            ctx = _ctx;
        }

        public override void EnterState()
        {
            Debug.Log("Entered Game Over State");
        }

        public override void ExitState()
        {
            Debug.Log("Exited Game Over State");
        }

        public override GameStateMachine.GameState GetNextState()
        {
            // TODO:
            // 1. Wait X seconds then go back to ReadyUpScreen
            // 1.1 Display winner, flash lights, etc.

            return GameStateMachine.GameState.GameOver;
        }

        public override void UpdateState()
        {

        }
    }
}
