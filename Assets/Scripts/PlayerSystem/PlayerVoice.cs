using System.Collections;
using UnityEngine;
///<summary>
///PlayerVoice
///</summary>
[DisallowMultipleComponent]
public class PlayerVoice : MonoBehaviour
{
    [SerializeField]AudioClip[] hurt;
    [SerializeField]AudioClip die;
    [SerializeField]AudioClip switchWeapon;
    [SerializeField]AudioClip[] move;

    bool isPlayingStep;
    AudioSource source;
    [SerializeField]AudioSource gunSource;
    Rigidbody rb;
    PlayerInfo info;
    private void Start()
    {
        isPlayingStep = false;
        source = GetComponent<AudioSource>();
        info = GetComponent<PlayerInfo>();
        rb = GetComponent<Rigidbody>();
        info.OnDamageHandler += PlayHurtVoice;
        info.OnDeathHandler += ()=> source.PlayOneShot(die);
        info.OnWeaponSwitchBeginHandler += () => { gunSource.Stop();gunSource.PlayOneShot(switchWeapon); };
    }
    private void Update()
    {
        if (Mathf.Abs(rb.velocity.y) < .1f)
        {
            if (Mathf.Abs(rb.velocity.x) > 1f || Mathf.Abs(rb.velocity.z) > 1f)
            {
                StartCoroutine(nameof(PlayStepVoice));
            }
        }
    }

    private IEnumerator PlayStepVoice()
    {
        if (!isPlayingStep)
        {
            isPlayingStep = true;
            source.PlayOneShot(move[Random.Range(0, move.Length)]);
            yield return new WaitForSeconds(0.5f);
            isPlayingStep = false;
        }
    }

    private void PlayHurtVoice()
    {
        if (info.HP > 0)
        {
            source.Stop();
            source.PlayOneShot(hurt[Random.Range(0, hurt.Length)]);
        }
    }
}
