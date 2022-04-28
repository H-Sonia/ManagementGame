using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelController : MonoBehaviour
{
    public GameObject ResourcesPanel;
    public Inventory inventory;
    public TMP_Text ResourceDescription;
    public List<CharactersData> Characters;
    public int currentCharacter;

    public void Start()
    {
        UpdatePanelUI();
    }
   

    public void Quit()
    {
        ResourcesPanel.SetActive(false);
    }

    public void UpdatePanelUI()
    {
        ResourceDescription.text = inventory.content[inventory.currentResource].name;
    }

    public void GetNextResources()
    {
        if(inventory.content.Count == 0)
        {
            return;
        }

        inventory.currentResource++;
        if(inventory.currentResource > inventory.content.Count - 1)
        {
            inventory.currentResource = 0;
        }

        UpdatePanelUI();
    }

    public void GetPreviousResources()
    {
        if (inventory.content.Count == 0)
        {
            return;
        }

        inventory.currentResource--;
        if (inventory.currentResource < 0)
        {
            inventory.currentResource = inventory.content.Count - 1;
        }

        UpdatePanelUI();
    }

    public void GiveResources()
    {
        FoodData currentResource = inventory.content[inventory.currentResource];
        Characters[currentCharacter].resourcesAttribuated.Add(currentResource);
        inventory.content.Remove(currentResource);
    }




}
