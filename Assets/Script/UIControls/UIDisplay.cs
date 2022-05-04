using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class UIDisplay : MonoBehaviour
{
    public TMP_Text[] names;
    public GameObject[] giveButtons;
    public GameObject[] seeButtons;
    public GameObject[] idButtons;
    public CharactersData empty;

    void Start()
    {
        UpdateCheck();
    }

    public void UpdateCheck()
    {
        System.Random random = new System.Random();
        int probability = random.Next(100);
        if (probability > 70)
        {
            SomeoneDisappears();
        }
        System.Random random2 = new System.Random();
        int probability2 = random2.Next(100);
        if (probability2 > 70)
        {
            SomeoneAppears();
        }
        UpdateMainUi();
    }

    public void UpdateMainUi()
    {
        for (int i = 0; i < names.Length; i++)
        {
            CharactersData character = Inventory.instance.Characters[i];


            if (character.id == 0)
            {

                names[i].text = "";
                giveButtons[i].SetActive(false);
                seeButtons[i].SetActive(false);
                idButtons[i].SetActive(false);
            }
            else
            {
                names[i].text = character.firstname + " " + character.surname;
            }
        }
    }

    void SomeoneDisappears()
    {
        System.Random random = new System.Random();
        int index = random.Next(12);
        if (Inventory.instance.Characters[index].id != 0)
        {
            Inventory.instance.DeceasedCharacters.Add(Inventory.instance.Characters[index]);
            Inventory.instance.Characters[index] = empty;
        }
    }

    int IsEmptySpot()
    {
        for(int i=0; i < Inventory.instance.Characters.Length;i++)
        {
            if(Inventory.instance.Characters[i].id == 0)
            {
                return i;
            }
        }
        return -1;
    }

    void SomeoneAppears()
    {
        int index = IsEmptySpot();
        Debug.LogWarning("in someoneAppears");
        Debug.LogWarning(Inventory.instance.Newcomers.Count);

        if (Inventory.instance.Newcomers.Count <= 0)
        {
            Debug.LogWarning("new commers vide");
            return;
        }

        if(index != -1)
        {
            Debug.LogWarning("Pas vide");
            Inventory.instance.Characters[index] = Inventory.instance.Newcomers[0];
            Inventory.instance.Newcomers.Remove(Inventory.instance.Newcomers[0]);
            //Debug.LogWarning(Inventory.instance.Newcomers.Count);
            Debug.LogWarning("Here");
            for (int i = 0; i < Inventory.instance.Newcomers.Count; i++)
            {
                Debug.LogWarning(Inventory.instance.Newcomers[i].firstname);
            }
            Debug.LogWarning("Loop end");
            //MainUi.UpdateMainUi();
        }
    }
}
