using UnityEngine;
using UnityEngine.UI;
using System.Collections;
///<summary>
///ScoreUI
///</summary>
[DisallowMultipleComponent]
public class ScoreUI : MonoBehaviour
{
    [SerializeField]Text scoreUI;
    [SerializeField]Text comboUI;
    public int scoreValue { get; private set; }
    int comboValue;
    [SerializeField] int difficulty;
    [SerializeField] float comboTime;
    float intervalTime;
    float lastTime;
    public void OnScoreUpdate()
    {
        comboUI.enabled = true;
        StopCoroutine(nameof(CloseCombo));
        intervalTime = Time.time - lastTime;
        if (intervalTime < comboTime)
        {
            comboValue++;
        }
        else
        {
            comboValue = 1;
        }
        scoreValue += (int)Mathf.Pow(difficulty, comboValue);
        comboUI.text = "Combo x<color=orange>" + comboValue + "</color>";
        scoreUI.text = "Score: <color=orange>" + scoreValue + "</color>";
        StartCoroutine(nameof(CloseCombo));
        lastTime = Time.time;
    }

    IEnumerator CloseCombo()
    {
        yield return new WaitForSeconds(comboTime);
        comboUI.enabled = false;
    }
    private void Start()
    {
        comboUI.enabled = false;
        comboValue = 0;
        scoreValue = 0;
        lastTime = -12;
        scoreUI.text = "Score: <color=orange>" + scoreValue + "</color>";
    }
}
