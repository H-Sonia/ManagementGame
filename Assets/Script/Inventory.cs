using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<FoodData> content = new List<FoodData>();
    public int currentResource = 0;
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

    public void ClearInventory()
    {
        content.Clear();
    }

   
}
