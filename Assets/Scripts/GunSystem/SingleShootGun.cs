using UnityEngine;

public class SingleShootGun : Gun
{
    protected override void Start()
    {
        base.Start();
    }

    override protected void Update()
    {
        base.Update();
        if (Input.GetMouseButtonDown(0))
        {
            Firing();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(nameof(UpdateAmmo));
        }
    }
}