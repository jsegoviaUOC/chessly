using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SendLogin();
    }

    public IEnumerator SendLogin()
    {
        WWWForm form = new WWWForm();
        form.AddField("login", "test");
        form.AddField("password", "test");

        UnityWebRequest www = UnityWebRequest.Post("http://18.116.223.113/api/login", form);
        yield return www.SendWebRequest();
        Debug.Log(www.downloadHandler.data);

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
}
