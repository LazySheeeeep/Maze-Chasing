using System;
///<summary>
///IDeath�ṩ�����¼����Ƿ����о�
///</summary>
public interface IDeath
{
    bool IsAlive { get; }
    event Action OnDeathHandler;
}
