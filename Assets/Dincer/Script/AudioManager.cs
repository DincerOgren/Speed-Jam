using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    // Start is called before the first frame update
    public AudioSource audioSource;
    [Header("Audio Clips")]
    public AudioClip stageMusic;
    public AudioClip bossStageEnterSound;
    
    public AudioClip bossStageEndMusic;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
        
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        ChangeBGM(stageMusic);
        DontDestroyOnLoad(gameObject);

    }

    public void PlayBossMusic()
    {
        ChangeBGM(bossStageEndMusic);
    }

    public void ChangeBGM(AudioClip music)
    {
        audioSource.Stop();
        audioSource.clip = music;
        audioSource.Play();
        print("Playing " + music + " sfx");
    }
    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }
}
