using UnityEngine;
///<summary>
///RetreatState
///</summary>
public class RetreatState : IState
{
    private FSM manager;
    readonly Vector3 bornPoint;

    public RetreatState(FSM manager)
    {
        this.manager = manager;
        bornPoint = manager.transform.position;
    }

    public void OnEnter()
    {
        manager.agent.enabled = true;
        manager.agent.SetDestination(bornPoint);
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (manager.info.gun.currentAmmoCount != 0)
            manager.TransferState(StateType.Chase);
    }
}
