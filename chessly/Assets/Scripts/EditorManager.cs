using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EditorManager : MonoBehaviour
{
    // Definició del tauler
    public BoardEditor mBoard;

    // Definició del gestor de peces
    public PieceEditorManager mPieceManager;

    // Proporcions del tauler
    public int xAxis;
    public int yAxis;
    public static int xMargin;

    // Llistat de peces
    private string[] mTypePieces = new string[5]
    {
        "P","T", "KN", "B", "Q"
    };
    private static int pieceRand;

    // Array de l'ordre de les peces en el joc (pendent de fer el tamany dinàmic)
    public static string[] mPieces = new string[16];

    // Objecte amb les dades guardades
    public static OptionsData optionsData;

    // OptionsJson paths
    public const string path = "Data";
    public const string optionsFileName = "options";

    // Objecte per gestionar les traduccions
    public static LanguagesData languageData;

    // Color del jugador controlat per l'ordinador (Non-Player)
    private static int NPColorRand;
    public static Color NPColor = Color.red;

    public static string selectedPiece = "P";
    public static string selectedColor = "W";

    public static VoidPiece[,] matrixPiecesEditor;

    // S'executa al iniciar l'escena
    void Start()
    {
        xMargin = 0;
        xAxis = 8;
        yAxis = 8;

        matrixPiecesEditor = new VoidPiece[xAxis, yAxis];

        // Carrega les opcions sel·leccionades
        OptionsData();

        // Carrega les traduccions dels textos
        LanguageManager.ApplyLanguageData();

        // Creació del tauler
        mBoard.Create(xAxis, yAxis);

        // Creació de les peces
        mPieceManager.Setup(mBoard);

        GameObject.Find("PawnButton").GetComponent<Button>().interactable = false;
        GameObject.Find("WhiteButton").GetComponent<Button>().interactable = false;
    }

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

    // Retorn al menu principal
    public void BackMenu()
    {
        // Carrega la escena del menu principal
        SceneManager.LoadScene("Main Menu");
    }

    public void ChangeSelectedPiece(string piece)
    {
        foreach(Button button in GameObject.Find("PieceButtons").GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;

        selectedPiece = piece;
    }

    public void ChangeSelectedColor(string color)
    {
        foreach (Button button in GameObject.Find("ColorButtons").GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        selectedColor = color;
    }

    public void Game()
    {
        // S'inicia l'escena del joc
        SceneManager.LoadScene("Game");
    }
}
