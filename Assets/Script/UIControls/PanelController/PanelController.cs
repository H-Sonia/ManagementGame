using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PanelController : MonoBehaviour
{
    public GameObject ResourcesPanel;
    public TMP_Text ResourceDescription;
    public TMP_Text ResourcesCounter;
    public Image RessourceImage;
    public Sprite emptyImage;
    public UIDisplay MainUI;
    string characterMessage;


    public void Setup()
    {
        UpdatePanelUI();
        MainManager.instance.MainCheck();
    }
   

    public void Quit()
    {
        ResourcesPanel.SetActive(false);
        if(characterMessage != "")
            MainUI.UpdateMainUi(characterMessage, false);
        characterMessage = "";
    }
    public void Quit2()
    {
        ResourcesPanel.SetActive(false);
        MainUI.UpdateMainUi(characterMessage, false);
        characterMessage = "";
    }

    public void UpdatePanelUI()
    {
        int count = Inventory.instance.content.Count;
        ResourcesCounter.text = count.ToString();

        if (Inventory.instance.content.Count > 0)
        {
            ResourceDescription.text = Inventory.instance.content[Inventory.instance.currentResource].itemName;
            RessourceImage.sprite = Inventory.instance.content[Inventory.instance.currentResource].itemImage;
        }
        else 
        {
            ResourceDescription.text = "";
            RessourceImage.sprite = emptyImage;
        }

    }

    public void GetNextResources()
    {
        if(Inventory.instance.content.Count == 0)
        {
            return;
        }

        Inventory.instance.currentResource++;
        if(Inventory.instance.currentResource > Inventory.instance.content.Count - 1)
        {
            Inventory.instance.currentResource = 0;
        }

        UpdatePanelUI();
    }

    public void GetPreviousResources()
    {
        if (Inventory.instance.content.Count == 0)
        {
            return;
        }

        Inventory.instance.currentResource--;
        if (Inventory.instance.currentResource < 0)
        {
            Inventory.instance.currentResource = Inventory.instance.content.Count - 1;
        }

        UpdatePanelUI();
    }

    //would prefer an array of arrays but need to get it implemented quickly


    public void GiveResources()
    {
        try
        {
            ItemData currentResource = Inventory.instance.content[Inventory.instance.currentResource];

            Character curr = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter];
            //dirty system to get it implemented in time
            //check for Key character
            characterMessage = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].firstname + " thanks you.\n";
            Debug.Log(curr.fedToday);
            Debug.Log(curr.isKey);
            if (EventManager.instance.key1EndEvent)
            {
                characterMessage = curr.firstname + " thanks me. “Have you seen Alberto?” I ask. \n" +
                    " “Haven’t you heard? He tried to escape by jumping into Vistula River. They caught him, tortured him, and killed him.” \n";
                /*+
                    "[new screen] After the liberation, I learnt that Alberto’s pictures managed to be smuggled outside the camps. \n" +
                    "They are the only pictorial evidence in existence taken by a prisoner of what was happening around the gas chambers.";*/
                EventManager.instance.key1EndEvent = false;
            }
            else if (!curr.fedToday && curr.isKey)
            {
                bool cont = true;
                int stage = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].keyStage;

                Debug.Log(stage);
                Debug.Log(curr.message.Length);
                if (curr.id == 1)
                {
                    if (stage == 4)
                        EventManager.instance.key1EndEvent = true;
                        //key1EndEvent = true;
                    if (stage >= 5)
                    {
                        CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].isKey = false;
                    }
                }
                else if (curr.id == 2)
                {
                    if (stage >= 2)
                    {
                        CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].isKey = false;
                    }
                }
                else if (curr.id == 3) // needs diverging events
                {
                    if (stage == 2)
                        EventManager.instance.key3MedEvent = true;

                    //works for medicine being found but no way to countdown
                    if (EventManager.instance.key3MedEvent)
                    {
                        if (currentResource.id == 4)
                        {
                            EventManager.instance.key3MedEvent = false;
                            EventManager.instance.key3GoodEndEvent = true;
                        }
                        else
                        {
                            cont = false;
                        }
                    }

                    if (stage >= 5)
                    {
                        CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].isKey = false;
                    }
                }
                else if (curr.id == 4)
                {
                    // characterMessage = keylist4[stage];
                    if (stage >= 1)
                    {
                        CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].isKey = false;
                    }
                }
                else if (curr.id == 5)
                {
                    //option at stage 1
                    if (stage == 0)
                        EventManager.instance.key5MetThief = true;

                    if (EventManager.instance.key5MetThief)
                        cont = true;

                    //characterMessage = keylist5[stage];
                    if (stage >= 3)
                    {
                        CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].isKey = false;
                    }
                }

                if (cont && stage < curr.message.Length)
                {
                    characterMessage = "";
                    foreach (string s in curr.message[stage].sentences)
                    {
                        s.Replace("[name]", curr.firstname);

                        characterMessage += s + "\n";
                    }
                }
            }
            else
                Debug.Log("NOT KEY");
             
            CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].resourcesAttribuated.Add(currentResource);
            CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].daysBeforeExpiration.Add(currentResource.daysBeforeExpiration);
            CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].friendshipLevel += 1;
            Inventory.instance.content.Remove(currentResource);
            curr.fedToday = true;
            GetNextResources();
            if (curr.isKey)
            {
                curr.keyStage += 1;
            }
            UpdatePanelUI();
            Quit();

        }
        catch
        {
            Debug.Log("ERROR");
            characterMessage = "";
        }

    }

}
