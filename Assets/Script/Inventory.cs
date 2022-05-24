using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<ItemData> content = new List<ItemData>();
    public int currentResource = 0;
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
        int firstTimeInDorms = PlayerPrefs.GetInt("firstTimeInDorms", 0);
        if (firstTimeInDorms == 0)
        {
            ClearInventory();
            ui.UpdateMainUi("", false);
        }
            
    }

    public void ClearInventory()
    {
        content.Clear();
    }

}
