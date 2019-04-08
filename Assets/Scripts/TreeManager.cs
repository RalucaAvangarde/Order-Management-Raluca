//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class TreeManager : MonoBehaviour
//{
//    public Node myNode;

//    public TreeManager()
//    {
//        myNode = null;
//    }

//    public Node InsertNode(Node node, int key)
//    {
//        if (node == null)
//        {
//            node = new Node(key);
//            return node;
//        }
//        if (key < node.key)
//        {
//            node.left = InsertNode(node.left, key);
//        }
//        else if (key > node.key)
//        {
//            node.right = InsertNode(node.right, key);
//        }
//        return node;
//    }

//    public void Insert(int key)
//    {
//        myNode = InsertNode(myNode, key);
//        Debug.Log("Added");
//    }

//    public void InOrder()
//    {
//        Ordered(myNode);
//        Debug.Log("inorder");
//    }

//    private void Ordered(Node root)
//    {
//        if (root != null)
//        {
//            return;
//        }
//        Ordered(root.left);
//        Debug.Log("root key is:" + " " + root.key);
//        Ordered(root.right);

//    }

//    public void Delete(int key)
//    {
//        myNode = DeleteNode(myNode, key);
//        //Debug.Log("Deleted");
//    }

//    private Node DeleteNode(Node node, int key)
//    {
//        if (node == null)
//        {
//            return node;

//        }
//        if (key < node.key)
//        {
//            node.left = DeleteNode(node.left, key);
//            Debug.Log("Deleted");
//        }
//        else if (key > node.key)
//        {
//            node.right = DeleteNode(node.right, key);
//        }
//        else
//        {
//            if (node.left == null)          //node with one child or no child
//            {
//                return node;
//            }
//            else if (node.right == null)
//            {
//                return node;
//            }
//            //node with 2 child
//            node.key = ValueMinim(node.right);
//        }
//        return node;
//    }

//    private int ValueMinim(Node root)
//    {
//        int minVal = root.key;
//        while (root.left != null)
//        {
//            minVal = root.left.key;
//            root = root.left;
//        }
//        return minVal;
//    }
//}
