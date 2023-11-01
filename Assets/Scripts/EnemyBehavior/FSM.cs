using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 状态种类枚举
/// </summary>
[Serializable]
public enum StateType
{
    Chase, Attack, Retreat, Win
}

///<summary>
///有限状态机
///</summary>
[DisallowMultipleComponent]
public class FSM : MonoBehaviour
{
    private IState currenState;
    private Dictionary<StateType, IState> states;
    [HideInInspector]public EnemyInfo info;
    [HideInInspector]public Transform player;
    [HideInInspector]public NavMeshAgent agent;
    private PlayerInfo playerInfo;

    private void Start()
    {
        try
        {
            playerInfo = FindObjectOfType<PlayerInfo>();
            playerInfo.OnDeathHandler += OnTargetLosing;
            player = Camera.main.transform;

            info = GetComponent<EnemyInfo>();
            agent = GetComponent<NavMeshAgent>();
            states = new()
            {
                { StateType.Chase, new ChaseState(this)},
                { StateType.Attack, new AttackState(this)},
                { StateType.Retreat, new RetreatState(this)},
                { StateType.Win, new WinState(this)}
            };
            TransferState(StateType.Chase);
        }
        catch
        {
            states = new()
            {
                { StateType.Win, new WinState(this)}
            };
            OnTargetLosing();
        }
    }

    private void OnTargetLosing()
    {
        TransferState(StateType.Win);
    }

    public void TransferState(StateType type)
    {
        if(currenState != null) currenState.OnExit();
        currenState = states[type];
        currenState.OnEnter();
    }

    private void Update()
    {
        currenState.OnUpdate();
    }
}
