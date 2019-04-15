using System;
using System.Collections.Generic;
using System.Linq;
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

    /// <summary>
    /// display products with their quantities into a scroll view
    /// </summary>
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
    /// <summary>
    /// show clients and their orders into a scroll view when click on "View Orders" button
    /// </summary>
    public void ShowClients()
    {
        try
        {
            bstOrder = BinarySearchTree<Order>.FromList(utils.GetOrderList());
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
   /// <summary>
   /// Display a pop-up with a message when order list is empty
   /// </summary>
    private void DisplayPanel()
    {
        panelOrders.gameObject.SetActive(true);
    }
   
    /// <summary>
    /// Add product with name and quantity, to Product list, save it to Json
    /// </summary>
    public void AddElement()
    {
        if (!string.IsNullOrEmpty(inputName.text) && int.Parse(inputQuantity.text) > 0)
        {
            var exists = bst.FindNode(inputName.text);
            //if product exist already update just quantity
            if (exists != null)
            {
                exists.value.ProductQuantity += int.Parse(inputQuantity.text);
                bst.UpdateValue(exists.value);
                SaveElementsToJson();
                ClearFields();
            }
            else
            {
                var itemToAdd = new Product() { IdProduct = 0, ProductName = inputName.text, ProductQuantity = int.Parse(inputQuantity.text) };
                itemToAdd.IdProduct = itemToAdd.GetHashCode();
                bst.AddNode(itemToAdd);
                SaveElementsToJson();
                ClearFields();
            }
        }
        else
        {
            Debug.LogError("Incorrect format");
        }
    }
   
    /// <summary>
    /// Create order for Clients and save it to Json, when press on "AddToOrder" button
    /// </summary>
    public void AddOrder()
    {
        var prod = bst.FindNode(productNameTextOnPanel.text).value;

        var initialQuantity = prod.ProductQuantity;
        var quantityAfterOrder = initialQuantity - int.Parse(updateQuantity.text);

        if (bstOrder.FindNode(inputCustomerName.text) == null )
        {
            if (prod.ProductQuantity >= int.Parse(updateQuantity.text))
            {
                orderTextMessage.text = "Order was succesfuly created";
                prod.ProductQuantity = quantityAfterOrder;
                var order = new Order();
                order.ClientName = inputCustomerName.text;
                order.OrderId = order.GetHashCode();
                order.OrderElements = new List<Product>();
                //Debug.Log(prod.ProductQuantity + " prod1");

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

                var updateItemIfExist = new Product();
                try
                {
                    updateItemIfExist = orderToUpdate.value.OrderElements.Where(x => x.ProductName == prod.ProductName).First();
                }
                catch (Exception)
                {
                    updateItemIfExist = null;
                }

                if (updateItemIfExist != null)
                {
                    updateItemIfExist.ProductQuantity += int.Parse(updateQuantity.text);
                    bstOrder.UpdateValue(orderToUpdate.value);
                }
                else
                {
                    orderToUpdate.value.OrderElements.Add(prod);

                    bstOrder.UpdateValue(orderToUpdate.value);
                }

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

    /// <summary>
    /// save products to  json
    /// </summary>
    private void SaveElementsToJson()
    {
        var prodList = new List<Product>();
        bst.ToList(prodList);
        JsonUtils.DefaultElements.ProductList = prodList;
        utils.SaveData();
    }
    
    /// <summary>
    /// save orders to json 
    /// </summary>
    private void SaveOrdersToJson()
    {
        var orderList = new List<Order>();
        bstOrder.ToList(orderList);
        JsonUtils.DefaultOrderElements.ElementsOrder = orderList;
        utils.SaveOrderData();
    }
/// <summary>
/// Update quantity for Selected product by pressing "Update" button
/// </summary>
    public void UpdateElement()
    {
        if (int.Parse(updateQuantity.text) > 0)
        {


            var itemToUpdate = bst.FindNode(productNameTextOnPanel.text);
            itemToUpdate.value.ProductQuantity = int.Parse(updateQuantity.text);
            bst.UpdateValue(itemToUpdate.value);
            SaveElementsToJson();

        }
        else
        {
            Debug.LogError("Quantity can`t be negative!");
        }
    }
   
    /// <summary>
    /// Delete a product from inventory when click on Delete button 
    /// </summary>
    public void DeleteElement()
    {
        var itemToDelete = bst.FindNode(productNameTextOnPanel.text);
       Debug.Log(itemToDelete.value);
       bst = bst.Delete(itemToDelete.value);
       SaveElementsToJson();
    }

  
    /// <summary>
    /// Delete orders when click on Proceed button
    /// </summary>
    public void DeleteOrders()
    {
        utils.EmptyOrders();
        bstOrder = new BinarySearchTree<Order>();
        Debug.Log("Delete orders");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="input"></param>
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
  
    /// <summary>
    /// add quantity for a product
    /// </summary>
    /// <param name="operation"></param>
    public void AddQuantity(int operation)
    {
        Quantity(operation, inputQuantity);
    }


    /// <summary>
    /// update quantity for a product
    /// </summary>
    /// <param name="operation"></param>
    public void UpdateQuantity(int operation)
    {
        Quantity(operation, updateQuantity);
    }

  
    /// <summary>
    /// clean old entries
    /// </summary>
    /// <param name="itemContainer"></param>
    private void ClearList(Transform itemContainer)
    {
        foreach (Transform item in itemContainer)
        {
            Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// clean order list view
    /// </summary>
    public void ClearClientsView()
    {
        ClearList(parentOrders);
    }

    /// <summary>
    /// clean inputFields
    /// </summary>
    private void ClearFields()
    {
        inputName.text = "";
        inputQuantity.text = "0";
        updateQuantity.text = "0";
    }
    /// <summary>
    /// Set start values for input fields
    /// </summary>
    private void SetInputValues()
    {
        numberField = 1;
        inputQuantity.text = "0";
        updateQuantity.text = "0";
    }
}