using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameScript : MonoBehaviour
{
    public GameObject NewGamePanel;

    public void YesButton()
    {
        string filePath = Application.persistentDataPath + "/CharactersData.json";
        if(System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        SceneManager.LoadScene("UiTest");
    }

    public void NoButton()
    {
        NewGamePanel.SetActive(false);
    }
}
