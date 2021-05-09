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

    public static VoidPiece[,] matrixPieces;

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
        GameButton.OptionsData();

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

                // Ordre de les peces en el joc random
                SetRandomPieces();

                // reinicio el color del Non-Player al default
                NPColor = Color.red;

                break;
            case 2:// Joc clàssic contra l'ordinador

                xMargin = 0;
                xAxis = 8;
                yAxis = 8;

                // Sel·lcció aleatotia del jugador inicial
                NPColorRand = Random.Range(0, 2);
                NPColor = NPColorRand == 1 ? Color.white : Color.black;

                // Ordre de les peces en el joc classic
                SetClassicPieces();

                break;
            case 3:// Joc Random (NonClassic)contra l'ordinador

                xMargin = Random.Range(0, 3);
                xAxis = 8 + xMargin * 2;
                yAxis = Random.Range(5, 8);

                // Es cetra el board en la pantalla
                mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(300 - (xMargin * 100), mBoard.GetComponent<RectTransform>().offsetMin.y);
                mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(mBoard.GetComponent<RectTransform>().offsetMin.x, 50 + (8 - yAxis) * 50);

                // Ordre de les peces en el joc random
                SetRandomPieces();

                NPColorRand = Random.Range(0, 2);
                NPColor = NPColorRand == 1 ? Color.white : Color.black;
                
                break;
            case 4:// Joc Editat contra l'ordinador

                xAxis = EditorManager.xAxis;
                yAxis = EditorManager.yAxis;
                xMargin = (xAxis - 8) / 2;

                // Sel·lcció aleatotia del jugador inicial
                NPColorRand = Random.Range(0, 2);
                NPColor = NPColorRand == 1 ? Color.white : Color.black;

                // Ordre de les peces en el joc classic
                matrixPieces = EditorManager.matrixPiecesEditor;

                break;
            case 5:// Joc Editat 2P

                xAxis = EditorManager.xAxis;
                yAxis = EditorManager.yAxis;
                xMargin = (xAxis - 8) / 2;

                // reinicio el color del Non-Player al default
                NPColor = Color.red;

                // Ordre de les peces en el joc classic
                matrixPieces = EditorManager.matrixPiecesEditor;

                break;
            default:// Joc clàssic

                xMargin = 0;
                xAxis = 8;
                yAxis = 8;

                // Ordre de les peces en el joc classic
                SetClassicPieces();

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

    private void SetClassicPieces()
    {
        string[] listPiecesClassic = new string[] {
            "T", "KN", "B", "Q", "K", "B", "KN", "T"
        };

        string[] colorsClassic = new string[] { "W", "B" };

        matrixPieces = new VoidPiece[xAxis, yAxis];

        for (int i = 0; i< xAxis; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                VoidPiece vP = new VoidPiece();
                vP.Setup(listPiecesClassic[i], colorsClassic[j]);

                matrixPieces[i, j * 7] = vP;

                VoidPiece vPP = new VoidPiece();
                vPP.Setup("P", colorsClassic[j]);

                matrixPieces[i, 5 * j + 1 ] = vPP;
            }
        }
       
    }

    private void SetRandomPieces()
    {
        int piecesInLine = 8;
        string[] listFrontPiecesClassic = new string[piecesInLine];
        string[] listBackPiecesClassic = new string[piecesInLine];

        for (int i = 0; i < listBackPiecesClassic.Length; i++)
        {
            pieceRand = Random.Range(0, 5);
            listFrontPiecesClassic[i] = mTypePieces[pieceRand];
            pieceRand = Random.Range(0, 5);
            listBackPiecesClassic[i] = mTypePieces[pieceRand];
        }

        string[] colorsClassic = new string[] { "W", "B" };

        matrixPieces = new VoidPiece[xAxis, yAxis];

        for (int i = 0; i < piecesInLine; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                VoidPiece vPF = new VoidPiece();
                vPF.Setup(listFrontPiecesClassic[i], colorsClassic[j]);

                matrixPieces[i + xMargin, j * ( yAxis - 1 )] = vPF;

                VoidPiece vPB = new VoidPiece();
                vPB.Setup(listBackPiecesClassic[i], colorsClassic[j]);

                matrixPieces[i + xMargin, (yAxis - 3) * j + 1] = vPB;
            }
        }

        int wheresKing = Random.Range(0, piecesInLine);

        VoidPiece KW = new VoidPiece();
        KW.Setup("K", "W");
        matrixPieces[wheresKing + xMargin, 0] = KW;

        VoidPiece KB = new VoidPiece();
        KB.Setup("K", "B");
        matrixPieces[wheresKing + xMargin, yAxis - 1] = KB;

    }

}
