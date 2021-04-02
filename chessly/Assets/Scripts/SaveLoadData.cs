using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveLoadData
{
    public static void SaveData<T>(T data, string path, string filename)
    {
        string fullPath = Application.persistentDataPath + "/" + path + "/";
        
        bool checkPath = Directory.Exists(fullPath);

        if (!checkPath)
        {
            Directory.CreateDirectory(fullPath);
        }

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(fullPath + filename + ".json", json);
    }

    public static T LoadData<T>(string path, string filename)
    {
        string fullPath = Application.persistentDataPath + "/" + path + "/" + filename + ".json";
        bool checkFile= File.Exists(fullPath);

        if (checkFile)
        {
            string dataText = File.ReadAllText(fullPath);
            T dataObj = JsonUtility.FromJson<T>(dataText);

            return dataObj;
        }
        else
        {
            return default;
        }
    }
}
