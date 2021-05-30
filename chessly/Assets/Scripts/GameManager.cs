using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    // Definició del tauler
    public Board mBoard;

    // Definició del gestor de peces
    public PieceManager mPieceManager;

    // Recuadre de text amb informació de la partida
    public GameObject textDisplayCanvas;

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
    public static Color yourColor;

    // indicador de si la partida és online
    public static bool isOnline;
    public static int idGame;

    // S'executa al iniciar l'escena
    void Start()
    {
        // Carrega les opcions sel·leccionades
        OptionsData();

        // Carrega les traduccions dels textos
        LanguageManager.ApplyLanguageData();

        // Es deshabilita el recuadre informatiu de la partida
        textDisplayCanvas.GetComponent<Canvas>().enabled = false;
        
        // Definició del tipus de joc
        TypeGame();
    }

    void Run()
    { 
        // Creació del tauler
        mBoard.Create(xAxis, yAxis);

        // Creació de les peces
        mPieceManager.Setup(mBoard);
    }

    // Funció per definir el tipus de joc
    public void TypeGame()
    {
        isOnline = false;

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

                Run();
                break;
            case 2:// Joc clàssic contra l'ordinador

                xMargin = 0;
                xAxis = 8;
                yAxis = 8;

                // Sel·lcció aleatotia del jugador inicial
                NPColorRand = Random.Range(0, 2);
                if(NPColorRand == 1)
                {
                    NPColor = Color.white;
                    yourColor = Color.black;
                }
                else
                {
                    yourColor = Color.white;
                    NPColor = Color.black;
                }

                // Ordre de les peces en el joc classic
                SetClassicPieces();

                Run();
                break;
            case 3:// Joc Random (NonClassic) contra l'ordinador

                xMargin = Random.Range(0, 3);
                xAxis = 8 + xMargin * 2;
                yAxis = Random.Range(5, 8);

                // Es cetra el board en la pantalla
                mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(300 - (xMargin * 100), mBoard.GetComponent<RectTransform>().offsetMin.y);
                mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(mBoard.GetComponent<RectTransform>().offsetMin.x, 50 + (8 - yAxis) * 50);

                // Ordre de les peces en el joc random
                SetRandomPieces();

                NPColorRand = Random.Range(0, 2);
                if (NPColorRand == 1)
                {
                    NPColor = Color.white;
                    yourColor = Color.black;
                }
                else
                {
                    yourColor = Color.white;
                    NPColor = Color.black;
                }

                Run();
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

                Run();
                break;
            case 5:// Joc Editat 2P

                xAxis = EditorManager.xAxis;
                yAxis = EditorManager.yAxis;
                xMargin = (xAxis - 8) / 2;

                // reinicio el color del Non-Player al default
                NPColor = Color.red;

                // Ordre de les peces en el joc classic
                matrixPieces = EditorManager.matrixPiecesEditor;

                Run();
                break;
            case 6: //partida online

                NPColor = Color.red;
                idGame = Login.idNewGame;

                StartCoroutine(GetGame());

                break;
            default:// Joc clàssic

                xMargin = 0;
                xAxis = 8;
                yAxis = 8;

                // Ordre de les peces en el joc classic
                SetClassicPieces();

                // reinicio el color del Non-Player al default
                NPColor = Color.red;

                Run();
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
        if (isOnline)
        {
            // Carrega la escena de Login
            SendExitGame();
            SceneManager.LoadScene("Login");
        }
        else
        {
            // Carrega la escena del menu principal
            SceneManager.LoadScene("Main Menu");
        }
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

    //intancia la matriu de peces random
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

    /* FUNCIONS GAME ONLINE */

    public void SendExitGame()
    {
        StartCoroutine(ExitGame());
    }

    // funció per a get game
    public IEnumerator GetGame()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://18.116.223.113/api/game/" + Login.idNewGame);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var json = JsonUtility.FromJson<OnlineGameData>(www.downloadHandler.text);

            xAxis = json.data.xAxis;
            yAxis = json.data.yAxis;
            xMargin = (xAxis - 8) / 2;

            matrixPieces = new VoidPiece[xAxis, yAxis];
            
            var base64EncodedBytes = System.Convert.FromBase64String(json.data.pieces);
            string[] piecesString = System.Text.Encoding.UTF8.GetString(base64EncodedBytes).Split(',');

            foreach (string piece in piecesString)
            {
                string[] infoPiece = piece.Split('-');

                VoidPiece vP = new VoidPiece();
                vP.color = infoPiece[3];
                vP.type = infoPiece[2];

                matrixPieces[int.Parse(infoPiece[0]), int.Parse(infoPiece[1])] = vP;
            }

            if (json.data.colorCreator == "B")
            {
                yourColor = json.data.creatorId == Login.idPlayer ? Color.black: Color.white;
            }
            else
            {
                yourColor = json.data.creatorId == Login.idPlayer ? Color.white : Color.black;
            }

            isOnline = true;

            Run();
        }
    }

    public static IEnumerator ExitGame()
    {
        WWWForm form = new WWWForm();
        form.AddField("player_id", Login.idPlayer);

        UnityWebRequest www = UnityWebRequest.Post("http://18.116.223.113/api/game/" + Login.idNewGame + "/exit/" + Login.idPlayer, form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
        }
    }
}
