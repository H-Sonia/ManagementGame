using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeResourcesPanelControler : MonoBehaviour
{

    public GameObject SeeResourcesPanel;
    public void Quit()
    {
        SeeResourcesPanel.SetActive(false);
    }
}
