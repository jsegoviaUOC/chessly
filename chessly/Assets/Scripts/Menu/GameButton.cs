using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    // Inici del joc
    public void StartGame()
    {
        // S'inicia l'escena del joc
        SceneManager.LoadScene("Game");
    }

    // funció per tancar le joc
    public void QuitGame()
    {
        // Es tanca el joc
        Application.Quit();
    }

}
