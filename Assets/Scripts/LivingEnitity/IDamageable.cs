using UnityEngine;
using System;
///<summary>
///IDamageable提供造成伤害的函数接口
///</summary>
public interface IDamageable<T> where T : IComparable<T>
{
    event Action OnDamageHandler;
    void Damage(T amount,Vector3 hitPoint, Vector3 hitDirection);
}
