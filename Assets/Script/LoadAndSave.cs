using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LoadAndSave : MonoBehaviour
{
    public static LoadAndSave instance;
    public PanelController ui;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one instance of LoadAndSave in this scene");
            return;
        }
        instance = this;
    }

    public void Start()
    {
        string resourcesSaved = PlayerPrefs.GetString("inventoryResources","");
        
        //The condition is only here for test

        if (resourcesSaved != "")
        {
            Inventory.instance.ClearInventory();
        }

        string[] resourcesSavedArray = resourcesSaved.Split(",");

        for (int i = 0; i < resourcesSavedArray.Length; i++)
        {
            if (resourcesSavedArray[i] != "")
            {
                int id = int.Parse(resourcesSavedArray[i]);
                FoodData currentResource = ResourcesDataBase.instance.allResources.Single(x => x.id == id);
                Inventory.instance.content.Add(currentResource);
            }
        }
        ui.UpdatePanelUI();
    }

    public void SaveData()
    {
        string resourcesInInventory = string.Join(",", Inventory.instance.content.Select(x => x.id));
        PlayerPrefs.SetString("inventoryResources", resourcesInInventory);
    }
}
