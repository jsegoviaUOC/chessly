using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

public class Login : MonoBehaviour
{
    private int isLogin;
    private int ifExsist;
    private string username;
    private string password;

    // Start is called before the first frame update
    void Start()
    {
        isLogin = 0;
        ifExsist = 0;
        username = "";
        password = "";

        GameObject.Find("ResponseLoginDisplayerCanvas").GetComponent<Canvas>().enabled = false;

        // Deshabilita el menú de creació d'usuaris
        //GameObject.Find("CreateUserMenu").GetComponent<Canvas>().enabled = false;

        GameButton.OptionsData();

        LanguageManager.ApplyLanguageData();
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
        // S'exectura la funció per a loguejar
        StartCoroutine(SendLogin(username, password));
    }

    // Funció que prova de crear un user nou
    public void TryCreate()
    {
        // S'executa la funció per a crear l'user
        StartCoroutine(CreateUser(username, password));
    }

    public void ShowLoginText()
    {
        GameObject.Find("ResponseLoginDisplayerCanvas").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Fail login :(";

        if (isLogin != 0)
        {

            GameObject.Find("ResponseLoginDisplayerCanvas").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "Login succes!";
        }

        if (ifExsist != 0)
        {

            GameObject.Find("ResponseLoginDisplayerCanvas").GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "This user already exist.\nChange username";
        }

        GameObject.Find("Main Menu").GetComponent<GraphicRaycaster>().enabled = false;
        GameObject.Find("ResponseLoginDisplayerCanvas").GetComponent<Canvas>().enabled = true;
    }

    // Retorn al menu principal
    public void HideLoginText()
    {
        // Amaga el text sobre el response del login
        GameObject.Find("ResponseLoginDisplayerCanvas").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Main Menu").GetComponent<GraphicRaycaster>().enabled = true;
    }

    // Retorn al menu principal
    public void BackMenu()
    {
        // Carrega la escena del menu principal
        SceneManager.LoadScene("Main Menu");
    }

    // Funcions get/post

    // funció per a logueajar l'usuari
    public IEnumerator SendLogin(string username, string password)
    {
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
            Debug.Log("Form upload complete!");

            Debug.Log(www.downloadHandler.text);
            isLogin = int.Parse(www.downloadHandler.text);

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
            Debug.Log("Form upload complete!");

            Debug.Log(www.downloadHandler.text);
            int id = int.Parse(www.downloadHandler.text);
            if (id != 0)
            {
                isLogin = id;
                ifExsist = 0;
            }
            else
            {
                ifExsist = 1;
            }

            ShowLoginText();
        }
    }
}
