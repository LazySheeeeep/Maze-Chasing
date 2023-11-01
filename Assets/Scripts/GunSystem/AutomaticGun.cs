using UnityEngine;
/// <summary>
/// ����ǹ
/// </summary>
public class AutomaticGun : Gun
{
    private float timer;

    [Header("�������ʱ��")][SerializeField] private float interval;

    override protected void Start()
    {
        timer = 0;
        base.Start();
    }

    override protected void Update()
    {
        base.Update();
        if (Input.GetMouseButton(0) && timer <= Time.time)
        {
            Firing();
            timer = Time.time + interval;
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(nameof(UpdateAmmo));
        }
    }
}
