using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject ResourcesManagementPanel;
    public GameObject SeeResourcesPanel;
    public GameObject IdPanel;
    public SeeResourcesPanelControler panelControler;
    public IDpanelControler idpanelController;

    public void openResourcesPanel(int characterId)
    {
        ResourcesManagementPanel.SetActive(true);
        Inventory.instance.currentCharacter = characterId;
    }

    public void openSeeResourcesPanel(int characterId)
    {
        SeeResourcesPanel.SetActive(true);
        Inventory.instance.currentCharacter = characterId;
        panelControler.UpdatePanelUI();
    }

    public void openIdPanel(int characterId)
    {
        IdPanel.SetActive(true);
        Inventory.instance.currentCharacter = characterId;
        idpanelController.DisplayID();
    }
}
