using System.Collections;
using UnityEngine;
///<summary>
///GameSettings
///</summary>
[DisallowMultipleComponent]
public class GameSettings : MonoBehaviour
{
    [Header("ÕÊº“…Ë÷√")]
    public GameObject playerPrefab;
    public int MaxHP;

    Vector3 playerBornPosition;
    Camera cam;
    MapGenerator map;
    PlayerInfo info;
    private void Awake()
    {
        playerBornPosition = new Vector3(.5f, 2f, .5f);
        map = FindObjectOfType<MapGenerator>();
        map.OnMapReloadHandler += AdjustMapSceneCamera;
        map.OnMapReloadHandler += ()=> info.transform.position = playerBornPosition;
        cam = GetComponent<Camera>();
        info = Instantiate(playerPrefab, playerBornPosition, Quaternion.identity).GetComponent<PlayerInfo>();
        info.MaxHP = MaxHP;
        info.OnDeathHandler += () => StartCoroutine(nameof(PlayerDeath));
    }

    private void AdjustMapSceneCamera()
    {
        cam.enabled = false;
        cam.enabled = true;
        cam.orthographicSize = Mathf.Max(map.mapCenter.x, map.mapCenter.y);
    }

    private IEnumerator PlayerDeath()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        info.gameObject.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            cam.enabled = !cam.enabled;
        }
        if (info.transform.position.y < -1)
        {
            if (info.IsAlive) info.Damage(info.HP, Vector3.zero, Vector3.zero);
        }
    }
}