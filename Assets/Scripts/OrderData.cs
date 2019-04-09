using System;
using System.Collections.Generic;

[Serializable]
public class OrderData
{
    public List<Order> ElementsOrder;
   
}
[Serializable]
public class ProductData
{
    public List<Product> ProductList;
}
[Serializable]
public class Product : IDataTypeHelper
{
    public int IdProduct;
    public int ProductQuantity;
    public string ProductName;

    public int ID => IdProduct;

    public string Name => ProductName;

    public override string ToString() => "ID: " + IdProduct + " - Name:  " + ProductName + "  - Quantity:  " + ProductQuantity;
}
[Serializable]
public class Order : IDataTypeHelper
{
    public int OrderId;
    public string ClientName;
    public List<Product> OrderElements;

    public int ID => OrderId;

    public string Name => ClientName;
}
