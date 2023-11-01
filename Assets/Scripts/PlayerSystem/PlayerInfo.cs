using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
///<summary>
///PlayerInfo
///</summary>
[DisallowMultipleComponent]
public class PlayerInfo : LivingEntity
{
    public Gun[] GUNS;
    Queue<Gun> guns;
    [HideInInspector]public Gun currentGun;
    [SerializeField] AudioClip switchGun;
    bool isDoingCoroutine;
    public Action AdjustAmmoCountUI;
    public event Action OnWeaponSwitchBeginHandler;
    public event Action OnWeaponSwitchEndHandler;

    protected override void Start()
    {
        base.Start();
        guns = new();
        GUNS = GetComponentsInChildren<Gun>();
        StartCoroutine(nameof(LateStart));
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForSeconds(.2f);
        foreach (var item in GUNS)
        {
            item.OnBulletCountChangeHandler += AdjustAmmoCountUI;
            item.enabled = false;
            guns.Enqueue(item);
        }
        currentGun = guns.Dequeue();
        currentGun.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
        {
            StartCoroutine(nameof(SwitchGun));
        }
    }

    private IEnumerator SwitchGun()
    {
        if (!isDoingCoroutine)
        {
            OnWeaponSwitchBeginHandler?.Invoke();
            isDoingCoroutine = true;
            currentGun.enabled = false;
            guns.Enqueue(currentGun);
            currentGun = guns.Dequeue();
            yield return new WaitForSeconds(switchGun.length);
            currentGun.enabled = true;
            OnWeaponSwitchEndHandler?.Invoke();
            isDoingCoroutine = false;
        }
    }
}