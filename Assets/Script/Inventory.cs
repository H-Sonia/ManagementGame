using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<ItemData> content = new List<ItemData>();
    public int currentResource = -1;
    public UIDisplay ui;
    public static Inventory instance;
    
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one Inventory instance in this scene");
            return;
        }
        instance = this;
    }
    public void Start()
    {
        ClearInventory();
        //ui.UpdateMainUi("", false);
    }
    public void DayFunction()
    {

    }

    public void ClearInventory()
    {
        content.Clear();
    }
}
