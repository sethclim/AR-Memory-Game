using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public abstract class StateMachine<EState> : MonoBehaviourPunCallbacks where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();

    protected BaseState<EState> CurrentState;

    void Awake() { }

    void Start()
    {
        CurrentState.EnterState();
    }

    void Update()
    {
        EState nextStateKey = CurrentState.GetNextState();

        if (nextStateKey.Equals(CurrentState.StateKey))
        {
            CurrentState.UpdateState();
        }
        else
        {
            TransitionToState(nextStateKey);
        }
    }

    public virtual void TransitionToState(EState statekey)
    {
        CurrentState.ExitState();
        CurrentState = States[statekey];
        CurrentState.EnterState();
    }

    // [PunRPC]
    // void RPC_TransitionState(EState statekey)
    // {
    //     CurrentState.ExitState();
    //     CurrentState = States[statekey];
    //     CurrentState.EnterState();
    // }

    // public void SendSwitchPatternLight(int lightBulbIndex, bool onOff)
    // {
    //     if (lightBulbIndex >= 0 && lightBulbIndex < m_patternLights.Length)
    //     {
    //         photonView.RPC(nameof(RPC_UpdateLightController), RpcTarget.AllBuffered, lightBulbIndex, onOff);
    //     }
    // }

    // [PunRPC]
    // void RPC_UpdateLightController(int lightBulbIndex, bool newValue)
    // {
    //     if (lightBulbIndex >= 0 && lightBulbIndex < m_patternLights.Length)
    //     {
    //         m_patternLights[lightBulbIndex].ToggleLight(newValue);
    //     }
    // }

}
