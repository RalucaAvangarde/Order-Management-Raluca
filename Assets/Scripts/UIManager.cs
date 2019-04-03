using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private InputField inputName;
    [SerializeField]
    private InputField inputQuantity;
    [SerializeField]
    private InputField inputCustomerName;
    public TreeManager list;
    public List<string> MyList;
    private JsonUtils utils;
    // Start is called before the first frame update
    void Start()
    {
        utils = new JsonUtils();
        MyList = new List<string>();
        list = new TreeManager();
        list.Insert(30);
        list.Insert(50);
        list.Insert(20);
    }
    public void Afis()
    {
        Debug.Log(list.myNode.key);
        Debug.Log(list.myNode.left.key);
        Debug.Log(list.myNode.right.key);
        
    }
    public void ShowItems()
    {
        var space = "";
        if (!string.IsNullOrEmpty(inputName.text) || inputName.text.Contains(space))
        {
           MyList.Add(inputName.text);
            
        }
        else
        {
            Debug.Log("Input is empty");
        }
    }

   
}
