using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    // Controla el tipus de partida
    public static int typeGame;

    // Inici del joc
    public void StartGame(int type)
    {
        // S'inicialitza el tipus de partida
        typeGame = type;

        // S'inicia l'escena del joc
        SceneManager.LoadScene("Game");
    }

    // funció per tancar le joc
    public void QuitGame()
    {
        // Es tanca el joc
        Application.Quit();
    }

    // Retorn al menu principal
    public void BackMenu()
    {
        // Carrega la escena del menu principal
        SceneManager.LoadScene("Main Menu");
    }

    // funció canviar idioma 
    public void ChangeLanguage( string language)
    {
        // Canvi de text segons idioma
        if (language == "ca")
        {
            GameObject.Find("ClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Joc Clàssic";
            GameObject.Find("NonClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Joc a l'Atzar";
            GameObject.Find("QuitButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Sortir";
            GameObject.Find("ReturnMenuButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Tornar al Menú";
        }
        else if(language == "en")
        {
            GameObject.Find("ClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Classic Game";
            GameObject.Find("NonClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Random Game";
            GameObject.Find("QuitButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Exit";
            GameObject.Find("ReturnMenuButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Return to Menu";
        }
    }

}
