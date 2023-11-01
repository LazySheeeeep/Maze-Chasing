using UnityEngine;
using System;
///<summary>
///LivingEntity生命体基类
///</summary>
[DisallowMultipleComponent]
public abstract class LivingEntity : MonoBehaviour, IHealth, IDamageable<int>, IDeath
{
    public int MaxHP { get ; set ; }
    public int HP {get;protected set; }

    public bool IsAlive{get; protected set; }
    public event Action OnDeathHandler;
    public event Action OnDamageHandler;

    protected virtual void Start()
    {
        HP = MaxHP;
        IsAlive = true;
    }

    public virtual void Damage(int amount,Vector3 hitPoint, Vector3 hitDirection)
    {
        if (IsAlive)
        {
            HP -= amount;
            if (HP <= 0)
            {
                HP = 0;
                IsAlive = false;
                Die(hitPoint, hitDirection);
            }
            else
                OnDamageHandler?.Invoke();
        }
    }

    protected virtual void Die(Vector3 hitPoint, Vector3 hitDirection)
    {
        IsAlive = false;
        OnDeathHandler?.Invoke();
    }

}
