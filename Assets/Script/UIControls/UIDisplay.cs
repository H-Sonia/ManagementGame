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
                giveButtons[i].SetActive(true);
                seeButtons[i].SetActive(true);
                idButtons[i].SetActive(true);
            }
        }
    }

    
}
