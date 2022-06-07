using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioClip KitchenAmbiance;
    public AudioClip ArenaAmbiance;
    public AudioClip NightMusic;
    public AudioClip SpringDayAmbiance;
    public AudioClip WinterDayAmbiance;
    public AudioClip SpringNightAmbiance;
    public AudioClip WinterNightAmbiance;
    public AudioSource audioSource;
    // 0 = map, 1 = dorm, 2 = kitchen, 3 = arena
    int room;

    [SerializeField]
    GameObject dorm, boxing, kitchen;

    void Start()
    {
        dorm = GameObject.Find("Dorm");
        boxing = GameObject.Find("Boxing");
        kitchen = GameObject.Find("Kitchen");

    }

    // Update is called once per frame
    void Update()
    {
        int whichRoom = inWhichRoom();
        if (room != whichRoom)
        {
            room = whichRoom;
            ChangeMusic(room);
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        
    }

    public int inWhichRoom()
    {
        if(dorm.activeSelf)
        {
            return 1;
        }
        if(kitchen.activeSelf)
        {
            return 2;
        }
        if(boxing.activeSelf)
        {
            return 3;
        }
        return 0;
    }

    public void ChangeMusic(int whichRoom)
    {
        switch(whichRoom)
        {
            case 0:
                DayNightAmbiance();
                audioSource.Play();
                break;
            case 1:
                DayNightAmbiance();
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = KitchenAmbiance;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = ArenaAmbiance;
                audioSource.Play();
                break;
            default:
                Debug.LogWarning("Room index out of range");
                break;
        }
    }

    public void DayNightAmbiance()
    {
        if(MainManager.instance.isDay)
        {
            if(MainManager.instance.season == 1 || MainManager.instance.season == 0)
            {
                audioSource.clip = SpringDayAmbiance;
            }
            else
            {
                audioSource.clip = WinterDayAmbiance;
            }
        }
        else
        {
            if (MainManager.instance.season == 1 || MainManager.instance.season == 0)
            {
                audioSource.clip = SpringNightAmbiance;
            }
            else
            {
                audioSource.clip = WinterDayAmbiance;
            }
        }
    }

}
