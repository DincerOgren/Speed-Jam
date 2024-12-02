using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI textPro;

    float timer;

    // Update is called once per frame
    void Update()
    {

        if (Input.anyKeyDown)
        {

            timer += Time.deltaTime;

            // Calculate minutes, seconds, and milliseconds
            int minutes = Mathf.FloorToInt(timer / 60); // Get total minutes
            int seconds = Mathf.FloorToInt(timer % 60); // Get remaining seconds
            int milliseconds = Mathf.FloorToInt((timer * 1000) % 1000); // Get milliseconds

            // Format the timer as 00:00:00
            string formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);

            // Update the TextMeshPro text
            textPro.text = formattedTime;
        }

    }
}
