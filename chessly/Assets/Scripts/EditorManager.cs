using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class EditorManager : MonoBehaviour
{
    // Definició del tauler
    public BoardEditor mBoard;

    // Definició del gestor de peces
    public PieceEditorManager mPieceManager;

    // Proporcions del tauler
    public static int xAxis;
    public static int yAxis;
    public static int xMargin;

    // Objecte amb les dades guardades
    public static OptionsData optionsData;

    // OptionsJson paths
    public const string path = "Data";
    public const string optionsFileName = "options";

    // Objecte per gestionar les traduccions
    public static LanguagesData languageData;

    // Color del jugador controlat per l'ordinador (Non-Player)
    public static Color NPColor = Color.red;

    public static string selectedPiece;
    public static string selectedColor;

    public static VoidPiece[,] matrixPiecesEditor;

    // S'executa al iniciar l'escena
    void Start()
    {
        // Carrega les opcions sel·leccionades
        OptionsData();

        // Carrega les traduccions dels textos
        LanguageManager.ApplyLanguageData();
        
        GameObject.Find("ErrorKingDisplayerCanvas").GetComponent<Canvas>().enabled = false;

        selectedPiece = "P";
        selectedColor = "W";

    }

    public void RunEditor()
    {

        matrixPiecesEditor = new VoidPiece[xAxis, yAxis];

        xMargin = (xAxis - 8) / 2;

        // Es cetra el board en la pantalla
        mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(480 - (xMargin * 80), mBoard.GetComponent<RectTransform>().offsetMin.y);
        mBoard.GetComponent<RectTransform>().offsetMin = new Vector2(mBoard.GetComponent<RectTransform>().offsetMin.x, 60 + (8 - yAxis) * 40);

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

    // Retorn al menu principal
    public void BackMenu()
    {
        // Carrega la escena del menu principal
        SceneManager.LoadScene("Main Menu");
    }

    // Canvia l'aspecte del button seleccionat
    public void ChangeSelectedPiece(string piece)
    {
        foreach(Button button in GameObject.Find("PieceButtons").GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;

        selectedPiece = piece;
    }

    // Canvia l'aspecte del button seleccionat
    public void ChangeSelectedColor(string color)
    {
        foreach (Button button in GameObject.Find("ColorButtons").GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
        UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.GetComponent<Button>().interactable = false;
        selectedColor = color;
    }

    // Funció per actualitzar el valor del eix X
    public void UpdateAxisX()
    {
        string inputText = GameObject.Find("InputAxisX").GetComponent<TMP_InputField>().text;
        if (inputText.Length > 0)
        {
            xAxis = int.Parse(inputText);
        }
    }

    // Funció per actualitzar el valor del eix Y
    public void UpdateAxisY()
    {
        string inputText = GameObject.Find("InputAxisY").GetComponent<TMP_InputField>().text;
        if (inputText.Length > 0)
        {
            yAxis = int.Parse(inputText);
        }
    }


    // Funció per tancar el recuadre de text
    public void CloseAxisInfo()
    {
        bool error = false;

        if (xAxis < 5 || xAxis > 14){ error = true;  }
        if(yAxis < 5 || yAxis > 8){ error = true;  }

        if (error)
        {
            // S'habiliten i deshabiliten els botons i recuadres de text pertinents
            GameObject.Find("ErrorAxisBox").GetComponentInChildren<TMPro.TextMeshProUGUI>().enabled = true;
        }
        else
        {
            // S'habiliten i deshabiliten els botons i recuadres de text pertinents
            GameObject.Find("AxisDisplayerCanvas").GetComponent<Canvas>().enabled = false;

            RunEditor();
        }

    }

    // Es comprova si hi ha mínim un rei de cada color
    public void CheckPieces()
    {
        bool whiteKing = false;
        bool blackKing = false;

        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                VoidPiece vp = EditorManager.matrixPiecesEditor[i, j];
                if(vp != null)
                {
                    if (vp.type == "K")
                    {
                        if (vp.color == "W")
                        {
                            whiteKing = true;
                        }
                        else
                        {
                            blackKing = true;
                        }
                    }
                }
            }
        }

        if (!whiteKing || !blackKing)
        {
            GameObject.Find("ErrorKingDisplayerCanvas").GetComponent<Canvas>().enabled = true;
            GameObject.Find("PrefEPM").GetComponent<GraphicRaycaster>().enabled = false;
        }
        else
        {
            Game();
        }
    }

    // Funció per tancar el recuadre de text
    public void CloseErrorKingsInfo()
    {
        GameObject.Find("ErrorKingDisplayerCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("PrefEPM").GetComponent<GraphicRaycaster>().enabled = true;

    }

    // Funció per començar la partida
    public void Game()
    {
        if(Login.idPlayer == 0)
        {
            // S'inicia l'escena del joc
            SceneManager.LoadScene("Game");
        }
        else
        {
            StartCoroutine(Login.CreateGame("custom"));
        }
    }

}
