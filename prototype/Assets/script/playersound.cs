using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playersound : MonoBehaviour
{
    public List<AudioClip> playerWalking;
    public AudioClip playerJumping;
    public AudioClip playerAttack;
    public AudioClip playerDrink;
    private AudioSource playerSource;
    public int pos;

    public static playersound instance;

    private void Awake()
    {
        instance = this;
        playerSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
 
    public void playWalking()
    {
        pos = (int)Mathf.Floor(Random.Range(0, playerWalking.Count));
        playerSource.PlayOneShot(playerWalking[pos]);

    }

    public void playJumping()
    {
        playerSource.PlayOneShot(playerJumping);
    }
    public void playAttack()
    {
        playerSource.PlayOneShot(playerAttack);
    }
    public void playDrinking()
    {
        playerSource.PlayOneShot(playerDrink);
    }
}
