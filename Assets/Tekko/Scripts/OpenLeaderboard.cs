using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLeaderboard : MonoBehaviour
{
    public GameObject leaderboard;
    public GameObject stats;

    public void Click()
    {
        stats.SetActive(false);
        leaderboard.SetActive(true);
    }
}
