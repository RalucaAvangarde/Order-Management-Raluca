using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryElements : MonoBehaviour
{
    [SerializeField]
    public Text productText;
    [SerializeField]
    private Text quantityText;
    


    public void SetValues(string product, string quantity, Action<InventoryElements> showPanel)
    {
        productText.text = product;
        quantityText.text = quantity;
        GetComponent<Button>().onClick.AddListener(() => { showPanel(this); });
        
    }

    
    
}
