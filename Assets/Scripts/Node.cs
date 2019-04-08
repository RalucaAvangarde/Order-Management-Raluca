using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Node<T> where T : IDataTypeHelper
{
    public T value;
    public Node<T> left;
    public Node<T> right;
    public Node(T val)
    {
        value = val;
        left = null;
        right = null;
    }
}

public interface IDataTypeHelper
{
    int ID { get; }
    string Name { get; }
}