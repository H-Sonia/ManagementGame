using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class KitchenScript : MonoBehaviour
{
    public GameObject Kitchen;
    public void ObtainResources()
    {
        
        System.Random random = new System.Random();
        int nbResources = random.Next(20);
        for (int i = 0; i < nbResources; i++)
        {
            int foodIndex = random.Next(ResourcesDataBase.instance.allResources.Length);
            Inventory.instance.content.Add(ResourcesDataBase.instance.allResources[foodIndex]);
        }
        
    }

    public void quit()
    {
        Kitchen.SetActive(false);
    }
}
