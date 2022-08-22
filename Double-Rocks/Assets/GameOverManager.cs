using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject gameOverUI;

    public static GameOverManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }
        instance = this;
    }
    public void OnPlayerDeath()
    {
        gameOverUI.SetActive(true);
    }

    public void RetryButton()
    {
        //Recommencer le niveau
        //Recharger la scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Replace le joueur au spawn
        //Reactive les mouvements + lui rendre sa vie
        gameOverUI.SetActive(false);
    }

    public void MainMenu()
    {
        //Aller au menu principal
    }

    public void QuitButton()
    {
        //Quitter le jeu
        Application.Quit();
    }
}
