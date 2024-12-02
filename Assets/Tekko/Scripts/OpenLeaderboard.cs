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

    public void Click()
    {
        
        leaderboard.SetActive(true);
    }
}
