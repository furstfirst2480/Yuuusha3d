using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosssound : MonoBehaviour
{
    public List<AudioClip> bossWalking;
    public AudioClip bossbreak;
    public AudioClip bossCries;
    private AudioSource BossSource;
    public AudioClip GiantSwing;
    public AudioClip GroundBreak;
    public AudioClip bossdead;
    public int pos;
    public GameObject EdgyEffect;
    public AudioClip bomb;

    public static bosssound instance;

    private void Awake()
    {
        instance = this;
        BossSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update

    public void bossWalkingSFX()
    {
        pos = (int)Mathf.Floor(Random.Range(0, bossWalking.Count));
        BossSource.PlayOneShot(bossWalking[pos]);

    }

    public void bossbreakSFX()
    {
        BossSource.PlayOneShot(bossbreak);
    }


    public void bossCriesSFX()
    {
        BossSource.PlayOneShot(bossCries);
    }
    public void bossSwingSFX()
    {
        BossSource.PlayOneShot(GiantSwing);
    }
    public void groundbreakSFX()
    {
        BossSource.PlayOneShot(GroundBreak);
    }
    public void bossdeadSFX()
    {
        BossSource.PlayOneShot(bossdead);
    }
    public void bossJumpAttack()
    {
        EdgyEffect.SetActive(true);
        StartCoroutine(StopEdgy());
        BossSource.PlayOneShot(bomb);
    }
    private IEnumerator StopEdgy()
    {
        yield return new WaitForSeconds(0.8f);
        EdgyEffect.SetActive(false);
    }
}
