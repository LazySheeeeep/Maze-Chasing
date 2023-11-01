using System;
///<summary>
///IDeath提供死亡事件，是否存活判据
///</summary>
public interface IDeath
{
    bool IsAlive { get; }
    event Action OnDeathHandler;
}
