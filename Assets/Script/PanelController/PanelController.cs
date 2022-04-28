using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelController : MonoBehaviour
{
    public GameObject ResourcesPanel;
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
        ResourceDescription.text = Inventory.instance.content[Inventory.instance.currentResource].name;
    }

    public void GetNextResources()
    {
        if(Inventory.instance.content.Count == 0)
        {
            return;
        }

        Inventory.instance.currentResource++;
        if(Inventory.instance.currentResource > Inventory.instance.content.Count - 1)
        {
            Inventory.instance.currentResource = 0;
        }

        UpdatePanelUI();
    }

    public void GetPreviousResources()
    {
        if (Inventory.instance.content.Count == 0)
        {
            return;
        }

        Inventory.instance.currentResource--;
        if (Inventory.instance.currentResource < 0)
        {
            Inventory.instance.currentResource = Inventory.instance.content.Count - 1;
        }

        UpdatePanelUI();
    }

    public void GiveResources()
    {
        FoodData currentResource = Inventory.instance.content[Inventory.instance.currentResource];
        Characters[currentCharacter].resourcesAttribuated.Add(currentResource);
        Inventory.instance.content.Remove(currentResource);
    }




}
