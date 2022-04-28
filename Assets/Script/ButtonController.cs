using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject ResourcesManagementPanel;

    public void openResourcesPanel()
    {
        ResourcesManagementPanel.SetActive(true);
    }
}
