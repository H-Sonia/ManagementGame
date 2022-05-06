using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public GameObject settingPanel;
  
  public void LoadGame()
  {
      SceneManager.LoadScene("UiTest");
  }
  public void QuitGame()
  {
      Application.Quit();
  }
  public void openSettingPanel()
  {
      settingPanel.SetActive(true);
  }
}
