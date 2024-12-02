using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loading;
    public TMP_InputField nick;
    public PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("PlayerStats").GetComponent<PlayerStats>();
    }
    public void StartGame()
    {
        if(nick.text.Length >= 5)
        {
            playerStats.playerName = nick.text;
            
            StartCoroutine(StartGameFonk());
            
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    IEnumerator StartGameFonk()
    {
        mainMenu.SetActive(false);
        loading.gameObject.SetActive(true);
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        
        yield return null;
        loading.gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
