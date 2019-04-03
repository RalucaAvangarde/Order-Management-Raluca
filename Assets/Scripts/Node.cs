using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public int key;
    public Node left;
    public Node right;
    

    public Node(int item)
    {
        key = item;
        left = null;
        right = null;
    }
    public void DisplayNode()
    {
        Debug.Log(key + " ");
    }
}
