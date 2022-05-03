using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeeResourcesPanelControler : MonoBehaviour
{

    public GameObject SeeResourcesPanel;
    public TMP_Text ResourceDescription;
    public Image RessourceImage;
    public Sprite emptyImage;
    //private List<FoodData> characterResources = Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated;
    private int index = 0; 


    public void Quit()
    {
        SeeResourcesPanel.SetActive(false);
    }

    public void UpdatePanelUI()
    {
        Debug.LogWarning(index);
        if (Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated.Count > 0)
        {
            ResourceDescription.text = Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated[index].foodName;
            RessourceImage.sprite = Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated[index].foodImage;
         
        }
        else
        {
            ResourceDescription.text = "";
            RessourceImage.sprite = emptyImage;
        }
    }

    public void GetNextResources()
    {
        if (Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated.Count == 0)
        {
            return;
        }

        index++;
        if (index > Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated.Count - 1)
        {
            index = 0;
        }

        UpdatePanelUI();
    }

    public void GetPreviousResources()
    {
        if (Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated.Count == 0)
        {
            return;
        }

        index--;
        if (index < 0)
        {
            index = Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated.Count - 1;
        }

        UpdatePanelUI();
    }

    public void TakeResources()
    {
        FoodData currentResource = Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated[index];
        Inventory.instance.content.Add(currentResource);
        Inventory.instance.Characters[Inventory.instance.currentCharacter].resourcesAttribuated.Remove(currentResource);
        GetNextResources();
        UpdatePanelUI();
    }
}
