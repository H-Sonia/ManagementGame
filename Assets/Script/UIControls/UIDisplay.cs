using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;


public class UIDisplay : MonoBehaviour
{
    public TMP_Text[] names;
    public GameObject[] giveButtons;
    public GameObject[] idButtons;
    public CharactersData empty;
    public GameObject infoPanel;
    public ScrollRect scrollRect;
    public TMP_Text infos;

    
    public void UpdateMainUi(string characterMessage, bool opening)
    {
        if (opening)
        {
            scrollRect.verticalNormalizedPosition = 1f;
            infoPanel.SetActive(true);
        }
        else
        {
            if(characterMessage != "")
            {
                infos.text = characterMessage;
                scrollRect.verticalNormalizedPosition = 1f; 
                infoPanel.SetActive(true);
            }
        }
        

        for (int i = 0; i < names.Length; i++)
        {
            Character character = CharacterManager.instance.charactersLists.CharactersInDorm[i];

            
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
