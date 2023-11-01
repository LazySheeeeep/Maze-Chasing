using UnityEngine;

public class BulletShooting : MonoBehaviour {

	public float existTime = 2;
    public int ATK = 10;
	public float speed=100;

    [Header("弹孔特效")]public GameObject bulletHoleEffect;
	[Header("溅血特效")]public GameObject bloodEffect;
	[Header("火花特效")] public GameObject hitVFX;

	[Header("设置子弹击中无效的层")]public LayerMask ignoreLayer;

	RaycastHit hit;
	private float timer=0;

	void Update () {
		if (timer < existTime)
		{
			timer += Time.deltaTime;
			CheckCollision();
			transform.position += speed * Time.deltaTime * transform.forward;
		}
        else
        {
			Destroy(gameObject);
        }
	}

	private void CheckCollision()
	{
		if (Physics.Raycast(transform.position, transform.forward, out hit, speed * Time.deltaTime, ~ignoreLayer))
		{
			if (hit.transform.CompareTag("Environment"))
			{
				if(bulletHoleEffect!=null)
					Instantiate(bulletHoleEffect, hit.point + hit.normal * .01f, Quaternion.LookRotation(hit.normal));
				if(hitVFX!=null)
					Instantiate(hitVFX, hit.point + hit.normal * .01f, Quaternion.LookRotation(hit.normal));
				Destroy(gameObject);
			}
			if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Player"))
			{
				if (bloodEffect != null)
					Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
				var item = hit.transform.GetComponent<IDamageable<int>>();
				item.Damage(ATK, hit.point,transform.forward);
				Destroy(gameObject);
			}
		}
	}
}
