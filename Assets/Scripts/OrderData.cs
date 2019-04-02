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
    public List<Item> ElementList;
    public List<Elements> ElementsOrder;
}

[Serializable]
public class Item
{
    public int IdItem;
    public int ItemQuantity;
    public string ItemName;
}
[Serializable]
public class Elements
{
    public int Order;
    public string ClientName;
}
