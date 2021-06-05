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

    // Get dels textos
    public static LanguagesData getLanguageText()
    {
        return languageData;
    }

    // Funció per a aplicar els textos
    public static void ApplyLanguageData()
    {
        OptionsData();
        LoadLanguageData();

        /*
         * Traduccions Menú 
         */
        // Textos del menú principal
        if (GameObject.Find("OnePlayerSection")) { GameObject.Find("OnePlayerSection").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.OnePlayerText; }
        if (GameObject.Find("TwoPlayersSection")) { GameObject.Find("TwoPlayersSection").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.TwoPlayersText; }

        // Botons d'accés
        if (GameObject.Find("ClassicNPButton")) { GameObject.Find("ClassicNPButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.ClassicNPButton; }
        if (GameObject.Find("NonClassicNPButton")) { GameObject.Find("NonClassicNPButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.NonClassicNPButton; }
        if (GameObject.Find("EditorNPButton")) { GameObject.Find("EditorNPButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.EditorNPButton; }
        if (GameObject.Find("ClassicButton")) { GameObject.Find("ClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.ClassicButton; }
        if (GameObject.Find("NonClassicButton")) { GameObject.Find("NonClassicButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.NonClassicButton; }
        if (GameObject.Find("EditorButton")) { GameObject.Find("EditorButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.EditorButton; }
        if (GameObject.Find("QuitButton")) { GameObject.Find("QuitButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.QuitButton; }
        if (GameObject.Find("ReturnMenuButton")) { GameObject.Find("ReturnMenuButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.ReturnMenuButton; }
        if (GameObject.Find("OptionsButton")) { GameObject.Find("OptionsButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.OptionsButton; }
        if (GameObject.Find("LoginButton")) { GameObject.Find("LoginButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.general.LoginButton; }

        // Botons d'idiomes
        if (GameObject.Find("LangEngButton")) { GameObject.Find("LangEngButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.LangEngButton; }
        if (GameObject.Find("LangCatButton")) { GameObject.Find("LangCatButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.LangCatButton; }

        // Textos de les opcions
        if (GameObject.Find("SelectLanguageText")) { GameObject.Find("SelectLanguageText").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.SelectLanguageText; }
        if (GameObject.Find("WhiteColorText")) { GameObject.Find("WhiteColorText").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.SelectWhitePiecesColorText; }
        if (GameObject.Find("BlackColorText")) { GameObject.Find("BlackColorText").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.SelectBlackPiecesColorText; }
        if (GameObject.Find("BoardColorText")) { GameObject.Find("BoardColorText").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.SelectBoardColorText; }

        // Textos opcions color peces blanques
        if (GameObject.Find("PWButton")) { GameObject.Find("PWButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.whitePiecesColor.PW; }
        if (GameObject.Find("IVButton")) { GameObject.Find("IVButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.whitePiecesColor.IV; }
        if (GameObject.Find("SBButton")) { GameObject.Find("SBButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.whitePiecesColor.SB; }

        // Textos opcions color peces blanques
        if (GameObject.Find("PBButton")) { GameObject.Find("PBButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.blackPiecesColor.PB; }
        if (GameObject.Find("EBButton")) { GameObject.Find("EBButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.blackPiecesColor.EB; }
        if (GameObject.Find("DRButton")) { GameObject.Find("DRButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.blackPiecesColor.DR; }

        // Textos opcions color peces blanques
        if (GameObject.Find("BWButton")) { GameObject.Find("BWButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.boardColor.BW; }
        if (GameObject.Find("WCButton")) { GameObject.Find("WCButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.boardColor.WC; }
        if (GameObject.Find("NEButton")) { GameObject.Find("NEButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.options.boardColor.NE; }

        /*
         * Traduccions menu Login
         */
        if (GameObject.Find("LabelUsername")) { GameObject.Find("LabelUsername").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.LabelUsername; }
        if (GameObject.Find("PlaceholderUsername")) { GameObject.Find("PlaceholderUsername").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.PlaceholderUsername; }
        if (GameObject.Find("LabelPassword")) { GameObject.Find("LabelPassword").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.LabelPassword; }
        if (GameObject.Find("PlaceholderPassword")) { GameObject.Find("PlaceholderPassword").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.PlaceholderPassword; }

        // Textos botons
        if (GameObject.Find("SignInButton")) { GameObject.Find("SignInButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.SignInButton; }
        if (GameObject.Find("SignUpButton")) { GameObject.Find("SignUpButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.SignUpButton; }
        if (GameObject.Find("SearchGameButton")) { GameObject.Find("SearchGameButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.SearchGameButton; }
        if (GameObject.Find("CreateGameButton")) { GameObject.Find("CreateGameButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.CreateGameButton; }
        if (GameObject.Find("StatisticsButton")) { GameObject.Find("StatisticsButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.StatisticsButton; }
        if (GameObject.Find("LogOutButton")) { GameObject.Find("LogOutButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.LogOutButton; }
        if (GameObject.Find("ClassicGameButton")) { GameObject.Find("ClassicGameButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.ClassicGameButton; }
        if (GameObject.Find("CustomGameButton")) { GameObject.Find("CustomGameButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.CustomGameButton; }
        if (GameObject.Find("BackToSelectButton")) { GameObject.Find("BackToSelectButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.buttons.BackToSelectButton; }

        // Estadístiques
        if (GameObject.Find("TotalWinsLabel")) { GameObject.Find("TotalWinsLabel").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.menu.login.statistics.TotalWinsLabel; }
        if (GameObject.Find("CreatedGamesLabel")) { GameObject.Find("CreatedGamesLabel").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.menu.login.statistics.CreatedGamesLabel; }
        if (GameObject.Find("SearchedGamesLabel")) { GameObject.Find("SearchedGamesLabel").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.menu.login.statistics.SearchedGamesLabel; }
        if (GameObject.Find("ColorPiecesLabel")) { GameObject.Find("ColorPiecesLabel").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.menu.login.statistics.ColorPiecesLabel; }
        if (GameObject.Find("TotalMovesLabel")) { GameObject.Find("TotalMovesLabel").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.menu.login.statistics.TotalMovesLabel; }
        if (GameObject.Find("TotalWinsValue")) { GameObject.Find("TotalWinsValue").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.menu.login.statistics.TotalWinsValue; }


        /*
         * Traduccions Joc 
         */
        // Textos botons del joc
        if (GameObject.Find("ReturnButton")) { GameObject.Find("ReturnButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.game.buttons.ReturnButton; }
        if (GameObject.Find("QuitTextButton")) { GameObject.Find("QuitTextButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.game.buttons.OkButton; }

        // Textos joc online
        if (GameObject.Find("Player2")) { GameObject.Find("Player2").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.game.info.WaitingText; }
        if (GameObject.Find("Player1")) { GameObject.Find("Player1").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.game.info.WaitingText; }

        /*
         * Traduccions editor de taulers
         */
        if (GameObject.Find("PawnButton")) { GameObject.Find("PawnButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.pieces.Pawn; }
        if (GameObject.Find("TowerButton")) { GameObject.Find("TowerButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.pieces.Tower; }
        if (GameObject.Find("BishopButton")) { GameObject.Find("BishopButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.pieces.Bishop; }
        if (GameObject.Find("KnigthButton")) { GameObject.Find("KnigthButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.pieces.Knigth; }
        if (GameObject.Find("QueenButton")) { GameObject.Find("QueenButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.pieces.Queen; }
        if (GameObject.Find("KingButton")) { GameObject.Find("KingButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.pieces.King; }
        if (GameObject.Find("WhiteButton")) { GameObject.Find("WhiteButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.colors.White; }
        if (GameObject.Find("BlackButton")) { GameObject.Find("BlackButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.colors.Black; }
        if (GameObject.Find("GameButton")) { GameObject.Find("GameButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.buttons.GameButton; }
        if (GameObject.Find("ReturnButton")) { GameObject.Find("ReturnButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.buttons.ReturnButton; }
        if (GameObject.Find("ColorButtonsTitle")) { GameObject.Find("ColorButtonsTitle").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.info.ColorButtonsTitle; }
        if (GameObject.Find("PieceButtonsTitle")) { GameObject.Find("PieceButtonsTitle").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.info.PieceButtonsTitle; }

        // Textos caixa d'error
        if (GameObject.Find("TitleErrorBox")) { GameObject.Find("TitleErrorBox").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.error.TitleErrorBox; }
        if (GameObject.Find("QuitErrorButton")) { GameObject.Find("QuitErrorButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.error.QuitErrorButton; }

        // Textos configurador del tauler
        if (GameObject.Find("TitleAxisBox")) { GameObject.Find("TitleAxisBox").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.TitleAxisBox; }
        if (GameObject.Find("LabelAxisX")) { GameObject.Find("LabelAxisX").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.LabelAxisX; }
        if (GameObject.Find("InformationAxisX")) { GameObject.Find("InformationAxisX").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.InformationAxisX; }
        if (GameObject.Find("LabelAxisY")) { GameObject.Find("LabelAxisY").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.LabelAxisY; }
        if (GameObject.Find("InformationAxisY")) { GameObject.Find("InformationAxisY").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.InformationAxisY; }
        if (GameObject.Find("PlaceholderAxisXEditor")) { GameObject.Find("PlaceholderAxisXEditor").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.PlaceholderEditor; }
        if (GameObject.Find("PlaceholderAxisYEditor")) { GameObject.Find("PlaceholderAxisYEditor").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.PlaceholderEditor; }
        if (GameObject.Find("ApplyAxisButton")) { GameObject.Find("ApplyAxisButton").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.ApplyAxisButton; }
        if (GameObject.Find("ErrorAxisBox")) { GameObject.Find("ErrorAxisBox").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.editor.conf.ErrorAxisBox; }

        // Textos zona de jugador
        if (GameObject.Find("WhitePlayerText")) { GameObject.Find("WhitePlayerText").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.editor.info.WhitePlayerText; }
        if (GameObject.Find("BlackPlayerText")) { GameObject.Find("BlackPlayerText").GetComponent<TMPro.TextMeshProUGUI>().text = languageData.editor.info.BlackPlayerText; }

    }

}
