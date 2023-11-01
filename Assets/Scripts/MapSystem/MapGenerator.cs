using System;
using System.Collections.Generic;
using UnityEngine;
///<summary>
///MapGenerator��ͼ������
///</summary>
[DisallowMultipleComponent]
public class MapGenerator : MonoBehaviour
{
    #region �ֶ�����
    //public Map[] mapsInfoSettings;
    public Queue<Map> maps;
    private Map currentMap;
    private Vector2Int mapMaxSize;

    #region ����������
    [Header("��ͼ��С�޶��������")]public Transform area;
    [SerializeField, Header("��ƬԤ����")] private GameObject tilePrefab;
    [SerializeField, Header("�ϰ���Ԥ����")]private GameObject obsPrefab;
    [SerializeField, Header("�߽����ǽԤ����")] private GameObject obsWallPrefab;
    [SerializeField, Header("��ȡ��ͼ��Ϣ���ļ���")] private string mapsDataFileName;
    #endregion

    #region ��ǰ�ĵ�ͼ������Ϣ
    private Transform mapHolder;
    private Vector2Int mapSize;
    private Color foregroundColor;
    private Color backgroundColor;//ǰ����ɫ
    private float tileOutlinePercent;//��Ƭ��϶��СRange(0f,1f)
    private float obsOutlinePercent;//�ϰ����϶��СRange(0f, 1f)
    private Vector2 obsHeightLimit;//�ϰ���߶�����
    #endregion

    #region �洢��һЩ��Ϣ
    private List<Vector2Int> tilesCoord;
    private Transform[,] tiles;
    public Vector2Int mapCenter { get; private set; }
    private bool[,] isSpare;
    private Vector2Int[] moves = new Vector2Int[4] {Vector2Int.down,Vector2Int.up,Vector2Int.left,Vector2Int.right};
    #endregion
    #endregion

    public event Action OnMapReloadHandler;

    public Transform GetRandomSpareTile()
    {
        var coord = tilesCoord[UnityEngine.Random.Range(0, tilesCoord.Count)];
        return tiles[coord.x, coord.y];
    }

    private void Start()
    {
        //maps = mapsInfoSettings.Length > 0 ? 
        //    new(mapsInfoSettings) : 
        //    new(DataManager<MapsData>.LoadFromJsonFile(mapsDataFileName).maps);

        //DataManager<MapsData>.WriteToJsonFile(new MapsData() { maps = mapsInfoSettings},mapsDataFileName,true);
        maps = new(DataManager<MapsData>.LoadFromJsonFile(mapsDataFileName).maps);
        mapMaxSize.x = (int)area.localScale.x;
        mapMaxSize.y = (int)area.localScale.y;
        GenerateMap();
    }

    public void GenerateMap()
    {
        currentMap = maps.Dequeue();
        InitiateCurrentMapArgs();
        InitiateMapHolder();
        GenerateTiles();
        GenerateObstacles();
        GenerateObstacleWalls();
        OnMapReloadHandler?.Invoke();
    }
    private void InitiateCurrentMapArgs()
    {
        mapSize = currentMap.mapSize;
        mapCenter = mapSize / 2;
        foregroundColor = currentMap.foregroundColor;
        backgroundColor = currentMap.backgroundColor;
        isSpare = new bool[mapSize.x, mapSize.y];
        tileOutlinePercent = currentMap.tileOutlinePercent;
        obsOutlinePercent = currentMap.obsOutlinePercent;
        obsHeightLimit = currentMap.obsHeightLimit;
    }

    private void InitiateMapHolder()
    {
        if (transform.Find("MapHolder"))
        {
            Destroy(transform.Find("MapHolder").gameObject);
        }
        mapHolder = new GameObject("MapHolder").transform;
        mapHolder.SetParent(transform);
    }

    private void GenerateObstacleWalls()
    {
        var forwardWall = Instantiate(obsWallPrefab, Vector3.forward * ((mapMaxSize.y + mapSize.y) / 4), Quaternion.identity, mapHolder);
        forwardWall.transform.localScale = new Vector3(mapSize.x, 2f, (mapMaxSize.y - mapSize.y) / 2);

        var backWall = Instantiate(obsWallPrefab, Vector3.back * ((mapMaxSize.y + mapSize.y) / 4), Quaternion.identity, mapHolder);
        backWall.transform.localScale = new Vector3(mapSize.x, 2f, (mapMaxSize.y - mapSize.y) / 2);

        var leftWall = Instantiate(obsWallPrefab, Vector3.left * ((mapMaxSize.x + mapSize.x) / 4), Quaternion.identity, mapHolder);
        leftWall.transform.localScale = new Vector3((mapMaxSize.x - mapSize.x) / 2, 2f, mapSize.y);

        var rightWall = Instantiate(obsWallPrefab, Vector3.right * ((mapMaxSize.x + mapSize.x) / 4), Quaternion.identity, mapHolder);
        rightWall.transform.localScale = new Vector3((mapMaxSize.x - mapSize.x) / 2, 2f, mapSize.y);
    }

    private void GenerateTiles()
    {
        tilesCoord = new List<Vector2Int>(mapSize.x * mapSize.y);
        tiles = new Transform[mapSize.x, mapSize.y];
        for (int i = 0; i < mapSize.x; i++)
        {
            for (int j = 0; j < mapSize.y; j++)
            {
                Vector3 newPos = new(-(float)mapSize.x / 2 + .5f + i, 0, -(float)mapSize.y / 2 + .5f + j);
                var spawnTile = Instantiate(tilePrefab, newPos, Quaternion.Euler(90, 0, 0), mapHolder);
                spawnTile.transform.localScale *= 1 - tileOutlinePercent;
                tilesCoord.Add(new Vector2Int(i, j));
                tiles[i, j] = spawnTile.transform;
            }
        }
    }

    private void GenerateObstacles()
    {
        int obsCount = (int)(mapSize.x * mapSize.y * currentMap.obsDensity);
        int spawnedObsCount = 0;
        for (int i = 0; i < obsCount; i++)
        {
            var selectedCoord = tilesCoord[UnityEngine.Random.Range(0, tilesCoord.Count)];
            tilesCoord.Remove(selectedCoord);

            isSpare[selectedCoord.x,selectedCoord.y] = true;
            spawnedObsCount++;

            if (selectedCoord != mapCenter && (i < 2 || Accessible(isSpare, spawnedObsCount)))
            {
                GenerateObstacleAt(selectedCoord);
            }
            else
            {
                isSpare[selectedCoord.x, selectedCoord.y] = false;
                spawnedObsCount--;
            }
        }
    }

    private void GenerateObstacleAt(Vector2Int selectedCoord)
    {
        float obsHeight = UnityEngine.Random.Range(obsHeightLimit.x, obsHeightLimit.y);

        Vector3 newPos = new(-(float)mapSize.x / 2f + .5f + selectedCoord.x, obsHeight / 2f, -(float)mapSize.y / 2f + .5f + selectedCoord.y);
        var spawnObs = Instantiate(obsPrefab, newPos, Quaternion.identity, mapHolder);
        spawnObs.transform.localScale = new Vector3(1 - obsOutlinePercent, obsHeight, 1 - obsOutlinePercent);

        spawnObs.GetComponent<MeshRenderer>().material.color =
            Color.Lerp(foregroundColor, backgroundColor, (float)selectedCoord.y / ((float)mapSize.y - 1f));
    }

    private bool Accessible(bool[,] isSpare, int spawnedObsCount)
    {
        int hypotheticalReachablePositionCount = mapSize.x * mapSize.y - spawnedObsCount;
        Queue<Vector2Int> positionsForChecking = new(hypotheticalReachablePositionCount);
        List<Vector2Int> reachablePositions = new(hypotheticalReachablePositionCount);
        reachablePositions.Add(mapCenter);
        positionsForChecking.Enqueue(mapCenter);
        while(positionsForChecking.Count > 0)
        {
            Vector2Int currentPos = positionsForChecking.Dequeue();
            foreach (var move in moves)
            {
                var pos = currentPos + move;
                if (pos.x >= 0 && pos.x < mapSize.x && pos.y >= 0 && pos.y < mapSize.y)
                    if (!reachablePositions.Contains(pos) && !isSpare[pos.x, pos.y])
                    {
                        positionsForChecking.Enqueue(pos);
                        reachablePositions.Add(pos);
                    }
            }
        }
        return reachablePositions.Count == hypotheticalReachablePositionCount;
    }

}