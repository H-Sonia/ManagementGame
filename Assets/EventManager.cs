using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public bool key1EndEvent = false;

    public bool key3MedEvent = false;
    public bool key3BadEndEvent = false;
    public bool key3GoodEndEvent = false;
    public int days = 0;
    public Character key3 = null;

    public bool key5MetThief = false;
    public bool key5FedThief = false;
    public Character key5 = null;

    public void UpdateDay()
    {
        if (key3MedEvent)
            days++;

        if (days > 5)
        {
            CharacterManager.instance.RemoveCharacter(key3);
            print("dead kid");
            key3MedEvent = false;
            key3BadEndEvent = true;
        }
            //kill character
    }
}
