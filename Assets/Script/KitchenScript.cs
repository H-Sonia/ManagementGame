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
    public string ifNoResources = "I didn't managed to salvage anything today... I go back in the block with a heavy heart.";
    public string ifResources = "I managed to salvage some ";
    [TextArea]
    public string ifResources2;
    int maxResources; 


    public void MaxResourcesNumber()
    {
        switch (MainManager.instance.season)
        {
            case 0:
                maxResources = 3;
                break;
            case 1:
                maxResources = 3;
                break;
            case 2:
                maxResources = 2;
                break;
            case 3:
                maxResources = 2;
                break;
            default:
                Debug.LogWarning("Season index out of range");
                break;
        }
    }

    public void ObtainResources()
    {
        List<string> resourcesObtained = new List<string>();
        MaxResourcesNumber();
        System.Random random = new System.Random();
        int nbResources = random.Next(maxResources);
        for (int i = 0; i < nbResources; i++)
        {
            int rarity = random.Next(100);
            if (rarity <= 5)
            {
                int foodIndex = random.Next(ResourcesDataBase.instance.veryRareResources.Length);
                Inventory.instance.content.Add(ResourcesDataBase.instance.veryRareResources[foodIndex]);
                string itemName = ResourcesDataBase.instance.veryRareResources[foodIndex].itemName;
                if(!resourcesObtained.Contains(itemName))
                {
                    resourcesObtained.Add(itemName);
                }
            }
            else
            {
                if (rarity <= 20)
                {
                    int foodIndex = random.Next(ResourcesDataBase.instance.rareResources.Length);
                    Inventory.instance.content.Add(ResourcesDataBase.instance.rareResources[foodIndex]);
                    string itemName = ResourcesDataBase.instance.rareResources[foodIndex].itemName;
                    if(!resourcesObtained.Contains(itemName))
                    {
                        resourcesObtained.Add(itemName);
                    }
                }
                else
                {
                    int foodIndex = random.Next(ResourcesDataBase.instance.commonResources.Length);
                    Inventory.instance.content.Add(ResourcesDataBase.instance.commonResources[foodIndex]);
                    string itemName = ResourcesDataBase.instance.commonResources[foodIndex].itemName;
                    if(!resourcesObtained.Contains(itemName))
                    {
                        resourcesObtained.Add(itemName);
                    }
                }
                itemObtain.text = ifResources + itemText +ifResources2+ "\n";
            }
            
            
        }
        if(IsSomeoneSick())
        {
            int rarity = random.Next(100);
            if (rarity <= 20)
            {
                
                Inventory.instance.content.Add(ResourcesDataBase.instance.medicine);
                string itemName =  " a Medicine bottle";
                if(!resourcesObtained.Contains(itemName))
                    {
                        resourcesObtained.Add(itemName);
                    }
            }

        }
        DisplayMessage(nbResources, ref resourcesObtained);

        ResultPanel.SetActive(true);
        CookButton.interactable = false;
    }

    bool IsSomeoneSick()
    {
        for (int i = 0; i < CharacterManager.instance.charactersLists.CharactersInDorm.Count; i++)
        {
            if(CharacterManager.instance.charactersLists.CharactersInDorm[i].isSick)
            {
                
                return true;
            }
        }
        return false;
    }

    public void DisplayMessage(int numberOfResources, ref List<string> listOfResources)
    {
        Debug.LogWarning("nb :"+numberOfResources);
        switch(numberOfResources)
        {
            case 0:
                itemObtain.text = ifNoResources;
                break;
            case 1:
                itemObtain.text = ifResources+listOfResources[0]+". "+ifResources2;
                break;
            default:
                string itemlist = ListReceived(ref listOfResources);
                itemObtain.text = ifResources+itemlist+". "+ifResources2;
                break;
        }
    }

    public string ListReceived(ref List<string> listOfResources)
    {
        string itemlist = "";

        for(int i=0; i < listOfResources.Count-1; i++ )
        {
            itemlist += listOfResources[i]+", ";
        }
        itemlist += "and " + listOfResources[listOfResources.Count-1];

        return itemlist;
    }
    public void QuitResultPanel()
    {
        ResultPanel.SetActive(false);
    }

    public void ActiveCook()
    {
        CookButton.interactable = true;
    }
    public void DayFunction()
    {
        CookButton.interactable = true;
    }
}
