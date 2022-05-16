using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public AudioClip MapMusic;
    public AudioClip DormMusic;
    public AudioClip KitchenMusic;
    public AudioSource audioSource;
    void Start()
    {
        audioSource.clip = MapMusic;
        audioSource.Play();

    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
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
