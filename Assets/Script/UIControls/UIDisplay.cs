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

    public GameObject Dorm;
    public void Setup()
    {
        Dorm = GameObject.Find("Dorm");
        if (Dorm != null)
        {
            names = Dorm.transform.GetChild(0).Find("Names").GetComponentsInChildren<TMP_Text>();
            Transform[] children;

            children = Dorm.transform.GetChild(0).Find("GiveButtons").GetComponentsInChildren<Transform>();
            giveButtons = new GameObject[(children.Length-1)/2];
            int j = 0;
            for (int i = 1; i < children.Length; i++)
            {
                int temp = j;
                if (children[i].name.Contains("give"))
                {
                    giveButtons[j] = children[i].gameObject;
                    children[i].GetComponent<Button>().onClick.AddListener(()=>GiveButtonClick(temp));
                    j++;
                }
            }

            children = Dorm.transform.GetChild(0).Find("IDButtons").GetComponentsInChildren<Transform>();
            idButtons = new GameObject[(children.Length-1) / 2];
            j = 0;
            for (int i = 1; i < children.Length; i++)
            {
                int temp = j;
                if (children[i].name.Contains("ID"))
                {
                    idButtons[j] = children[i].gameObject;
                    children[i].GetComponent<Button>().onClick.AddListener(() => IDButtonClick(temp));
                    j++;
                }
            }
        }
        MainManager.instance.MainCheck();
    }

    public void IDButtonClick(int id)
    {
        Debug.Log(id);
        GetComponent<ButtonController>().openIdPanel(id);
    }

    public void GiveButtonClick(int id)
    {
        GetComponent<ButtonController>().openResourcesPanel(id);
    }

    public void DayFunction()
    {

    }

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
