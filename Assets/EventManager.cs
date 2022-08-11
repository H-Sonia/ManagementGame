using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public bool firstNight = true;

    public bool key1ToothpasteEvent = false;
    public bool key1toothpasteHeld = false;
    public bool key1EndEvent = false;

    public bool key3MedEvent = false;
    public bool key3BadEndEvent = false;
    public bool key3GoodEndEvent = false;
    public int days = 0;

    public Character key1 = null;
    public Character key3 = null;

    public bool key5MetThief = false;
    public bool key5FedThief = false;
    public Character key5 = null;

    public void UpdateDay()
    {
        if(key1EndEvent)
            CharacterManager.instance.RemoveCharacter(key1);

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

    public GameObject quitKey;
    public GameObject uiSection;
    public GameObject ImageSection;
    public GameObject nextButton; 
    public (string, Sprite)[] strings;
    public int stringNo = 0;

    public void resetString()
    {
        stringNo = 0;
    }
    public void UpdateUI()
    {
        Debug.Log("Updating ui");
        print(stringNo);
        print(strings.Length);

        nextButton.SetActive(false);
        quitKey.SetActive(true);
        if (stringNo+1 < strings.Length)
        {
            quitKey.SetActive(false);
            nextButton.SetActive(true);
        }
        if (strings[stringNo].Item2 == null)
        {
            ImageSection.SetActive(false);
            uiSection.SetActive(true);

            uiSection.GetComponent<TMP_Text>().text = strings[stringNo].Item1;
        }
        else
        {
            ImageSection.SetActive(true);
            uiSection.SetActive(false);

            ImageSection.GetComponentInChildren<TMP_Text>().text = strings[stringNo].Item1;
            ImageSection.GetComponent<Image>().sprite = strings[stringNo].Item2;
        }
        stringNo++;
    }

    public eventDetails details;
    public eventDetails saveEvents()
    {
        eventDetails ed = new eventDetails();
        ed.firstNight = firstNight;

        ed.key1ToothpasteEvent = key1ToothpasteEvent;
        ed.key1toothpasteHeld = key1toothpasteHeld;
        ed.key1EndEvent = key1EndEvent;

        ed.key3MedEvent = key3MedEvent;
        ed.key3BadEndEvent = key3BadEndEvent;
        ed.key3GoodEndEvent = key3GoodEndEvent;
        ed.days = days;

        ed.key1 = key1;
        ed.key3 = key3;
        ed.key5 = key5;

        ed.key5MetThief = key5MetThief;
        ed.key5FedThief = key5FedThief;

        Debug.Log("EVENTDETAILS EXIST :" + ed);
        return ed;
    }

    public void loadEvents(eventDetails loadin)
    {
        Debug.Log("Loading");
        if (loadin != null)
        {
            firstNight = loadin.firstNight;

            key1ToothpasteEvent = loadin.key1ToothpasteEvent;
            key1toothpasteHeld = loadin.key1toothpasteHeld;
            key1EndEvent = loadin.key1EndEvent;

            key3MedEvent = loadin.key3MedEvent;
            key3BadEndEvent = loadin.key3BadEndEvent;
            key3GoodEndEvent = loadin.key3GoodEndEvent;
            days = loadin.days;

            key1 = loadin.key1;
            key3 = loadin.key3;
            key5 = loadin.key5;

            key5MetThief = loadin.key5MetThief;
            key5FedThief = loadin.key5FedThief;
        }
    }
}

public class eventDetails
{
    public bool firstNight = true;

    public bool key1ToothpasteEvent = false;
    public bool key1toothpasteHeld = false;
    public bool key1EndEvent = false;

    public bool key3MedEvent = false;
    public bool key3BadEndEvent = false;
    public bool key3GoodEndEvent = false;
    public int days = 0;

    public Character key1 = null;
    public Character key3 = null;

    public bool key5MetThief = false;
    public bool key5FedThief = false;
    public Character key5 = null;
}
