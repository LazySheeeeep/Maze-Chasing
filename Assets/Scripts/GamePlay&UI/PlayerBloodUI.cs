using UnityEngine;
using System.Collections;
using UnityEngine.UI;
///<summary>
///PlayerBloodUI
///</summary>
[DisallowMultipleComponent]
public class PlayerBloodUI : MonoBehaviour
{
    Image blood;
    Text bloodCount;
    PlayerInfo info;
    float percent;
    private void Start()
    {
        blood = GetComponent<Image>();
        bloodCount = GetComponentInChildren<Text>();

        StartCoroutine(nameof(LateStart));
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.3f);
        info = FindObjectOfType<PlayerInfo>();
        if (info == null) print("没找到玩家信息脚本！");
        info.OnDamageHandler += AdjustBloodUI;
        bloodCount.text = string.Format($"{info.HP}/{info.MaxHP}");
    }

    private void AdjustBloodUI()
    {
        percent = (float)info.HP / (float)info.MaxHP;
        blood.fillAmount = percent;
        if(percent > .8f)
        {
            blood.color = Color.green;
        }else if(percent > .3f)
        {
            blood.color = Color.yellow;
        }else
            blood.color = Color.red;
        bloodCount.text = string.Format($"{info.HP}/{info.MaxHP}");
    }
}
