using System;
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
    public void QuitWithUpdate()
    {
        ResourcesPanel.SetActive(false);
        MainUI.UpdateMainUi("UPDATE", false);
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
            if (!Inventory.instance.content.Contains(currentResource))
            {
                Debug.Log("NOT ENOUGH");
                return;
            }
            Character curr = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter];
            //dirty system to get it implemented in time
            //check for Key character
            characterMessage = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].firstname + " thanks you.\n";
            bool cont = true;

            bool updatingUI = false;

            if (EventManager.instance.key1EndEvent)
            {
                characterMessage = curr.firstname + " thanks me. “Have you seen Alberto?” I ask. \n" +
                    " “Haven’t you heard? He tried to escape by jumping into Vistula River. They caught him, tortured him, and killed him.” \n";
                updatingUI = true;

                (string, Sprite)[] s = new (string, Sprite)[2];
                s[0] = (curr.firstname + " thanks me. “Have you seen Alberto?” I ask. \n" +
                    "“Haven’t you heard? He tried to escape by jumping into Vistula River. They caught him, tortured him, and killed him.” \n", null);
                s[1] = ("After the liberation, I learnt that Alberto’s pictures managed to be smuggled outside the camps. \n" +
                    "They are the only pictorial evidence in existence taken by a prisoner of what was happening around the gas chambers.", null);

                EventManager.instance.strings = s;
                EventManager.instance.key1EndEvent = false;
            }
            else if (!curr.fedToday && curr.isKey)
            {
                int stage = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].keyStage;

                if (curr.id == 1)
                {
                    if (stage == 2)
                        EventManager.instance.key1ToothpasteEvent = true;
                    if(stage == 3)
                    {
                        if (!EventManager.instance.key1toothpasteHeld)
                            cont = false;
                        else
                        {
                            EventManager.instance.key1toothpasteHeld = false;
                            EventManager.instance.key1ToothpasteEvent = false;
                        }
                    }

                    if (stage == 4)
                    {
                        updatingUI = true;
                        (string, Sprite)[] s = new (string, Sprite)[6];
                        characterMessage = "I share some food with Alberto. \n" +
                            " “Thank you,” he says, and takes something out of his worn clogs. It’s the toothpaste tube I’d given him some days ago. “Take a look,” he whispers.";

                        s[0] = ("I share some food with Alberto. \n" +
                            " “Thank you,” he says, and takes something out of his worn clogs. It’s the toothpaste tube I’d given him some days ago. “Take a look,” he whispers." , null);
                        s[1] = ("He unfolds the aluminium tube and reveals a small reel of film.It’s so fragile I am afraid to touch it.I hold it to the light and try to discern what it depicts.  \n", Resources.Load<Sprite>("KeySprites/AE1"));
                        s[2] = ("He unfolds the aluminium tube and reveals a small reel of film.It’s so fragile I am afraid to touch it.I hold it to the light and try to discern what it depicts.  \n", Resources.Load<Sprite>("KeySprites/AE2"));
                        s[3] = ("He unfolds the aluminium tube and reveals a small reel of film.It’s so fragile I am afraid to touch it.I hold it to the light and try to discern what it depicts.  \n", Resources.Load<Sprite>("KeySprites/AE3"));
                        s[4] = ("He unfolds the aluminium tube and reveals a small reel of film.It’s so fragile I am afraid to touch it.I hold it to the light and try to discern what it depicts.  \n", Resources.Load<Sprite>("KeySprites/AE1"));
                        s[5] = ("“How did you manage to take these?” I exclaim. The Nazis try to keep their atrocities hidden from everyone, even the prisoners in the camps. \n " +
                            "He smiles ruefully. “I told you to have faith.” ", null);
                        EventManager.instance.strings = s;
                        EventManager.instance.key1EndEvent = true;
                    }
                    //key1EndEvent = true;
                    if (stage >= 5)
                    {
                        CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].isKey = false;
                    }
                }

                else if (curr.id == 2)
                {
                    if (stage == 1)
                    {
                        print("SECOND STAGE");
                        updatingUI = true;

                        (string,Sprite)[] s = new (string,Sprite)[2];

                        characterMessage = curr.firstname + " thanks me. “Are you married ?” he asks. \n " +
                            "I shake my head. \n" +
                            "“I miss my wife. I haven’t seen her since we arrived and they separated us. Do you think she’s still alive?” \n" +
                            "I hesitate. \n" +
                            "He looks away. “Maybe it’s for the better,” he says. \n" +
                            "“What is?” \n";


                        s[0] = (curr.firstname + " thanks me. “Are you married ?” he asks. \n " +
                            "I shake my head. \n" +
                            "“I miss my wife. I haven’t seen her since we arrived and they separated us. Do you think she’s still alive?” \n" +
                            "I hesitate. \n" +
                            "He looks away. “Maybe it’s for the better,” he says. \n" +
                            "“What is?” \n", null);

                        s[1] = ("That you are not married.", null);
                        EventManager.instance.strings = s;
                    }
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

                    if (stage == 0)
                    {
                        (string, Sprite)[] s = new (string, Sprite)[2];
                        updatingUI = true;
                        characterMessage = curr.firstname + " thanks me.";
                        if (EventManager.instance.key5MetThief == false)
                        {
                            characterMessage = "Behind me, a few other prisoners protest. “Don’t give anything to him,” they say. “He’s a thief.He steals everything from us.”";
                            s[0] = (characterMessage, null);
                            s[1] = ("I must choose whether to feed this so called thief or not", null);
                            EventManager.instance.strings = s;

                            EventManager.instance.key5MetThief = true;

                            UpdatePanelUI();
                            EventManager.instance.UpdateUI();
                            Quit();
                            return;
                        }
                        else if (EventManager.instance.key5MetThief == true)
                        {
                            print("THIEF 2");
                            characterMessage = " “How can you give him food when you know he’s a thief?” another inmate asks as I give" + curr.firstname + "some food. \n" +
                                "“You’re letting others starve.” \n" +
                                "“My choices are not right or wrong.They are hardly choices.” \n" +
                                "“That doesn’t free you from any responsibility.”\n" +
                                "He walks away, frowning.";

                            s[0] = (" “How can you give him food when you know he’s a thief?” another inmate asks as I give " + curr.firstname + " some food. “You’re letting others starve.” \n" +
                                "“My choices are not right or wrong.They are hardly choices.” \n" +
                                "“That doesn’t free you from any responsibility.” He walks away, frowning.", null);

                            s[1] = (curr.firstname + " thanks me. ", null);

                            EventManager.instance.strings = s;
                        }
                    }

                    //characterMessage = keylist5[stage];
                    if (stage >= 1)
                    {
                        CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].isKey = false;
                    }
                }

                if (cont && stage < curr.message.Length && !updatingUI)
                {
                    characterMessage = "";
                    foreach (string s in curr.message[stage].sentences)
                    {
                        s.Replace("[name]", curr.firstname);
                        s.Replace("[Name]", curr.firstname);

                        characterMessage += s + "\n";
                    }
                }
                else
                {
                    characterMessage = curr.firstname + " thanks me";
                }

                characterMessage.Replace("[name]", curr.firstname);
                characterMessage.Replace("[Name]", curr.firstname);
            }

            CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].resourcesAttribuated.Add(currentResource);
            CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].daysBeforeExpiration.Add(currentResource.daysBeforeExpiration);
            CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].friendshipLevel += 1;
            Inventory.instance.content.Remove(currentResource);
            curr.fedToday = true;
            GetNextResources();

            if (curr.isKey && cont)
            {
                curr.keyStage += 1;
            }

            UpdatePanelUI();

            if (updatingUI)
            {  
                print("updatingUi");
                EventManager.instance.UpdateUI();
                //QuitWithUpdate();
            }
            Quit();

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            characterMessage = "";
        }

    }

}
