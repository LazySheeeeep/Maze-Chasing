using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource),typeof(Animator))]
public class Gun : MonoBehaviour
{
    #region 字段声明
    private AudioSource audioSource;

    /// <summary>
    /// 攻击力
    /// </summary>
    [Header("单次开火攻击力")] public int atk;

    private Animator anim;

    [Header("开火音效"), SerializeField] private AudioClip fireSound;
    [Header("干火音效"), SerializeField] private AudioClip dryfireSound;
    [Header("换弹音效"), SerializeField] private AudioClip ammoUpdatingSound;
    [Header("开火特效"), SerializeField] private GameObject[] muzzleFlashPrefabs;

    [Header("弹匣容量")] public int ammoCapacity = 30;

    [Header("当前弹匣余量")] public int currentAmmoCount;

    [Header("背包子弹数")] public int packageBulletCount = 300;

    [Header("子弹预制件")] public GameObject bulletPrefab;

    [Header("开火点")] public Transform firePoint;

    [HideInInspector] public bool isDoingCoroutine;
    #endregion

    public event Action OnBulletCountChangeHandler;
    protected virtual void Start()
    {
        currentAmmoCount = ammoCapacity;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Update()
    {

    }

    /// <summary>
    /// 开火 
    /// </summary>
    public void Firing()
    {
        if (!IsReady()) return;
        audioSource.PlayOneShot(fireSound);
        anim.Play("Base Layer.Fire");
        //火花特效播放。占位
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position,Quaternion.LookRotation(transform.forward));
        //传递信息
        bullet.GetComponent<BulletShooting>().ATK = atk;
        Instantiate(muzzleFlashPrefabs[UnityEngine.Random.Range(0, muzzleFlashPrefabs.Length)],
                firePoint.position,
                Quaternion.LookRotation(transform.forward));
        OnBulletCountChangeHandler?.Invoke();
    }

    private bool IsReady()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("UpdateAmmo"))
        {
            return false;
        }
        if (currentAmmoCount == 0)
        {
            anim.Play("Base Layer.Dryfire");
            audioSource.PlayOneShot(dryfireSound);
            return false;
        }
        currentAmmoCount--;
        return true;
    }

    /// <summary>
    /// 更换弹夹
    /// </summary>
    public IEnumerator UpdateAmmo()
    {
        if (!isDoingCoroutine)
        {
            isDoingCoroutine = true;
            if (packageBulletCount <= 0 || currentAmmoCount == ammoCapacity) yield break;
            audioSource.PlayOneShot(ammoUpdatingSound);
            anim.Play("Base Layer.UpdateAmmo");
            yield return new WaitForSeconds(ammoUpdatingSound.length);
            int numToAdd = ammoCapacity - currentAmmoCount;
            currentAmmoCount += packageBulletCount >= numToAdd ? numToAdd : packageBulletCount;
            packageBulletCount -= numToAdd;
            OnBulletCountChangeHandler?.Invoke();
            isDoingCoroutine = false;
        }
    }
}