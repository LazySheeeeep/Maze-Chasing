using UnityEngine;
///<summary>
///AttackState
///</summary>
public class AttackState : IState
{
    private FSM manager;
    Gun gun;
    float atkIntervalTime;
    float nextAtkTime;
    Transform transform;
    Transform player;

    public AttackState(FSM manager)
    {
        this.manager = manager;
        gun = manager.info.gun;
        atkIntervalTime = manager.info.aggressivity;
        transform = manager.transform;
        player = manager.player;
    }

    public void OnEnter()
    {
        manager.agent.enabled = false;
        nextAtkTime = Time.time;
    }

    public void OnExit()
    {
        if (gun.currentAmmoCount < gun.ammoCapacity / 2)
        {
            if (!gun.isDoingCoroutine && gun != null)
                gun.StartCoroutine(nameof(gun.UpdateAmmo));
        }
    }

    public void OnUpdate()
    {
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        gun.transform.LookAt(manager.player);
        if(Vector3.Distance(transform.position,player.position) > 100f) {
            manager.TransferState(StateType.Chase);
        }
        else if(nextAtkTime < Time.time)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f) 
                && hit.collider.CompareTag("Player"))
            {
                nextAtkTime += atkIntervalTime;
                if (gun.currentAmmoCount > 0) gun.Firing();
                else manager.TransferState(StateType.Retreat);
            }
            else manager.TransferState(StateType.Chase);
        }
    }
}