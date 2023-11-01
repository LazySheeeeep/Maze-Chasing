using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
///<summary>
///WaveUI
///</summary>
[DisallowMultipleComponent]
public class WaveUI : MonoBehaviour
{
    [SerializeField] EnemySpawner es;
    [SerializeField] Text waveUIText;
    float timer;
    private void Start()
    {
        es.OnWaveGenerateHandler += ()=> StartCoroutine(nameof(AdjustWaveCount));
        es.OnWaveCleanHandler += ()=> StartCoroutine(nameof(ShowWaveClean));
        es.OnPlayerWinHandler += OnPlayerWin;
    }

    private void OnPlayerWin()
    {
        waveUIText.enabled = true;
        waveUIText.rectTransform.localScale = Vector3.one;
        waveUIText.text = "-YOU WIN-";
    }

    private IEnumerator ShowWaveClean()
    {
        waveUIText.enabled = true;
        timer = 0;
        waveUIText.rectTransform.localScale = Vector3.one;
        waveUIText.text = "-Wave Cleaned-";
        while(timer < es.mapSwitchDelayTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        waveUIText.enabled = false;
    }

    private IEnumerator AdjustWaveCount()
    {
        waveUIText.enabled = true;
        timer = 0;
        waveUIText.text = "- Wave " + es.currentWaveSequence + " -";
        while(timer < es.waveSpawnInterval / 2)
        {
            timer += Time.deltaTime;
            waveUIText.rectTransform.localScale = new Vector3(1, Mathf.Lerp(0, 1, timer * 2f / es.waveSpawnInterval), 1);
            yield return null;
        }
        while(timer < es.waveSpawnInterval)
        {
            timer += Time.deltaTime;
            waveUIText.rectTransform.localScale = new Vector3(1, Mathf.Lerp(1, 0, timer * 2f / es.waveSpawnInterval - 1),1);
            yield return null;
        }
        waveUIText.enabled = false;
    }
}