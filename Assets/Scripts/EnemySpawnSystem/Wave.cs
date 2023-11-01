using UnityEngine;
using System;
///<summary>
///Wave
///</summary>
[Serializable]
public class Wave
{
    public bool Infinite;

    public int EnemyNumber = 2;

    public Vector2 SpawnIntervalRange = new (5f, 10f);

    public Vector2 PositivityRange = new (2, 3);

    public Vector2 AggressivityRange = new (0.5f, 1f);

    public Vector2Int ATKRange = new (8, 12);

    public Vector2Int MaxHpRange = new (50,80);

    public Vector2 DexterityRange = new (2, 4);

    public Color[] SkinColors = new Color[2] {Color.blue,Color.cyan};

}

public class WavesData
{
    public Wave[] waves;
}