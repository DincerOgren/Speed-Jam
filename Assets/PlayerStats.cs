using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    
    public string playerName;
    public int score = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
