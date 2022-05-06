using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingScript : MonoBehaviour
{
    public GameObject settingPanel; 
    public void quit()
    {
        settingPanel.SetActive(false);
    }
}
