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
        IDpicture.sprite = Inventory.instance.Characters[Inventory.instance.currentCharacter].picture;
        firstname.text = "";
        surname.text = "";
        backstory.text = ""; 

        if(Inventory.instance.Characters[Inventory.instance.currentCharacter].friendshipLevel > 0)
        {
            firstname.text = Inventory.instance.Characters[Inventory.instance.currentCharacter].firstname;
            surname.text = Inventory.instance.Characters[Inventory.instance.currentCharacter].surname;
        }
        if (Inventory.instance.Characters[Inventory.instance.currentCharacter].friendshipLevel > 2)
        {
            backstory.text = Inventory.instance.Characters[Inventory.instance.currentCharacter].infos;
        }
        
    }
    public void Quit()
    {
        IDPanel.SetActive(false);
    }
}
