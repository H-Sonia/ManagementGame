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
        firstname.text = Inventory.instance.Characters[Inventory.instance.currentCharacter].firstname;
        surname.text = Inventory.instance.Characters[Inventory.instance.currentCharacter].surname;
        backstory.text = Inventory.instance.Characters[Inventory.instance.currentCharacter].infos;
        IDpicture.sprite = Inventory.instance.Characters[Inventory.instance.currentCharacter].picture;
    }
    public void Quit()
    {
        IDPanel.SetActive(false);
    }
}
