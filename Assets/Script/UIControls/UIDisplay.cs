using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIDisplay : MonoBehaviour
{
    public TMP_Text[] names;
    public GameObject[] giveButtons;
    public GameObject[] idButtons;
    public CharactersData empty;
    public GameObject infoPanel;
    public TMP_Text infos;

    public void DayFunction()
    {

    }

    public void UpdateMainUi(string characterMessage, bool opening)
    {
        if (opening)
        {
            infoPanel.SetActive(true);
        }
        else
        {
            if(characterMessage != "")
            {
                infos.text = characterMessage;
                infoPanel.SetActive(true);
            }
        }
        

        for (int i = 0; i < names.Length; i++)
        {
            CharactersData character = Inventory.instance.Characters[i];

            
            if (character.id == 0)
            {

                names[i].text = "";
                giveButtons[i].SetActive(false);
                idButtons[i].SetActive(false);
            }
            else
            {
                if (character.friendshipLevel > 0)
                {
                    names[i].text = character.firstname + " " + character.surname;
                    giveButtons[i].SetActive(true);
                    idButtons[i].SetActive(true);
                }
                else
                {
                    names[i].text = " ? ? ? ";
                    giveButtons[i].SetActive(true);
                    idButtons[i].SetActive(true);
                }
            }
        }
    }

    public void QuitInfoPanel()
    {
        infoPanel.SetActive(false);
        infos.text = "";
    }

    
}
