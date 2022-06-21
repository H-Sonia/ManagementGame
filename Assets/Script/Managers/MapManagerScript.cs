using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MapManagerScript : MonoBehaviour
{

    [SerializeField]
    GameObject dorm, boxing, kitchen, dormSleep;
    [SerializeField]
    Button dButton, bButton, kButton;

    [SerializeField]
    Image currImage;
    //8 images? 2 per season
    [SerializeField]
    Sprite[] bgImages;
    int BGID = 0;

    bool isDay = true;
    public bool boxingOpen = true;

    public static MapManagerScript instance;

    private void Start()
    {
        instance = this;
        currImage.sprite = bgImages[BGID];

        if(dorm == null)
            dorm = GameObject.Find("Dorm");

        dormSleep = dorm.transform.GetChild(0).Find("TimeButton").gameObject;
        if(boxing == null)
            boxing = GameObject.Find("Boxing");
        if(kitchen == null)
            kitchen = GameObject.Find("Kitchen");
        StartRoomState();
    }

    //Change room, includes checks to prevent night/daytime access to certain rooms
    public void ChangeRoomState(int num)
    {
        GameObject g = null;
        switch(num)
        {
            case (0):
                dorm.SetActive(false);
                boxing.SetActive(false);
                kitchen.SetActive(false);
                gameObject.SetActive(true);
                return;
            case (1):
                if (!isDay)
                {
                    g = dorm;
                    dormSleep.SetActive(true);
                }
                else
                    return;
                break;
            case (2):
                if (isDay)
                {
                    if(boxingOpen)
                    {
                        Debug.Log("boxing open");
                        g = boxing;
                    }
                }
                else
                    return;
                break;
            case (3):
                if (isDay)
                    g = kitchen;
                else
                    return;
                break;
                
        }
        g.SetActive(!g.activeSelf);
        gameObject.SetActive(!gameObject.activeSelf);

    }
    public void StartRoomState()
    {
        dorm.SetActive(false);
        boxing.SetActive(false);
        kitchen.SetActive(false);
        gameObject.SetActive(true);
    }

    public void ChangeTime(bool firstStart = false)
    {
        isDay = MainManager.instance.isDay;
        //Switch to daytime
        if (isDay)
        {
            //Wakeup in dorm
            ChangeRoomState(0);
            dormSleep.SetActive(false);
            dButton.gameObject.SetActive(false);
            bButton.gameObject.SetActive(false);
            kButton.gameObject.SetActive(true);

            if (boxingOpen)
            {
                bButton.gameObject.SetActive(true);
                kButton.gameObject.SetActive(false);
            }

            BGID = MainManager.instance.season * 2;
        }
        //Switch to nighttime
        else
        {
            dormSleep.SetActive(true);
            dButton.gameObject.SetActive(true);
            kButton.gameObject.SetActive(false);
            bButton.gameObject.SetActive(false);

            if (!dorm.activeInHierarchy)
                ChangeRoomState(0);
            BGID = MainManager.instance.season * 2 + 1;
        }

        if(firstStart)
        {
            ChangeRoomState(0);
            dButton.gameObject.SetActive(false);
            kButton.gameObject.SetActive(false);
            bButton.gameObject.SetActive(true);
        }
        currImage.sprite = bgImages[BGID];
    }
    //Change background on season, add values for events here too
    public void ChangeSeason(int season)
    {
        Debug.Log("NEWSEASON: " + season);
        if(season == 0)
            BGID = 0;
        if (season == 1)
            BGID = 2;
        if (season == 2)
            BGID = 4;
        if (season == 3)
            BGID = 6;
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
