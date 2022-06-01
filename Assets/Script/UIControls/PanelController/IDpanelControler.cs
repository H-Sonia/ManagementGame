using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IDpanelControler : MonoBehaviour
{
    public GameObject IDPanel;
    public TMP_Text IDnumber;
    public TMP_Text firstname;
    public TMP_Text surname;
    public TMP_Text backstory;
    public Image IDpicture;

    public void DisplayID()
    {
        Debug.LogWarning("in DisplayID");
        IDpicture.sprite = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].picture;
        firstname.text = "";
        surname.text = "";
        backstory.text = ""; 

        if(CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].friendshipLevel > 0)
        {
            firstname.text = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].firstname;
            surname.text = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].surname;
        }
        if (CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].friendshipLevel > 2)
        {
            backstory.text = CharacterManager.instance.charactersLists.CharactersInDorm[CharacterManager.instance.charactersLists.currentCharacter].infos;
        }
        
    }
    public void Quit()
    {
        IDPanel.SetActive(false);
    }
}
