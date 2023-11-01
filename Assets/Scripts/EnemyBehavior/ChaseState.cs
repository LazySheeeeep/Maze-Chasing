using UnityEngine.AI;
using UnityEngine;
///<summary>
///ChaseState×·×Ù×´Ì¬
///</summary>
public class ChaseState : IState
{
    private FSM manager;
    private readonly float intervalTime;
    float nextSetTime;
    readonly float fov;
    readonly Transform transform;
    readonly Transform player;
    readonly NavMeshAgent agent;
    readonly LayerMask ignoreLayer;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        agent = manager.agent;
        agent.speed = manager.info.dexterity;
        intervalTime = manager.info.positivity;
        player = manager.player;
        transform = manager.transform;
        fov = manager.info.fov;
        ignoreLayer = manager.info.ignore;
    }

    public void OnEnter()
    {
        agent.enabled = true;
        agent.SetDestination(manager.player.position);
        nextSetTime = Time.time + intervalTime;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {
        if (nextSetTime < Time.time)
        {
            nextSetTime += intervalTime;
            agent.SetDestination(player.position);
        }
        else
        {
            if (FoundPlayer())
            {
                manager.TransferState(StateType.Attack);
            }
        }
    }

    private bool FoundPlayer()
    {
        var vec = player.position - transform.position;
        if (vec.sqrMagnitude < 100f && Vector3.Angle(vec, transform.forward) < fov)
        {
            Physics.Raycast(transform.position, vec, out RaycastHit hit, 10f,~ignoreLayer);
            if (hit.collider.CompareTag("Player")) { return true; }
        }
        return false;
    }
}
