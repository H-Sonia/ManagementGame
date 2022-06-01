using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioClip MapMusic;
    public AudioClip DormMusic;
    public AudioClip KitchenMusic;
    public AudioClip ArenaMusic;
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

        audioSource.clip = MapMusic;
        audioSource.Play();

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
                audioSource.clip = DormMusic;
                audioSource.Play();
                break;
            case 1:
                audioSource.clip = MapMusic;
                audioSource.Play();
                break;
            case 2:
                audioSource.clip = KitchenMusic;
                audioSource.Play();
                break;
            case 3:
                audioSource.clip = ArenaMusic;
                audioSource.Play();
                break;
            default:
                Debug.LogWarning("Room index out of range");
                break;
        }
    }




    public void PlayDormMusic()
    {
        audioSource.clip = DormMusic;
        audioSource.Play();
    }

    public void PlayMapMusic()
    {
        audioSource.clip = MapMusic;
        audioSource.Play();
    }

    public void PlayKitchenMusic()
    {
        audioSource.clip = KitchenMusic;
        audioSource.Play();
    }

}
