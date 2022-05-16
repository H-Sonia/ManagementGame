using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public AudioClip ButtonSound;
    public AudioSource audioSource;

    public void ButtonClick()
    {
        
        audioSource.Play();
    }
}
