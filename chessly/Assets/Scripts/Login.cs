using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    private int isLogin = 0;
    private int ifExsist;
    private int nullValues;
    private string username;
    private string password;

    private int selection = 0;

    public static VoidPiece[,] matrixPieces;

    public static int idNewGame;

    public static int idPlayer = 0;
    public static string namePlayer = "";

    // Objecte per gestionar les traduccions
    public static LanguagesData languageData;

    // Start is called before the first frame update
    void Start()
    {
        ifExsist = 0;
        nullValues = 0;
        username = "0";
        password = "0";

        if (idPlayer == 0)
        {
            GameObject.Find("SelectMenu").GetComponent<Canvas>().enabled = false;
        }
        else
        {
            GameObject.Find("Main Menu").GetComponent<Canvas>().enabled = false;
        }

        GameObject.Find("TypeSelectionMenu").GetComponent<Canvas>().enabled = false;
        GameObject.Find("StatisticsMenu").GetComponent<Canvas>().enabled = false;

        GameObject.Find("ResponseLoginDisplayerCanvas").GetComponent<Canvas>().enabled = false;

        // Deshabilita el menú de creació d'usuaris
        //GameObject.Find("CreateUserMenu").GetComponent<Canvas>().enabled = false;

        GameButton.OptionsData();

        // Carrega les traduccions dels textos
        LanguageManager.ApplyLanguageData();

        // Recull els textos traduits
        languageData = LanguageManager.getLanguageText();
    }

    // funció per accedir al menu d'opcions
    public void SelectMenu()
    {
        // Tanca el menú principal
        GameObject.Find("Main Menu").GetComponent<Canvas>().enabled = false;

        // Obre el menú d'opcions
        GameObject.Find("SelectMenu").GetComponent<Canvas>().enabled = true;
    }

    // funció per accedir al menu d'opcions
    public void StatisticsMenu()
    {
        StartCoroutine(GetStatistics());

        // Tanca el menú d'opcions
        GameObject.Find("SelectMenu").GetComponent<Canvas>().enabled = false;

        // Obre les estadistiques
        GameObject.Find("StatisticsMenu").GetComponent<Canvas>().enabled = true;
    }

    // Funció per actualitzar el valor del username
    public void UpdateUsername()
    {
        string inputText = GameObject.Find("InputUsername").GetComponent<TMP_InputField>().text;
        if (inputText.Length > 0)
        {
            username = inputText;
        }
    }

    // Funció per actualitzar el valor del password
    public void UpdatePassword()
    {
        string inputText = GameObject.Find("InputPassword").GetComponent<TMP_InputField>().text;
        if (inputText.Length > 0)
        {
            password = inputText;
        }
    }

    // Funció que intenta loguejar al usuari
    public void TryLogin()
    {
        GameObject.Find("Main Menu").GetComponent<GraphicRaycaster>().enabled = false;
        GameObject.Find("SelectMenu").GetComponent<GraphicRaycaster>().enabled = false;

        // S'exectura la funció per a loguejar
        if (username != "0" && password != "0")
        {
            StartCoroutine(SendLogin(username, password));
        }
        else
        {
            nullValues = 1;
            ShowLoginText();
        }
    }

    // Funció que prova de crear un user nou
    public void TryCreate()
    {
        GameObject.Find("Main Menu").GetComponent<GraphicRaycaster>().enabled = false;
        GameObject.Find("SelectMenu").GetComponent<GraphicRaycaster>().enabled = false;

        // S'executa la funció per a crear l'user
        if (username != "0" && password != "0")
        {
            StartCoroutine(CreateUser(username, password));
        }
        else
        {
            nullValues = 1;
            ShowLoginText();
        }
    }

    public void ShowLoginText()
    {
        GameObject.Find("ResponseLoginDisplayerCanvas").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.FailLogin;

        if (isLogin != 0)
        {

            GameObject.Find("ResponseLoginDisplayerCanvas").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.SuccessLogin;
        }

        if (ifExsist != 0)
        {

            GameObject.Find("ResponseLoginDisplayerCanvas").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.FailCreateUser;
        }

        if (nullValues != 0)
        {
            nullValues = 0;
            GameObject.Find("ResponseLoginDisplayerCanvas").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.NullValues;
        }

        GameObject.Find("ResponseLoginDisplayerCanvas").GetComponent<Canvas>().enabled = true;
    }

    // Mostra el cuadre de text
    public void HideLoginText()
    {
        // Amaga el text sobre el response del login
        GameObject.Find("ResponseLoginDisplayerCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Main Menu").GetComponent<GraphicRaycaster>().enabled = true;
        GameObject.Find("SelectMenu").GetComponent<GraphicRaycaster>().enabled = true;
    }

    // Guarda l'elecció del jugador sobre buscar partida o crear-ne una
    public void SaveSelection(int s)
    {
        // s'amaga el menú de selecció
        GameObject.Find("SelectMenu").GetComponent<Canvas>().enabled = false;
        GameObject.Find("TypeSelectionMenu").GetComponent<Canvas>().enabled = true;

        //guardem la selecció
        selection = s;
    }

    // Botó per tornar a mostrar el menú de selecció
    public void BackToSelect()
    {
        // es mostra el menú de selecció
        GameObject.Find("SelectMenu").GetComponent<Canvas>().enabled = true;
        GameObject.Find("TypeSelectionMenu").GetComponent<Canvas>().enabled = false;
        GameObject.Find("StatisticsMenu").GetComponent<Canvas>().enabled = false;

        // esborra la selecció
        selection = 0;
    }

    // Gestor del botó classic game
    public void ClassicGamebutton()
    {
        if (selection == 1)
        {
            StartCoroutine(CreateGame("classic"));
        }
        else
        {
            StartCoroutine(AddUserToGame("classic"));
        }
    }

    // Gestor del botó classic game
    public void CustomGamebutton()
    {
        if (selection == 1)
        {
            SceneManager.LoadScene("BoardEditor");
        }
        else
        {
            StartCoroutine(AddUserToGame("custom"));
        }
    }

    // Retorn al menu principal
    public void BackMenu()
    {
        // Carrega la escena del menu principal
        SceneManager.LoadScene("Main Menu");
    }

    // Tanca la sessió online
    public void LogOut()
    {
        isLogin = 0;
        idPlayer = 0;
        // Recarrega l'escena
        SceneManager.LoadScene("Login");
    }

    /* Funcions get/post */

    // funció per a logueajar l'usuari
    public IEnumerator SendLogin(string username, string password)
    {
        Debug.Log("test login");
        WWWForm form = new WWWForm();
        form.AddField("login", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("http://18.116.223.113/api/login", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);

            isLogin = int.Parse(www.downloadHandler.text);

            idPlayer = isLogin;
            namePlayer = username;

            SelectMenu();
            ShowLoginText();
        }
    }

    //Funció per a crear l'usuari
    public IEnumerator CreateUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("login", username);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post("http://18.116.223.113/api/chess-users", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            int id = int.Parse(www.downloadHandler.text);
            if (id != 0)
            {
                isLogin = id;
                ifExsist = 0;

                idPlayer = isLogin;
                namePlayer = username;
            }
            else
            {
                ifExsist = 1;
            }

            SelectMenu();
            ShowLoginText();
        }
    }

    // funció per a crear partida
    public static IEnumerator CreateGame(string typeGame)
    {
        int xAxis = 0;
        int yAxis = 0;

        int randColor = UnityEngine.Random.Range(0, 2);
        string creatorColor = randColor == 1 ? "W" : "B";
        
        GameObject.Find("TypeSelectionMenu").GetComponent<GraphicRaycaster>().enabled = false;

        switch (typeGame)
        {
            case "custom":
                matrixPieces = EditorManager.matrixPiecesEditor;
                xAxis = EditorManager.xAxis;
                yAxis = EditorManager.yAxis;
                break;
            case "classic":

                xAxis = 8;
                yAxis = 8;

                string[] listPiecesClassic = new string[] {
                    "T", "KN", "B", "Q", "K", "B", "KN", "T"
                };

                string[] colorsClassic = new string[] { "W", "B" };

                matrixPieces = new VoidPiece[8, 8];

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        VoidPiece vP = new VoidPiece();
                        vP.Setup(listPiecesClassic[i], colorsClassic[j]);

                        matrixPieces[i, j * 7] = vP;

                        VoidPiece vPP = new VoidPiece();
                        vPP.Setup("P", colorsClassic[j]);

                        matrixPieces[i, 5 * j + 1] = vPP;
                    }
                }
                break;
        }

        string stringJsonPieces = "";
        for (int i = 0; i < xAxis; i++)
        {
            for (int j = 0; j < yAxis; j++)
            {
                VoidPiece vPiece = matrixPieces[i, j];
                if (vPiece != null)
                {
                    stringJsonPieces = String.Concat(stringJsonPieces, i, "-", j, "-", vPiece.type, "-", vPiece.color, ",");
                }

            }
        }
        stringJsonPieces = stringJsonPieces.TrimEnd(',');

        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(stringJsonPieces);
        stringJsonPieces = System.Convert.ToBase64String(plainTextBytes);

        WWWForm form = new WWWForm();
        form.AddField("pieces", stringJsonPieces);
        form.AddField("type", typeGame);
        form.AddField("color_creator", creatorColor);
        form.AddField("creator_id", idPlayer);
        form.AddField("x_axis", xAxis);
        form.AddField("y_axis", yAxis);

        UnityWebRequest www = UnityWebRequest.Post("http://18.116.223.113/api/game", form);
        yield return www.SendWebRequest();

        GameObject.Find("TypeSelectionMenu").GetComponent<GraphicRaycaster>().enabled = true;

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            int id = int.Parse(www.downloadHandler.text);

            GameButton.typeGame = 6;
            idNewGame = id;

            SceneManager.LoadScene("Game");
        }
    }


    //Funció per a crear l'usuari
    public IEnumerator AddUserToGame(string typeGame)
    {
        GameObject.Find("TypeSelectionMenu").GetComponent<GraphicRaycaster>().enabled = false;
        WWWForm form = new WWWForm();
        
        form.AddField("type", typeGame);

        UnityWebRequest www = UnityWebRequest.Post("http://18.116.223.113/api/connect-to-game/"+idPlayer, form);
        yield return www.SendWebRequest();

        GameObject.Find("TypeSelectionMenu").GetComponent<GraphicRaycaster>().enabled = true;

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            if (www.downloadHandler.text != "")
            {
                int id = int.Parse(www.downloadHandler.text);

                GameButton.typeGame = 6;
                idNewGame = id;

                SceneManager.LoadScene("Game");
            }
            else
            {
                GameObject.Find("ResponseLoginDisplayerCanvas").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = languageData.menu.login.NotGame;
                GameObject.Find("ResponseLoginDisplayerCanvas").GetComponent<Canvas>().enabled = true;
            }
        }
    }

    public IEnumerator GetStatistics()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://18.116.223.113/api/statistics/" + idPlayer);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            var json = JsonUtility.FromJson<OnlineStatisticsData>(www.downloadHandler.text);

            GameObject.Find("TotalWinsValue").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = json.wins + " / " + json.totalGames + " " + languageData.menu.login.statistics.TotalWinsValue;
            GameObject.Find("CreatedGamesValue").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = json.creatorClassic + " " + languageData.menu.login.statistics.Classics + " / " + json.creatorCustom + " " + languageData.menu.login.statistics.Customs;
            GameObject.Find("SearchedGamesValue").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = json.visitorClassic + " " + languageData.menu.login.statistics.Classics + " / " + json.visitorCustom + " " + languageData.menu.login.statistics.Customs;
            GameObject.Find("ColorPiecesValue").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = json.statisticsWhite +"% " + languageData.colors.White + " / "+ json.statisticsBlack + "% " + languageData.colors.Black;
            GameObject.Find("TotalMovesValue").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = json.totalMoves.ToString();
        }
    }
}
