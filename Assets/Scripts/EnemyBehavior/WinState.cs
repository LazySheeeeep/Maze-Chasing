using UnityEngine;
///<summary>
///WinState
///</summary>
public class WinState : IState
{
    FSM manager;
    EnemyUI ui;
    public WinState(FSM _manager)
    {
        manager = _manager;
        ui = manager.GetComponentInChildren<EnemyUI>();
    }

    public void OnEnter()
    {
        if (ui != null) ui.enabled = false;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}
