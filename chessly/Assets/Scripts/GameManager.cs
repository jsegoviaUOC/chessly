using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Definició del tauler
    public Board mBoard;

    // Definició del gestor de peces
    public PieceManager mPieceManager;

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

    // S'executa al iniciar l'escena
    void Start()
    {
        // Definició del tipus de joc
        typeGame();

        // Carrega les opcions sel·leccionades
        OptionsData();

        // Carrega les traduccions dels textos
        LanguageManager.ApplyLanguageData();

        // Creació del tauler
        mBoard.Create(xAxis, yAxis);

        // Creació de les peces
        mPieceManager.Setup(mBoard);
    }

    // Funció per definir el tipus de joc
    public void typeGame()
    {
        
        switch (GameButton.typeGame)
        {
            case 1:// Joc Random (NonClassic)

                xMargin = Random.Range(0, 3);
                xAxis = 8 + xMargin * 2;
                yAxis = Random.Range(5, 8);

                // Es cetra el board en la pantalla
                mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(300 - (xMargin*100), mBoard.GetComponent<RectTransform>().offsetMin.y);
                mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(mBoard.GetComponent<RectTransform>().offsetMin.x, 50 + (8- yAxis)*50);

                // Array de l'ordre de les peces en el joc random
                pieceRand = Random.Range(0, 5);

                for (int i = 0; i < 16; i++)
                {
                    mPieces[i] = mTypePieces[pieceRand];
                    pieceRand = Random.Range(0, 5);
                }

                // Es força el rei en la cel·la estàndar
                mPieces[12] = "K";

                // reinicio el color del Non-Player al default
                NPColor = Color.red;

                break;
            case 2:// Joc clàssic contra l'ordinador

                xMargin = 0;
                xAxis = 8;
                yAxis = 8;

                NPColorRand = Random.Range(0, 2);
                NPColor = NPColorRand == 1 ? Color.white : Color.black;

                // Array de l'ordre de les peces en el joc classic
                mPieces = new string[16]
                {
                    "P", "P", "P", "P", "P", "P", "P", "P",
                    "T", "KN", "B", "Q", "K", "B", "KN", "T"
                };

                break;
            case 3:// Joc Random (NonClassic)contra l'ordinador

                xMargin = Random.Range(0, 3);
                xAxis = 8 + xMargin * 2;
                yAxis = Random.Range(5, 8);

                // Es cetra el board en la pantalla
                mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(300 - (xMargin * 100), mBoard.GetComponent<RectTransform>().offsetMin.y);
                mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(mBoard.GetComponent<RectTransform>().offsetMin.x, 50 + (8 - yAxis) * 50);

                // Array de l'ordre de les peces en el joc random
                pieceRand = Random.Range(0, 5);

                for (int i = 0; i < 16; i++)
                {
                    mPieces[i] = mTypePieces[pieceRand];
                    pieceRand = Random.Range(0, 5);
                }

                NPColorRand = Random.Range(0, 2);
                NPColor = NPColorRand == 1 ? Color.white : Color.black;

                // Es força el rei en la cel·la estàndar
                mPieces[12] = "K";

                break;
            default:// Joc clàssic

                xMargin = 0;
                xAxis = 8;
                yAxis = 8;

                // Array de l'ordre de les peces en el joc classic
                mPieces = new string[16]
                {
                    "P", "P", "P", "P", "P", "P", "P", "P",
                    "T", "KN", "B", "Q", "K", "B", "KN", "T"
                };

                // reinicio el color del Non-Player al default
                NPColor = Color.red;

                break;
        }
        
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

}
