﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonUtils : MonoBehaviour
{
    public static OrderData DefaultOrderElements { get; set; }
    public static ProductData DefaultElements { get; set; }
    private string jsonFilePath = "Products.json";
    private string fileOrder = "OrdersFile.json";


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

    public List<Order> GetOrderList()
    {
        ReadOrdersData();
        return DefaultOrderElements.ElementsOrder;

    }
    public void SaveData()
    {
        string contents = JsonUtility.ToJson(DefaultElements, true);
        File.WriteAllText(jsonFilePath, contents);
        // Resources.Load - for json file
    }
    public void SaveOrderData()
    {
        string contents = JsonUtility.ToJson(DefaultOrderElements, true);
        File.WriteAllText(fileOrder, contents);
    }
    public void ReadData()
    {

        if (File.Exists(jsonFilePath))
        {
            string contents = File.ReadAllText(jsonFilePath);
            DefaultElements = JsonUtility.FromJson<ProductData>(contents);

        }
        else
        {
            Debug.Log("Unable to read default input file");
            var temp = new ProductData();
            temp.ProductList = new List<Product>();
            temp.ProductList.Add(new Product() { IdProduct = 1, ProductName = "Phone", ProductQuantity = 1 });
            DefaultElements = temp;
            SaveData();
        }
    }

    public void ReadOrdersData()
    {
        if (File.Exists(fileOrder))
        {
            string contents = File.ReadAllText(fileOrder);
            DefaultOrderElements = JsonUtility.FromJson<OrderData>(contents);

        }
        else
        {
            var temp = new ProductData();
            temp.ProductList = new List<Product>();
            temp.ProductList = new List<Product>();
            temp.ProductList.Add(new Product() { IdProduct = 1, ProductName = "Phone", ProductQuantity = 1 });
            Debug.Log("Unable to read default input file");
            var prod = new OrderData();
            prod.ElementsOrder = new List<Order>();
            prod.ElementsOrder.Add(new Order() { OrderId = 1, ClientName = "Raluca", OrderElements = temp.ProductList });
            DefaultOrderElements = prod;
            SaveOrderData();
        }
    }

    public void EmptyOrders()
    {
        DefaultOrderElements.ElementsOrder = null;
        SaveOrderData();
    }
}
