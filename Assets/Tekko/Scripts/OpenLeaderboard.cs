using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLeaderboard : MonoBehaviour
{
    public GameObject leaderboard;
    public GameObject stats;

    private void Start()
    {
        leaderboard.SetActive(false);
    }

    public void OpenLeaderBoard()
    {
        leaderboard.SetActive(true);
        stats.SetActive(false);
    }

    public void OpenMainMenu()
    {
        leaderboard.SetActive(false);
        stats.SetActive(true);
    }

}
