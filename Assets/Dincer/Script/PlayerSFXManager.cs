using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSFXManager : MonoBehaviour
{
    public static PlayerSFXManager instance;
    // Start is called before the first frame update
    public AudioSource audioSource;
    [Header("Audio Clips")]
    public AudioClip attack1;
    public AudioClip chargeAttack;

    private void Awake()
    {
        

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        instance = this;
    }
    public void PlayLightAttack()
    {
        print("SES1");
        audioSource.PlayOneShot(attack1);
    }

    public void PlayChargeAttack()
    {
        audioSource.PlayOneShot(chargeAttack);
        print("SES2");

    }
    public void ChangeBGM(AudioClip music)
    {
        audioSource.Stop();
        audioSource.clip = music;
        audioSource.Play();
        print("Playing " + music + " sfx");
    }
}