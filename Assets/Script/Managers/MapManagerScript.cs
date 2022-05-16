using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManagerScript : MonoBehaviour
{
    [SerializeField]
    GameObject dorm, boxing, kitchen;
    [SerializeField]
    Button dButton, bButton, kButton;

    [SerializeField]
    Image currImage;
    //8 images? 2 per season
    [SerializeField]
    Sprite[] bgImages;
    int BGID = 0;

    bool isDay;
    public bool boxingOpen = false;

    private void Start()
    {
        currImage.sprite = bgImages[BGID];
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
                    g = dorm;
                else
                    return;
                break;
            case (2):
                if (isDay && boxingOpen)
                    g = boxing;
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

    public void ChangeTime(bool isDayt)
    {
        isDay = isDayt;
        //Switch to nighttime
        if (isDayt)
        {
            //Wakeup in dorm
            ChangeRoomState(1);
            dButton.gameObject.SetActive(false);
            kButton.gameObject.SetActive(true);
            if(boxingOpen)
                bButton.gameObject.SetActive(true);

            BGID -= 1;
        }
        //Switch to daytime
        else
        {
            dButton.gameObject.SetActive(true);
            kButton.gameObject.SetActive(false);
            bButton.gameObject.SetActive(false);

            if (!dorm.activeInHierarchy)
                ChangeRoomState(0);
            BGID += 1;
        }

        currImage.sprite = bgImages[BGID];
    }
    //Change background on season, add values for events here too
    public void ChangeSeason(int season)
    {
        if(season == 0)
            BGID = 0;
        if (season == 1)
            BGID = 2;
        if (season == 2)
            BGID = 4;
        if (season == 3)
            BGID = 6;
    }
}
