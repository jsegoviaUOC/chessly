using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    public static int typeGame;

    // Inici del joc
    public void StartGame(int type)
    {
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

    // funció canviar idioma 
    public void ChangeLanguage( string language)
    {
        // Canvi de text segons idioma
        if (language == "ca")
        {
            GameObject.Find("ClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Joc Clàssic";
            GameObject.Find("NonClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Joc No Clàssic";
            GameObject.Find("QuitButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Sortir";
        }
        else if(language == "en")
        {
            GameObject.Find("ClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Classic Game";
            GameObject.Find("NonClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Non Classic Game";
            GameObject.Find("QuitButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Exit";
        }
    }

}
