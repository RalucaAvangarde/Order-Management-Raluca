using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    [SerializeField]
    private InputField inputName;
    [SerializeField]
    private InputField inputQuantity;
    [SerializeField]
    private InputField updateQuantity;
    [SerializeField]
    private InputField inputCustomerName;
    private BinarySearchTree<Product> bst;
    private BinarySearchTree<Order> bstOrder; 
    private JsonUtils utils;
    private int numberField;

    [SerializeField]
    private Button OrdersObj;
    [SerializeField]
    private Transform parentOrders;
    [SerializeField]
    private GameObject panelOrders;
    private Text clientName;
    private Text productName;
    private Text quantity;
    
    public Text obj;
    public Transform parent;
    public GameObject panel;
    public Text productNameTextOnPanel;
    void Start()
    {
        numberField = 1;
        inputQuantity.text = "0";
        updateQuantity.text = "0";
        utils = new JsonUtils();
        bst = BinarySearchTree<Product>.FromList(utils.GetProductList());
        bstOrder = BinarySearchTree<Order>.FromList(utils.GetOrderList());
        ShowProducts();
    }

    public void ShowProducts()
    {
        ClearList();
        var products = new List<Product>();
        bst.ToList(products);
        foreach (var item in products)
        {
            obj.text = item.ProductName + "    Q: " + item.ProductQuantity;
            var prod = Instantiate(obj, parent);
            var onClick = prod.GetComponent<ClickScript>();
            onClick.myObject = panel;
            onClick.myText = item.ProductName;
            Debug.Log(item.ProductName);
            onClick.productName = productNameTextOnPanel;
            onClick.productName.text = item.ProductName;
            //ClearFields();
        }
    }
    public void ShowClients()
    {
      
        Debug.Log("Clients");
        var orders = new List<Order>();
        bstOrder.ToList(orders);
        foreach (var item in orders)
        {
            Debug.Log(item.ClientName + "---" + item.OrderId );
            var textObj = OrdersObj.GetComponentInChildren<Text>();
            textObj.text = item.ClientName;
           var btn= Instantiate(OrdersObj, parentOrders);
            SetListener(btn);
            foreach (var item2 in item.OrderElements)
            {
              Debug.Log(item2.ProductName);
            }
        }
       
    }
    private void SetListener(Button b)
    {
        b.onClick.AddListener(() => { DisplayPanel(); });

    }
    private void DisplayPanel()
    {
        panelOrders.gameObject.SetActive(true);
    }
    private void ClearList()
    {
        foreach (Transform item in parent)
        {
            Destroy(item.gameObject);
        }
    }

    public void TemplAfis()
    {

        bst.recursiveInorder(bst.root);
        var listFromBst = new List<Product>();
        bst.ToList(listFromBst);
        foreach (var item in listFromBst)
        {
            Debug.Log("From list: " + item.ProductName + "--" + item.ProductQuantity);
        }
        Debug.Log("Found: " + bst.FindNode("Phone").value.ToString());
    }

    public void AddElement()
    {
        if (!string.IsNullOrEmpty(inputName.text))
        {
            var itemToAdd = new Product() { IdProduct = 0, ProductName = inputName.text, ProductQuantity = int.Parse(inputQuantity.text) };
            itemToAdd.IdProduct = itemToAdd.GetHashCode();
            bst.AddNode(itemToAdd);
            SaveElementsToJson();
            ClearFields();
        }
    }
    //Create order for Clients and save it to Json
    public void AddOrder()
    {
        if (bstOrder.FindNode(inputCustomerName.text) == null)
        {
            var order = new Order();
            order.ClientName = inputCustomerName.text;
            order.OrderId = order.GetHashCode();
            Debug.Log(order.OrderId + " Id Order is");
            order.OrderElements = new List<Product>();
            var prod = bst.FindNode(productNameTextOnPanel.text).value;
            prod.ProductQuantity = int.Parse(updateQuantity.text);
            order.OrderElements.Add(prod);
            bstOrder.AddNode(order);
            SaveOrdersToJson();
        }
        else 
        
        {
            var orderToUpdate = bstOrder.FindNode(inputCustomerName.text);
            var productToAdd = bst.FindNode(productNameTextOnPanel.text).value;
            productToAdd.ProductQuantity = int.Parse(updateQuantity.text);
            orderToUpdate.value.OrderElements.Add(productToAdd);

            bstOrder.UpdateValue(orderToUpdate.value);

            SaveOrdersToJson();

        }
        
    }
    //save products to  json
    private void SaveElementsToJson()
    {
        var prodList = new List<Product>();
        bst.ToList(prodList);
        JsonUtils.DefaultElements.ProductList = prodList;
        utils.SaveData();
    }
    //save orders to list
    private void SaveOrdersToJson()
    {
        var orderList = new List<Order>();
        bstOrder.ToList(orderList);
        JsonUtils.DefaultOrderElements.ElementsOrder = orderList;
        utils.SaveOrderData();
    }
    public void UpdateElement()
    {

        var itemToUpdate = bst.FindNode(productNameTextOnPanel.text);
        Debug.Log(itemToUpdate.value.IdProduct);
        itemToUpdate.value.ProductQuantity = int.Parse(updateQuantity.text);
        bst.UpdateValue(itemToUpdate.value);
        SaveElementsToJson();
       

    }
    //Delete product
    public void DeleteElement()
    {
        var itemToDelete = bst.FindNode(productNameTextOnPanel.text);
       Debug.Log(itemToDelete.value);
       bst = bst.Delete(itemToDelete.value);
       SaveElementsToJson();
    }
    
    public void Quantity(int operation, InputField input)
    {


        if (operation == 0)
        {
            if (int.Parse(input.text) > 0)
            {
                input.text = (int.Parse(input.text) - numberField).ToString();
            }
            else
            {
                Debug.LogError("Is not a valid quantity!");
            }
        }
        else
        {
            input.text = (numberField + int.Parse(input.text)).ToString();
        }

    }
    //add quantity
    public void AddQuantity(int operation)
    {
        Quantity(operation, inputQuantity);
    }

    //update quantity
    public void UpdateQuantity(int operation)
    {
        Quantity(operation, updateQuantity);
    }


    private void ClearFields()
    {
        //productNameTextOnPanel.text = "";
        inputName.text = "";
        inputQuantity.text = "0";
        updateQuantity.text = "0";
    }
}