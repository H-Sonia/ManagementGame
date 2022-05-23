using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class KitchenScript : MonoBehaviour
{
    public Button CookButton;
    public GameObject Kitchen;
    public GameObject ResultPanel;
    public TMP_Text itemObtain;

    public void ObtainResources()
    {
        itemObtain.text = "You obtained :\n";

        System.Random random = new System.Random();
        int nbResources = random.Next(10);
        for (int i = 0; i < nbResources; i++)
        {
            int rarity = random.Next(100);
            if (rarity <= 5)
            {
                int foodIndex = random.Next(ResourcesDataBase.instance.veryRareResources.Length);
                Inventory.instance.content.Add(ResourcesDataBase.instance.veryRareResources[foodIndex]);
                itemObtain.text += " - "+ ResourcesDataBase.instance.veryRareResources[foodIndex].itemName+"\n";
            }
            else
            {
                if (rarity <= 20)
                {
                    int foodIndex = random.Next(ResourcesDataBase.instance.rareResources.Length);
                    Inventory.instance.content.Add(ResourcesDataBase.instance.rareResources[foodIndex]);
                    itemObtain.text += " - " + ResourcesDataBase.instance.rareResources[foodIndex].itemName + "\n";
                }
                else
                {
                    int foodIndex = random.Next(ResourcesDataBase.instance.commonResources.Length);
                    Inventory.instance.content.Add(ResourcesDataBase.instance.commonResources[foodIndex]);
                    itemObtain.text += " - " + ResourcesDataBase.instance.commonResources[foodIndex].itemName + "\n";
                }
            }
            
        }
        if(IsSomeoneSick())
        {
            int rarity = random.Next(100);
            Debug.LogWarning("rarity = " + rarity);
            if (rarity <= 20)
            {
                
                Inventory.instance.content.Add(ResourcesDataBase.instance.medicine);
                itemObtain.text += " - Medicine Bottle\n";
            }

        }
        ResultPanel.SetActive(true);
        CookButton.interactable = false;
    }

    bool IsSomeoneSick()
    {
        for (int i = 0; i < Inventory.instance.Characters.Length; i++)
        {
            if(Inventory.instance.Characters[i].isSick)
            {
                
                return true;
            }
        }
        return false;
    }

    public void QuitResultPanel()
    {
        ResultPanel.SetActive(false);
    }

    public void ActiveCook()
    {
        CookButton.interactable = true;
    }
}
