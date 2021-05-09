using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameButton : MonoBehaviour
{
    // Controla el tipus de partida
    public static int typeGame;

    // Objecte amb les dades guardades
    public static OptionsData optionsData;

    // OptionsJson paths
    public const string path = "Data";
    public const string optionsFileName = "options";

    public void Start()
    {
        // Deshabilita el menú d'opcions
        GameObject.Find("OptionsMenu").GetComponent<Canvas>().enabled = false;

        OptionsData();

        LanguageManager.ApplyLanguageData();

        SelectedOptionButtons();
    }
    
    // Inici del joc
    public void StartGame(int type)
    {
        // S'inicialitza el tipus de partida
        typeGame = type;

        // S'inicia l'escena del joc
        SceneManager.LoadScene("Game");
    }

    // Inici de l'editor de taulers
    public void StartEditor(int type)
    {
        // S'inicialitza el tipus de partida
        typeGame = type;

        // S'inicia l'escena del joc
        SceneManager.LoadScene("BoardEditor");
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

    // funció per accedir al menu d'opcions
    public void OptionMenu()
    {
        // Tanca el menú principal
        GameObject.Find("Main Menu").GetComponent<Canvas>().enabled = false;

        // Obre el menú d'opcions
        GameObject.Find("OptionsMenu").GetComponent<Canvas>().enabled = true;
    }

    // Funció canviar idioma 
    public void ChangeLanguage(string newLanguage)
    {
        // S'actualitza l'idioma sel·leccionat
        optionsData.language = newLanguage;

        // Es guarda l'idioma sel·leccionat en el fitxer option.json
        SaveLoadData.SaveData<OptionsData>(optionsData, path, optionsFileName);

        // Aplica un canvi visual al botó d'idioma
        SelectedOptionButtons();

        // Es carrega la informació de l'idioma sel·leccionat i s'aplica
        LanguageManager.ApplyLanguageData();
    }

    // Funció canviar els colors 
    public void ChangeColor(string colorSlug)
    {
        // S'actualitza el color sel·leccionat
        switch (colorSlug)
        {
            case "PW":
                optionsData.colors.whitePiecesColor = "PW";
                break;
            case "IV":
                optionsData.colors.whitePiecesColor = "IV";
                break;
            case "SB":
                optionsData.colors.whitePiecesColor = "SB";
                break;
            case "PB":
                optionsData.colors.blackPiecesColor = "PB";
                break;
            case "EB":
                optionsData.colors.blackPiecesColor = "EB";
                break;
            case "DR":
                optionsData.colors.blackPiecesColor = "DR";
                break;
            case "BW":
                optionsData.colors.boardColor = "BW";
                break;
            case "WC":
                optionsData.colors.boardColor = "WC";
                break;
            case "NE":
                optionsData.colors.boardColor = "NE";
                break;
        }

        // Es guarda l'idioma sel·leccionat en el fitxer option.json
        SaveLoadData.SaveData<OptionsData>(optionsData, path, optionsFileName);

        // Aplica un canvi visual al botó d'idioma
        SelectedOptionButtons();

    }

    // Funció per carregar la informació guardada de les opcions
    private void OptionsData()
    {
        // Es buscar l'arxiu amb les opcions
        var dataFound = SaveLoadData.LoadData<OptionsData>(path, optionsFileName);

        // Si es troba es carreguen les
        if(dataFound != null)
        {
            optionsData = dataFound;
        }
        else // Si no, es carreguen les dades per default i en guarden en el fitxer option.json
        {
            
            var dataOpt = Resources.Load<TextAsset>( "defaultOptions");

            optionsData = JsonUtility.FromJson<OptionsData>(dataOpt.ToString());

            SaveLoadData.SaveData<OptionsData>(optionsData, path, optionsFileName);
        }
    }

    private void SelectedOptionButtons()
    {
        // Es deshabilita el botó del idioma sel·leccionat
        foreach (Button button in GameObject.Find("Languages").GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }

        if (optionsData.language == "en") {
            GameObject.Find("LangEngButton").GetComponent<Button>().interactable = false;
        }
        else
        {
            GameObject.Find("LangCatButton").GetComponent<Button>().interactable = false;
        }

        // Es deshabilita el botó del color de les peces balques sel·leccionat
        foreach (Button button in GameObject.Find("ColorWhitePiecesOption").GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }

        switch (optionsData.colors.whitePiecesColor)
        {
            case "IV":
                GameObject.Find("IVButton").GetComponent<Button>().interactable = false;
                break;
            case "SB":
                GameObject.Find("SBButton").GetComponent<Button>().interactable = false;
                break;
            default:
                GameObject.Find("PWButton").GetComponent<Button>().interactable = false;
                break;
        }

        // Es deshabilita el botó del color de les peces negres sel·leccionat
        foreach (Button button in GameObject.Find("ColorBlackPiecesOption").GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }

        switch (optionsData.colors.blackPiecesColor)
        {
            case "EB":
                GameObject.Find("EBButton").GetComponent<Button>().interactable = false;
                break;
            case "DR":
                GameObject.Find("DRButton").GetComponent<Button>().interactable = false;
                break;
            default:
                GameObject.Find("PBButton").GetComponent<Button>().interactable = false;
                break;
        }

        // Es deshabilita el botó del color del tauler sel·leccionat
        foreach (Button button in GameObject.Find("ColorBoardOption").GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }

        switch (optionsData.colors.boardColor)
        {
            case "WC":
                GameObject.Find("WCButton").GetComponent<Button>().interactable = false;
                break;
            case "NE":
                GameObject.Find("NEButton").GetComponent<Button>().interactable = false;
                break;
            default:
                GameObject.Find("BWButton").GetComponent<Button>().interactable = false;
                break;
        }
    }

    public static Color32 getColor(string colorSlug)
    {
        // S'actualitza el color sel·leccionat
        switch (colorSlug)
        {
            case "PW":
                return new Color32(239, 239, 239, 255);
            case "IV":
                return new Color32(238, 223, 170, 255);
            case "SB":
                return new Color32(73, 157, 220, 255);
            case "PB":
                return new Color32(31, 30, 24, 255);
            case "EB":
                return new Color32(38, 38, 53, 255);
            case "DR":
                return new Color32(113, 29, 29, 255);
            case "BWDark":
                return new Color32(95, 95, 95, 255);
            case "BWLight":
                return new Color32(143, 143, 143, 255);
            case "WCDark":
                return new Color32(118, 65, 23, 255);
            case "WCLight":
                return new Color32(197, 108, 52, 255);
            case "NEDark":
                return new Color32(97, 47, 177, 255);
            case "NELight":
                return new Color32(127, 177, 47, 255);
            case "void":
                return new Color32(255, 255, 255, 0);
            default:
                return new Color32(255, 255, 255, 255);
        }

    }
}
