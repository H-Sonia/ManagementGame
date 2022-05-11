using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject ResourcesManagementPanel;
    public GameObject IdPanel;
    public IDpanelControler idpanelController;

    public void openResourcesPanel(int characterId)
    {
        ResourcesManagementPanel.SetActive(true);
        Inventory.instance.currentCharacter = characterId;
    }

    public void openIdPanel(int characterId)
    {
        IdPanel.SetActive(true);
        Inventory.instance.currentCharacter = characterId;
        idpanelController.DisplayID();
    }
}
