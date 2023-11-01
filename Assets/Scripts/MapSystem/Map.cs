using UnityEngine;
using System;
///<summary>
///Map地图类，用于存储地图信息
///</summary>
[Serializable]public class Map
{
    /// <summary>
    /// 地图长宽
    /// </summary>
    public Vector2Int mapSize;

    /// <summary>
    /// [Header("障碍物密度"),  
    /// </summary>
    [Range(0, 1)]public float obsDensity;

    public Color foregroundColor;
    public Color backgroundColor;
    
    /// <summary>
    /// [Header("瓦片缝隙大小"),  
    /// </summary>
    [Range(0f, 0.3f)]public float tileOutlinePercent;

    /// <summary>
    /// [Header("障碍物缝隙大小"), 
    /// </summary>
    [Range(0f, 0.3f)]public float obsOutlinePercent;

    /// <summary>
    /// [SerializeField, Header("障碍物高度区间")]
    /// </summary>
    public Vector2 obsHeightLimit;
}

public class MapsData
{
    public Map[] maps;
}
