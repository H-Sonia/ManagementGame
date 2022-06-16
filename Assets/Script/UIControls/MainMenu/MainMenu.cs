using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject settingPanel;
    public GameObject NewGamePanel;
    public GameObject creditPanel;
  
  public void LoadGame()
  {
      SceneManager.LoadScene("UiTest");
  }

  public void NewGame()
  {
        NewGamePanel.SetActive(true);
  }
  public void QuitGame()
  {
      Application.Quit();
  }
  public void openSettingPanel()
  {
      settingPanel.SetActive(true);
  }

  public void LoadCredit()
  {
      SceneManager.LoadScene("Credit");
  }

}
