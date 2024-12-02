using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject loading;
    
    public void StartGame()
    {
        StartGameFonk();
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
    private IEnumerator StartGameFonk()
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
