# Order Management Raluca
Order management is an application that allows the user to add products to inventory, or create orders for clients.

## Quick start

1.Open app  
2.Click “AddProduct” to add a new product with a quantity in inventory. 	
3.Left click on a product from product stock, will open a pop-up panel. From here user can chose to update the quantity for the product, or delete the product, or can add it to an order, by click on “AddToOrder" button, in this way, a panel will be opened and user can type a client name, after this press “CreateOrder” and an order will be created for that client. 	
4. Click on “ViewOrders ”, will show to user a list with all clients and their products. From here, by clicking on “Proceed”, all orders will be  processed and the list will be emptied.

## Datatypes

-There are 2 json files in the project that allows the user to save his work from one app session use and reload all data collected and updated when a new session is started.
-These 2 files are named :-> “Products.json”: where all products added to the app will be stored
                          -> ”Orders.json” : where orders are saved.

## DataStructures

The challange for this app was to create and use a BinarySearchTree as the main data structure in the application, where all runtime orders and products will be stored before save them to json file.

The models that are stored in json are saved in “OrderData.cs” that encapsulate models for product and orders, one product contains an identifier(ID) generated from hashCode of product, a name and a quantity and an order contain also an identifier generated from hashCode, a client name and a list of products that was ordered by client. 
To manage and manipulate  multiple objects with only one implementation of BST i choose to use a generic implementation of it “BinarySearchTree<T>”, that allow the user to perform operation like add,update, delete and find data to tree and also to helping methods that convert an bst to list and a list to bst.
  
The entire UI of application is controlled from "UIManager" script, where data from tree is displayed in scroll views or method like add or update are triggered on specific button click.
