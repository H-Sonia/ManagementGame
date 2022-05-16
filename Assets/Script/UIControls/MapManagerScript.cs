using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject dorm, Boxing, Kitchen, Map;

    // Start is called before the first frame update
    void Start()
    {
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
