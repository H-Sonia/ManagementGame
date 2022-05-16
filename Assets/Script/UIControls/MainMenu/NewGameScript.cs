using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameScript : MonoBehaviour
{
    public GameObject NewGamePanel;

    public void YesButton()
    {
        PlayerPrefs.SetString("inventoryResources", "");
        PlayerPrefs.SetString("characters", "");
        PlayerPrefs.SetString("deceasedCharacters", "");
        PlayerPrefs.SetString("newcomers", "");
        PlayerPrefs.SetInt("firstTimeInDorms", 0);

        SceneManager.LoadScene("UiTest");
    }

    public void NoButton()
    {
        NewGamePanel.SetActive(false);
    }
}
