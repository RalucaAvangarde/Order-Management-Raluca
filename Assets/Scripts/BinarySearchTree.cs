using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class BinarySearchTree<T> where T : IDataTypeHelper
    {
        public Node<T> root { get; set; }
        public static int Length { get; set; }
        public void AddNode(T valueToBeInserted)
        {
            if (root == null)
            {
                root = new Node<T>(valueToBeInserted);
            }
            else
            {
                AddNode(valueToBeInserted, root);
            }
            Length++;
        }

        private void AddNode(T valueToBeInserted, Node<T> current)
        {
            if (valueToBeInserted.ID < current.value.ID)
            {
                if (current.left == null)
                    current.left = new Node<T>(valueToBeInserted);
                else
                    AddNode(valueToBeInserted, current.left);
            }

            if (valueToBeInserted.ID > current.value.ID)
            {
                if (current.right == null)
                    current.right = new Node<T>(valueToBeInserted);
                else
                    AddNode(valueToBeInserted, current.right);
            }
        }

        public void recursiveInorder(Node<T> root)
        {

            if (root.left != null)
            {
                recursiveInorder(root.left);
            }
            Debug.Log(root.value.ToString() + "From traversal");
            if (root.right != null)
            {
                recursiveInorder(root.right);
            }

        }
        public Node<T> FindNode(int idOFelementTofind)
        {
            return FindNode(root, idOFelementTofind);
        }
        private Node<T> FindNode(Node<T> root, int idOFelementTofind)
        {

            if (root == null)
            {
                return null;
            }
            else
            {
                if (root.value.ID == idOFelementTofind)
                {

                    return root;

                }
                if (idOFelementTofind > root.value.ID)
                {
                    return FindNode(root.right, idOFelementTofind);
                }
                else
                    return FindNode(root.left, idOFelementTofind);

            }

        }
        public Node<T> FindNode(string elementName)
        {
            return FindNode(root, elementName);
        }
        private Node<T> FindNode(Node<T> root, string elementName)
        {
            if (root == null)
            {
                return null;
            }
            if (root.left != null)
            {
                return FindNode(root.left, elementName);
            }
            if (root.value.Name == elementName)
            {
                return root;
            }
            if (root.right != null)
            {
                return FindNode(root.right, elementName);
            }
            return null;

        }

        public void UpdateValue(T value)
        {
            UpdateValue(root, value);
        }

        private void UpdateValue(Node<T> root, T value)
        {
            if (root == null)
            {
                return;
            }
            else
            {
                if (root.value.ID == value.ID)
                {

                    root.value = value;

                }
                if (root.value.ID < value.ID)
                {
                    UpdateValue(root.right, value);
                }
                else
                    UpdateValue(root.left, value);

            }
        }

        public void ToList(List<T> result)
        {
            ToList(root, result);
        }
        private void ToList(Node<T> root, List<T> result)
        {


            if (root.left != null)
            {
                ToList(root.left, result);
            }
            result.Add(root.value);
            if (root.right != null)
            {
                ToList(root.right, result);
            }
        }

        public static BinarySearchTree<T> FromList(List<T> result)
        {
            var tree = new BinarySearchTree<T>();
            foreach (var item in result)
            {
                tree.AddNode(item);
            }
            return tree;
        }
        private int FindIndexOfElementInList(List<T> list, T element)
        {
            var index = 0;
            foreach (var item in list)
            {
                if (item.ID == element.ID)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }
        public BinarySearchTree<T> Delete(T element)
        {
            var temp = new List<T>();
            this.ToList(temp);
            temp.RemoveAt(FindIndexOfElementInList(temp, element)); 
            return BinarySearchTree<T>.FromList(temp);
           
        }

    }
}
