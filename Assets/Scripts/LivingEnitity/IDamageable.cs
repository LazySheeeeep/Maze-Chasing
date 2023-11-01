using UnityEngine;
using System;
///<summary>
///IDamageable�ṩ����˺��ĺ����ӿ�
///</summary>
public interface IDamageable<T> where T : IComparable<T>
{
    event Action OnDamageHandler;
    void Damage(T amount,Vector3 hitPoint, Vector3 hitDirection);
}
