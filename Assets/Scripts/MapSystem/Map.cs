using UnityEngine;
using System;
///<summary>
///Map��ͼ�࣬���ڴ洢��ͼ��Ϣ
///</summary>
[Serializable]public class Map
{
    /// <summary>
    /// ��ͼ����
    /// </summary>
    public Vector2Int mapSize;

    /// <summary>
    /// [Header("�ϰ����ܶ�"),  
    /// </summary>
    [Range(0, 1)]public float obsDensity;

    public Color foregroundColor;
    public Color backgroundColor;
    
    /// <summary>
    /// [Header("��Ƭ��϶��С"),  
    /// </summary>
    [Range(0f, 0.3f)]public float tileOutlinePercent;

    /// <summary>
    /// [Header("�ϰ����϶��С"), 
    /// </summary>
    [Range(0f, 0.3f)]public float obsOutlinePercent;

    /// <summary>
    /// [SerializeField, Header("�ϰ���߶�����")]
    /// </summary>
    public Vector2 obsHeightLimit;
}

public class MapsData
{
    public Map[] maps;
}
