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

    public static LanguagesData languageData;

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
            
            var dataOpt = Resources.Load<TextAsset>( "defaultOptions.json");

            optionsData = JsonUtility.FromJson<OptionsData>(dataOpt.ToString());

            SaveLoadData.SaveData<OptionsData>(optionsData, path, optionsFileName);
        }
    }

    private void SelectedOptionButtons()
    {
        // Es deshabilita el botó del idioma sel·leccionat
        if (optionsData.language == "en") {
            GameObject.Find("LangEngButton").GetComponent<Button>().interactable = false;
            GameObject.Find("LangCatButton").GetComponent<Button>().interactable = true;
        }
        else
        {
            GameObject.Find("LangEngButton").GetComponent<Button>().interactable = true;
            GameObject.Find("LangCatButton").GetComponent<Button>().interactable = false;
        }

        // Es deshabilita el botó del color de les peces balques sel·leccionat
        switch (optionsData.colors.whitePiecesColor)
        {
            case "IV":
                GameObject.Find("PWButton").GetComponent<Button>().interactable = true;
                GameObject.Find("IVButton").GetComponent<Button>().interactable = false;
                GameObject.Find("SBButton").GetComponent<Button>().interactable = true;
                break;
            case "SB":
                GameObject.Find("PWButton").GetComponent<Button>().interactable = true;
                GameObject.Find("IVButton").GetComponent<Button>().interactable = true;
                GameObject.Find("SBButton").GetComponent<Button>().interactable = false;
                break;
            default:
                GameObject.Find("PWButton").GetComponent<Button>().interactable = false;
                GameObject.Find("IVButton").GetComponent<Button>().interactable = true;
                GameObject.Find("SBButton").GetComponent<Button>().interactable = true;
                break;
        }

        // Es deshabilita el botó del color de les peces negres sel·leccionat
        switch (optionsData.colors.blackPiecesColor)
        {
            case "EB":
                GameObject.Find("PBButton").GetComponent<Button>().interactable = true;
                GameObject.Find("EBButton").GetComponent<Button>().interactable = false;
                GameObject.Find("DRButton").GetComponent<Button>().interactable = true;
                break;
            case "DR":
                GameObject.Find("PBButton").GetComponent<Button>().interactable = true;
                GameObject.Find("EBButton").GetComponent<Button>().interactable = true;
                GameObject.Find("DRButton").GetComponent<Button>().interactable = false;
                break;
            default:
                GameObject.Find("PBButton").GetComponent<Button>().interactable = false;
                GameObject.Find("EBButton").GetComponent<Button>().interactable = true;
                GameObject.Find("DRButton").GetComponent<Button>().interactable = true;
                break;
        }

        // Es deshabilita el botó del color del tauler sel·leccionat
        switch (optionsData.colors.boardColor)
        {
            case "WC":
                GameObject.Find("BWButton").GetComponent<Button>().interactable = true;
                GameObject.Find("WCButton").GetComponent<Button>().interactable = false;
                GameObject.Find("NEButton").GetComponent<Button>().interactable = true;
                break;
            case "NE":
                GameObject.Find("BWButton").GetComponent<Button>().interactable = true;
                GameObject.Find("WCButton").GetComponent<Button>().interactable = true;
                GameObject.Find("NEButton").GetComponent<Button>().interactable = false;
                break;
            default:
                GameObject.Find("BWButton").GetComponent<Button>().interactable = false;
                GameObject.Find("WCButton").GetComponent<Button>().interactable = true;
                GameObject.Find("NEButton").GetComponent<Button>().interactable = true;
                break;
        }
    }
}
