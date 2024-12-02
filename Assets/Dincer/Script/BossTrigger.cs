using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public static BossTrigger instance;


    public AudioClip clip;
    public GameObject bossTriger;
    public GameObject doorClosed;
    public bool isOnBossStage = false;
    public GameObject bossHealthBar;
    bool playMusic = false;

    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        if (!AudioManager.instance.audioSource.isPlaying && isOnBossStage && !playMusic)
        {
            playMusic = true;
            AudioManager.instance.PlayBossMusic();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bossHealthBar.SetActive(true);
            doorClosed.SetActive(true);
            if (!isOnBossStage)
            {
                isOnBossStage = true;
                AudioManager.instance.audioSource.loop = false;
                AudioManager.instance.ChangeBGM(clip);
            }
        }
    }
}

