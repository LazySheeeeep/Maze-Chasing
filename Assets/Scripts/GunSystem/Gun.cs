using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource),typeof(Animator))]
public class Gun : MonoBehaviour
{
    #region �ֶ�����
    private AudioSource audioSource;

    /// <summary>
    /// ������
    /// </summary>
    [Header("���ο��𹥻���")] public int atk;

    private Animator anim;

    [Header("������Ч"), SerializeField] private AudioClip fireSound;
    [Header("�ɻ���Ч"), SerializeField] private AudioClip dryfireSound;
    [Header("������Ч"), SerializeField] private AudioClip ammoUpdatingSound;
    [Header("������Ч"), SerializeField] private GameObject[] muzzleFlashPrefabs;

    [Header("��ϻ����")] public int ammoCapacity = 30;

    [Header("��ǰ��ϻ����")] public int currentAmmoCount;

    [Header("�����ӵ���")] public int packageBulletCount = 300;

    [Header("�ӵ�Ԥ�Ƽ�")] public GameObject bulletPrefab;

    [Header("�����")] public Transform firePoint;

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
    /// ���� 
    /// </summary>
    public void Firing()
    {
        if (!IsReady()) return;
        audioSource.PlayOneShot(fireSound);
        anim.Play("Base Layer.Fire");
        //����Ч���š�ռλ
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position,Quaternion.LookRotation(transform.forward));
        //������Ϣ
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
    /// ��������
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