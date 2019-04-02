using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonUtils : MonoBehaviour
{
    public ElementsList DefaultElements { get; set; }
    private string fileName = "Orders.json";
    private string jsonFilePath;


    private void Awake()
    {
        DefaultElements = new ElementsList();
        jsonFilePath = Application.persistentDataPath + "/" + fileName;
        Debug.Log(jsonFilePath);
        SaveData();
    }
    private void SaveData()
    {
        string contents = JsonUtility.ToJson(DefaultElements, true);
        File.WriteAllText(jsonFilePath, contents);
    }
    public void ReadData()
    {

        if (File.Exists(jsonFilePath))
        {
            string contents = File.ReadAllText(jsonFilePath);
            DefaultElements = JsonUtility.FromJson<ElementsList>(contents);

        }
        else
        {
            Debug.Log("Unable to read default input file");
            //SaveData();
        }
    }
}
