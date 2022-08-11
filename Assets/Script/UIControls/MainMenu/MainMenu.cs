using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject creditPanel;
  public GameObject settingPanel;
  public GameObject NewGamePanel;
  public GameObject NewGameButton; 
  public GameObject ContinueButton; 

  void Start()
  {
    string filePathDataChar = Application.persistentDataPath + "/CharactersData.json";
    string filePathDataSave = Application.persistentDataPath + "/DataSave.json";
    if(System.IO.File.Exists(filePathDataChar))
    {
      Debug.Log(filePathDataChar);
    }

    if(System.IO.File.Exists(filePathDataChar))
        {
          Debug.Log("Active continue button");
          NewGameButton.SetActive(false);
          ContinueButton.SetActive(true);
        }
      else
      {
        Debug.Log("Active new game button");
        NewGameButton.SetActive(true);
        ContinueButton.SetActive(false);
      }

  }


  
  public void LoadGame()
  {
      SceneManager.LoadScene("UiTest");
  }

  public void NewGame()
  {
        string filePath = Application.persistentDataPath + "/CharactersData.json";
        if(System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
        filePath = Application.persistentDataPath + "/DataSave.json";
        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }

        SceneManager.LoadScene("IntroScreen");
  }
  public void QuitGame()
  {
      Application.Quit();
  }
  public void openSettingPanel()
  {
      settingPanel.SetActive(true);
  }

  public void openCreditPanel()
  {
    SceneManager.LoadScene("Credits");
  }
  public void closeCreditPanel()
  {
    creditPanel.SetActive(false);
  }

}
