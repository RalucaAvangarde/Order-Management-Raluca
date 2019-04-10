using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickScript : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector]
    public string MyNameText { get; set; }
   
    [HideInInspector]
    public GameObject myObject;
    [HideInInspector]
    public Text productName;
   

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            myObject.SetActive(true);
            productName.text = MyNameText;
            
        }
    }
}
