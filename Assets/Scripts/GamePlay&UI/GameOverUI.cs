using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
///<summary>
///GameOverUI
///</summary>
[DisallowMultipleComponent]
public class GameOverUI : MonoBehaviour
{
    [SerializeField] Image fadePlane;
    [SerializeField] Text finalScore;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] GameObject[] otherUI;

    int finalScoreValue;
    private void Start()
    {
        FindObjectOfType<PlayerInfo>().OnDeathHandler += OnGameOver;
    }

    private void OnGameOver()
    {
        finalScoreValue = FindObjectOfType<ScoreUI>().scoreValue;
        foreach (var item in otherUI)
        {
            item.SetActive(false);
        }
        StartCoroutine(Fade(Color.clear, Color.black, 1));
    }

    IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1f / time;
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);
            yield return null;
        }
        gameOverUI.SetActive(true);
        finalScore.text = "Final Score:" + finalScoreValue;
    }
    public void StartNewGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
