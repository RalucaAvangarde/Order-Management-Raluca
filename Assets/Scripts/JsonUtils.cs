using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonUtils : MonoBehaviour
{
    public static OrderData DefaultElements { get; set; }
    private string fileName = "Orders.json";
    private string jsonFilePath = "Orders.json";


    //private void Awake()
    //{
    //    DefaultElements = new OrderData();
    //    jsonFilePath = fileName;//Application.persistentDataPath + "/" + fileName;
    //    Debug.Log(jsonFilePath);
    //    //ReadData();
    //}
    public JsonUtils()
    {
        ReadData();
    }
    public List<Product> GetProductList()
    {
        ReadData();
        return DefaultElements.ProductList;
    }
    public void SaveData()
    {
        string contents = JsonUtility.ToJson(DefaultElements, true);
        File.WriteAllText(jsonFilePath, contents);
       // Resources.Load - for json file
    }
    public void ReadData()
    {

        if (File.Exists(jsonFilePath))
        {
            string contents = File.ReadAllText(jsonFilePath);
            DefaultElements = JsonUtility.FromJson<OrderData>(contents);

        }
        else
        {
            Debug.Log("Unable to read default input file");
            var temp = new OrderData();
            temp.ProductList = new List<Product>();
            temp.ProductList.Add(new Product() { IdProduct = 1, ProductName = "Phone", ProductQuantity = 1 });
            temp.ElementsOrder = new List<Order>();
            temp.ElementsOrder.Add(new Order() { OrderId = 1, ClientName = "Raluca", OrderElements = temp.ProductList });
            DefaultElements = temp;
            SaveData();
        }
    }
}
