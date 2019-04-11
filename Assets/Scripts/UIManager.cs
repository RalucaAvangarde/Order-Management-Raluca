using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private InventoryElements entryPrefab;
    private List<InventoryElements> entryElemList = new List<InventoryElements>();

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
    

    [SerializeField]
    private Button OrdersObj;
    [SerializeField]
    private Transform parentOrders;
    [SerializeField]
    private Transform parentProducts;
    [SerializeField]
    private GameObject panelOrders;
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private Text productNameTextOnPanel;
    [SerializeField]
    private Text orderTextMessage;
    private int numberField;

    void Start()
    {
        SetInputValues();
        utils = new JsonUtils();
        bst = BinarySearchTree<Product>.FromList(utils.GetProductList());
        bstOrder = BinarySearchTree<Order>.FromList(utils.GetOrderList());
        ShowProducts();
    }

    // display products with their quantities
    public void ShowProducts()
    {
        ClearList(parentProducts);
        var products = new List<Product>();
        bst.ToList(products);
        ShowProducts(products, parentProducts);

    }
    private void ShowPanel(InventoryElements element)
    {
        panel.SetActive(true);
        productNameTextOnPanel.text = element.productText.text;
    }

    public void ShowProducts(List<Product> listOfProducts, Transform parentContainer)
    {
        foreach (var item in listOfProducts)
        {
            InventoryElements elem = Instantiate(entryPrefab);
            elem.SetValues(item.ProductName, item.ProductQuantity.ToString(), ShowPanel);
            elem.transform.parent = parentContainer;
            entryElemList.Add(elem);

        }
        //force update canvas to reposition elements in UI and rebuild layouts
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentProducts.GetComponent<RectTransform>());
        Canvas.ForceUpdateCanvases();
    }

    public void ShowClients()
    {
        try
        {
            ClearList(parentOrders);
            Debug.Log("Clients");
            var orders = new List<Order>();
            bstOrder.ToList(orders);
            foreach (var item in orders)
            {
                Debug.Log(item.ClientName + "---" + item.OrderId);
                var textObj = OrdersObj.GetComponentInChildren<Text>();
                textObj.text = item.ClientName;
                var btn = Instantiate(OrdersObj, parentOrders);
                ShowProducts(item.OrderElements, parentOrders);

            }
        }
        catch (Exception)
        {
            DisplayPanel();
        }
       
    }
   
    private void DisplayPanel()
    {
        panelOrders.gameObject.SetActive(true);
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
        var prod = bst.FindNode(productNameTextOnPanel.text).value;

        var initialQuantity = prod.ProductQuantity;
        var quantityAfterOrder = initialQuantity - int.Parse(updateQuantity.text);

        if (bstOrder.FindNode(inputCustomerName.text) == null)
        {
            if (prod.ProductQuantity >= int.Parse(updateQuantity.text))
            {
                orderTextMessage.text = "Order was succesfuly created";
                prod.ProductQuantity = quantityAfterOrder;
                var order = new Order();
                order.ClientName = inputCustomerName.text;
                order.OrderId = order.GetHashCode();
                order.OrderElements = new List<Product>();
                Debug.Log(prod.ProductQuantity + " prod1");

                Product newProduct = new Product();
                newProduct.ProductQuantity = int.Parse(updateQuantity.text);
                newProduct.ProductName = prod.ProductName;
                newProduct.IdProduct = prod.IdProduct;


                order.OrderElements.Add(newProduct);
                bstOrder.AddNode(order);
                SaveOrdersToJson();
          
                bst.UpdateValue(prod);
                SaveElementsToJson();
                ShowProducts();
            }
            else
            {
                orderTextMessage.text = "Quantity is too big";
                Debug.LogError("Invalid Quantity!");
               
            }

        }
        else
        {
            if (prod.ProductQuantity >= int.Parse(updateQuantity.text))
            {
                orderTextMessage.text = "Order was succesfuly created";
                prod.ProductQuantity = int.Parse(updateQuantity.text);
                var orderToUpdate = bstOrder.FindNode(inputCustomerName.text);
                orderToUpdate.value.OrderElements.Add(prod);

                bstOrder.UpdateValue(orderToUpdate.value);
                SaveOrdersToJson();

                prod.ProductQuantity = quantityAfterOrder;
                bst.UpdateValue(prod);
                SaveElementsToJson();
               
                ShowProducts();
            }
            else
            {
                orderTextMessage.text = "Quantity is too big";      
            }

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

    //Delete orders
    public void DeleteOrders()
    {
        utils.EmptyOrders();
        bstOrder = new BinarySearchTree<Order>();
        //ShowClients();
        Debug.Log("Delete orders");
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

    //clean old entries
    private void ClearList(Transform itemContainer)
    {
        foreach (Transform item in itemContainer)
        {
            Destroy(item.gameObject);
        }
    }

    // clean order list view
    public void ClearClientsView()
    {
        ClearList(parentOrders);
    }

    //clean inputFields
    private void ClearFields()
    {
        inputName.text = "";
        inputQuantity.text = "0";
        updateQuantity.text = "0";
    }

    //Set start values for input fields
    private void SetInputValues()
    {
        numberField = 1;
        inputQuantity.text = "0";
        updateQuantity.text = "0";
    }
}