using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager
{
    // Objecte amb les dades guardades
    public static OptionsData optionsData;

    public static LanguagesData languageData;

    // OptionsJson paths
    public const string path = "Data";
    public const string optionsFileName = "options";

    // Funció per carregar la informació guardada de les opcions
    private static void OptionsData()
    {
        // Es buscar l'arxiu amb les opcions
        var dataFound = SaveLoadData.LoadData<OptionsData>(path, optionsFileName);

        // Si es troba es carreguen les
        if (dataFound != null)
        {
            optionsData = dataFound;
        }
        else // Si no, es carreguen les dades per default i en guarden en el fitxer option.json
        {
            var dataOpt = Resources.Load<TextAsset>("defaultOptions.json");
            optionsData = JsonUtility.FromJson<OptionsData>(dataOpt.ToString());
            SaveLoadData.SaveData<OptionsData>(optionsData, path, optionsFileName);
        }
    }

    // Funció per carregar els textos del idioma
    private static void LoadLanguageData()
    {
        var dataLang = Resources.Load<TextAsset>("Languages/" + optionsData.language);

        languageData = JsonUtility.FromJson<LanguagesData>(dataLang.ToString());
    }

    // Funció per a aplicar els textos
    public static void ApplyLanguageData()
    {
        OptionsData();
        LoadLanguageData();

        // Botons d'accés
        if (GameObject.Find("ClassicButton")) { GameObject.Find("ClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.ClassicButton; }
        if (GameObject.Find("NonClassicButton")) { GameObject.Find("NonClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.NonClassicButton; }
        if (GameObject.Find("QuitButton")) { GameObject.Find("QuitButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.QuitButton; }
        if (GameObject.Find("ReturnMenuButton")) { GameObject.Find("ReturnMenuButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.ReturnMenuButton; }
        if (GameObject.Find("OptionsButton")) { GameObject.Find("OptionsButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.OptionsButton; }

        // Botons d'idiomes
        if (GameObject.Find("LangEngButton")) { GameObject.Find("LangEngButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.LangEngButton; }
        if (GameObject.Find("LangCatButton")) { GameObject.Find("LangCatButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.LangCatButton; }

        // Textos de les opcions
        if (GameObject.Find("SelectLanguageText")) { GameObject.Find("SelectLanguageText").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.SelectLanguageText; }
        if (GameObject.Find("WhiteColorText")) { GameObject.Find("WhiteColorText").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.SelectWhitePiecesColorText; }
        if (GameObject.Find("BlackColorText")) { GameObject.Find("BlackColorText").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.SelectBlackPiecesColorText; }
        if (GameObject.Find("BoardColorText")) { GameObject.Find("BoardColorText").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.SelectBoardColorText; }

        //Textos opcions color peces blanques
        if (GameObject.Find("PWButton")) { GameObject.Find("PWButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.whitePiecesColor.PW; }
        if (GameObject.Find("IVButton")) { GameObject.Find("IVButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.whitePiecesColor.IV; }
        if (GameObject.Find("SBButton")) { GameObject.Find("SBButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.whitePiecesColor.SB; }

        //Textos opcions color peces blanques
        if (GameObject.Find("PBButton")) { GameObject.Find("PBButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.blackPiecesColor.PB; }
        if (GameObject.Find("EBButton")) { GameObject.Find("EBButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.blackPiecesColor.EB; }
        if (GameObject.Find("DRButton")) { GameObject.Find("DRButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.blackPiecesColor.DR; }

        //Textos opcions color peces blanques
        if (GameObject.Find("BWButton")) { GameObject.Find("BWButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.boardColor.BW; }
        if (GameObject.Find("WCButton")) { GameObject.Find("WCButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.boardColor.WC; }
        if (GameObject.Find("NEButton")) { GameObject.Find("NEButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.boardColor.NE; }

    }

}
