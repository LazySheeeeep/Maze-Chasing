using UnityEngine;
using UnityEngine.UI;
///<summary>
///EnemyUI
///</summary>
[DisallowMultipleComponent]
public class EnemyUI : MonoBehaviour
{
    Canvas canvas;
    Text bloodCount;
    LivingEntity info;
    [SerializeField]Image blood;
    [SerializeField]EnemyInfo enemyInfo;

    Camera cam;
    float percent;
    private void Start()
    {
        bloodCount = GetComponentInChildren<Text>();
        info = GetComponentInParent<EnemyInfo>();
        info.OnDamageHandler += AdjustBloodUI;
        bloodCount.text = string.Format($"{info.HP}/{info.MaxHP}");
        canvas = GetComponent<Canvas>();
        cam = Camera.main;
        canvas.worldCamera = cam;
    }
    private void Update()
    {
        transform.LookAt(cam.transform);
    }
    private void AdjustBloodUI()
    {
        percent = (float)enemyInfo.HP / (float)enemyInfo.MaxHP;
        blood.fillAmount = percent;
        if (percent > .8)
            blood.color = Color.green;
        else if (percent > .3)
            blood.color = Color.yellow;
        else blood.color = Color.red;
        bloodCount.text = string.Format($"{info.HP}/{info.MaxHP}");
    }
}
