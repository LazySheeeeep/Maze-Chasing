using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
///<summary>
///PlayerAmmoUI
///</summary>
[DisallowMultipleComponent]
public class PlayerAmmoUI : MonoBehaviour
{
    Gun gun;
    Image ammo;
    Text ammoCount;
    float percent;
    PlayerInfo info;
    private void Start()
    {
        ammo = GetComponent<Image>();
        ammoCount = GetComponentInChildren<Text>();
        StartCoroutine(nameof(LateStart));
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.1f);
        info = FindObjectOfType<PlayerInfo>();
        info.AdjustAmmoCountUI = AdjustAmmoCountUI;
        gun = info.currentGun;
        ammoCount.text = string.Format($"<size=26>{gun.currentAmmoCount}</size><size=15>/{gun.packageBulletCount}</size>");
        info.OnWeaponSwitchBeginHandler += OnWeaponSwitchBegin;
        info.OnWeaponSwitchEndHandler += OnWeaponSwitchEnd;
        info.OnWeaponSwitchEndHandler += AdjustAmmoCountUI;
    }

    private void AdjustAmmoCountUI()
    {
        percent = (float)gun.currentAmmoCount / (float)gun.ammoCapacity;
        ammo.fillAmount = percent;
        if (percent > .8f)
        {
            ammo.color = Color.green;
        }
        else if (percent > .3f)
        {
            ammo.color = Color.yellow;
        }
        else
            ammo.color = Color.red;

        ammoCount.text = string.Format($"<size=26>{gun.currentAmmoCount}</size><size=15>/{gun.packageBulletCount}</size>");
    }

    private void OnWeaponSwitchBegin()
    {
        ammo.color = Color.blue;
        ammo.fillAmount = 1f;
        ammoCount.text = string.Format($"<size=26>---</size><size=15>/----</size>");
    }

    private void OnWeaponSwitchEnd()
    {
        gun = info.currentGun;
    }
}
