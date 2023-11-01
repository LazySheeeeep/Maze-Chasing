using UnityEngine;
///<summary>
///EnemyInfo
///</summary>
[DisallowMultipleComponent]
public class EnemyInfo : LivingEntity
{
    [HideInInspector]public float positivity;//决定寻路积极性
    [HideInInspector]public float aggressivity;//决定攻击间隔时间
    [HideInInspector]public int ATK;
    [HideInInspector]public float dexterity;//灵活性，决定前进速度
    [HideInInspector]public Color color;

    public float fov =60f;
    public LayerMask ignore;
    [SerializeField] private ParticleSystem deathEffect;

    public Gun gun;

    public void SetInfomation(Vector2 PositivityRange, Vector2 AggressivityRange, Vector2Int ATKRange,
        Vector2Int MaxHpRange, Vector2 DexterityRange, Color[] colors)
    {
        positivity              = Random.Range(PositivityRange.x, PositivityRange.y);
        aggressivity    = Random.Range(AggressivityRange.x, AggressivityRange.y);
        ATK             = Random.Range(ATKRange.x, ATKRange.y);
        gun.atk = ATK;
        dexterity       = Random.Range(DexterityRange.x, DexterityRange.y);
        MaxHP           = Random.Range(MaxHpRange.x, MaxHpRange.y);
        color = colors[Random.Range(0, colors.Length)];
        GetComponent<MeshRenderer>().material.color = color;
    }

    protected override void Die(Vector3 hitPoint, Vector3 hitDirection)
    {
        base.Die(hitPoint, hitDirection);
        Destroy(gameObject);
        deathEffect.GetComponent<MeshRenderer>().sharedMaterial.color = color;
        Destroy(Instantiate(
                deathEffect.gameObject,
                hitPoint,
                Quaternion.FromToRotation(Vector3.forward, hitDirection)),
                deathEffect.main.startLifetime.constant);
    }
}