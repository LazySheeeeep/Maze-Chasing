using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
///<summary>
///EnemySpawner
///</summary>
[DisallowMultipleComponent]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField,Tooltip("敌人预制件")] GameObject enemyPrefab;

    [SerializeField,Tooltip("从瓦片提示开始时到敌人出生的延迟时间")] float spawnDelay;
    [SerializeField,Tooltip("瓦片闪烁速度")] float tileFlashSpeed;
    [SerializeField] Color flashColor;
    [SerializeField] private string wavesDataFileName;
    [SerializeField] ScoreUI scoreUI;
    [HideInInspector]public int currentWaveSequence;
    public float waveSpawnInterval;
    public float mapSwitchDelayTime;

    public Wave CurrentWave { get; private set; }
    public event Action OnWaveGenerateHandler;
    public event Action OnPlayerWinHandler;
    public event Action OnWaveCleanHandler;

    //[SerializeField,Header("波参数设置"),Tooltip("设置每一波敌人的信息，调整每一波难度")]
    //private Wave[] wavesInfoSettings;

    private Queue<Wave> waves;
    private bool enableToSpawn;
    private int numToSpawn;
    private int aliveCount;
    private float nextSpawnTime;
    private MapGenerator map;
    private PlayerInfo player;

    private void Start()
    {
        //waves = wavesInfoSettings.Length > 0 ?
        //    new Queue<Wave>(wavesInfoSettings) :
        //    new Queue<Wave>(DataManager<WavesData>.LoadFromJsonFile(wavesDataFileName).waves);
        //DataManager<WavesData>.WriteToJsonFile(new WavesData() { waves = wavesInfoSettings },wavesDataFileName,true);
        waves = new (DataManager<WavesData>.LoadFromJsonFile(wavesDataFileName).waves);
        map = FindObjectOfType<MapGenerator>();
        player = FindObjectOfType<PlayerInfo>();
        player.OnDeathHandler += PlayerDeath;
        enableToSpawn = true;
        currentWaveSequence = 0;
        nextSpawnTime = 0;
        StartCoroutine(nameof(GenerateNextWave));
    }

    private void PlayerDeath()
    {
        enableToSpawn=false;
        StopCoroutine(nameof(GenerateAnEnemy));
    }

    private IEnumerator GenerateNextWave()
    {
        ++currentWaveSequence;
        enableToSpawn = false;
        OnWaveGenerateHandler?.Invoke();
        CurrentWave = waves.Dequeue();
        numToSpawn = CurrentWave.EnemyNumber;
        aliveCount = numToSpawn;
        yield return new WaitForSeconds(waveSpawnInterval);
        enableToSpawn = true;
    }

    private void Update()
    {
        if (enableToSpawn)
        {
            if((numToSpawn > 0 || CurrentWave.Infinite) && nextSpawnTime < Time.time)
            {
                numToSpawn--;
                nextSpawnTime = Time.time + UnityEngine.Random.Range(CurrentWave.SpawnIntervalRange.x, CurrentWave.SpawnIntervalRange.y);

                StartCoroutine(nameof(GenerateAnEnemy));
            }
        }
    }

    private IEnumerator GenerateAnEnemy()
    {
        var randomTile = map.GetRandomSpareTile();
        var mat = randomTile.GetComponent<MeshRenderer>().material;
        Color origColor = mat.color;
        float timer = 0;
        while(timer < spawnDelay)
        {
            mat.color = Color.Lerp(origColor, flashColor, Mathf.PingPong(timer * tileFlashSpeed, 1));
            timer += Time.deltaTime;
            yield return null;
        }
        GameObject spawnEnemyObj = Instantiate(enemyPrefab, randomTile.position + Vector3.up, Quaternion.identity,transform);
        spawnEnemyObj.SetActive(false);
        EnemyInfo enemyInfo = spawnEnemyObj.GetComponent<EnemyInfo>();
        enemyInfo.OnDeathHandler += EnemyDeath;
        enemyInfo.SetInfomation(
            CurrentWave.PositivityRange,
            CurrentWave.AggressivityRange,
            CurrentWave.ATKRange,
            CurrentWave.MaxHpRange,
            CurrentWave.DexterityRange,
            CurrentWave.SkinColors);
        spawnEnemyObj.SetActive(true);
    }

    private void EnemyDeath()
    {
        scoreUI.OnScoreUpdate();
        aliveCount--;
        if(aliveCount == 0)
        {
            if (waves.Count > 0)
            {
                OnWaveCleanHandler?.Invoke();
                StartCoroutine(nameof(DelayMapSwitch));
            }
            else
            {
                OnPlayerWinHandler?.Invoke();
            }
        }
    }

    private IEnumerator DelayMapSwitch()
    {
        yield return new WaitForSeconds(mapSwitchDelayTime);
        if (map.maps.Count > 0) map.GenerateMap();
        StartCoroutine(nameof(GenerateNextWave));
    }
}
