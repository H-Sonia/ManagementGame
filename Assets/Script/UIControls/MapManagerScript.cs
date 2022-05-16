using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MapManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject dorm, Boxing, Kitchen, Map;
    public TMP_Text playerprefs; 

    // Start is called before the first frame update
    void Start()
    {
        string resourcesSaved = PlayerPrefs.GetString("inventoryResources", "");
        string characterSaved = PlayerPrefs.GetString("characters", "");
        string deceasedCharactersSaved = PlayerPrefs.GetString("deceasedCharacters", "");
        string newcomersSaved = PlayerPrefs.GetString("newcomers", "");

        playerprefs.text += "resource : " + resourcesSaved + "\n";
        playerprefs.text += "character : " + characterSaved + "\n"; 

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeRoomState(GameObject gin)
    {
        gin.SetActive(!gin.activeInHierarchy);
        Map.SetActive(!Map.activeInHierarchy);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
