using System;
using System.Collections.Generic;

[Serializable]
public class OrderData
{
    public ElementsList ManagementOrders;
}

[Serializable]
public class ElementsList
{
    public List<Order> ElementsOrder;
}

[Serializable]
public class Product
{
    public int IdProduct;
    public int ProductQuantity;
    public string ProductName;
    
}
[Serializable]
public class Order
{
    public int OrderId;
    public string ClientName;
    public List<Product> OrderElements;
}
