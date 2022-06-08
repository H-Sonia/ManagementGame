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
        itemObtain.text = ifNoResources;
        string itemText = "";
        //itemObtain.text += "You obtained :\n";

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
                itemObtain.text += " - "+ ResourcesDataBase.instance.veryRareResources[foodIndex].itemName+"\n";
                itemText += ResourcesDataBase.instance.veryRareResources[foodIndex].itemName + ", ";
            }
            else
            {
                if (rarity <= 20)
                {
                    int foodIndex = random.Next(ResourcesDataBase.instance.rareResources.Length);
                    Inventory.instance.content.Add(ResourcesDataBase.instance.rareResources[foodIndex]);
                    itemObtain.text += " - " + ResourcesDataBase.instance.rareResources[foodIndex].itemName + "\n";
                    itemText += ResourcesDataBase.instance.rareResources[foodIndex].itemName + ", ";
                }
                else
                {
                    int foodIndex = random.Next(ResourcesDataBase.instance.commonResources.Length);
                    Inventory.instance.content.Add(ResourcesDataBase.instance.commonResources[foodIndex]);
                    itemObtain.text += " - " + ResourcesDataBase.instance.commonResources[foodIndex].itemName + "\n";
                    itemText += ResourcesDataBase.instance.commonResources[foodIndex].itemName + ", ";
                }
                itemObtain.text = ifResources + itemText +ifResources2+"\n";
            }
            
            
        }
        if(IsSomeoneSick())
        {
            int rarity = random.Next(100);
            if (rarity <= 20)
            {
                
                Inventory.instance.content.Add(ResourcesDataBase.instance.medicine);
               
            }

        }

        ResultPanel.SetActive(true);
        CookButton.interactable = false;
        MapManagerScript.instance.ChangeRoomState(0);
        MainManager.instance.ChangeTime();
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
