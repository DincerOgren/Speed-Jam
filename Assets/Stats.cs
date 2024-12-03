using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stats : MonoBehaviour
{
    private PlayerStats playerStats;
    public TextMeshProUGUI nick,score;

    private void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();

        nick.text = playerStats.playerName;
        score.text = playerStats.score.ToString();

    }
}
