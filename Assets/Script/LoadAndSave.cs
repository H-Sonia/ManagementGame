using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LoadAndSave : MonoBehaviour
{
    public static LoadAndSave instance;
    public PanelController PanelUi;
    public UIDisplay MainUi;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one instance of LoadAndSave in this scene");
            return;
        }
        instance = this;
    }

    public void Start()
    {
        string resourcesSaved = PlayerPrefs.GetString("inventoryResources","");
        string characterSaved = PlayerPrefs.GetString("characters", "");
        string deceasedCharactersSaved = PlayerPrefs.GetString("deceasedCharacters", "");
        string newcomersSaved = PlayerPrefs.GetString("newcomers", "");

       

        //This condition is only here for a test
        if (resourcesSaved != "")
        {
            Inventory.instance.ClearInventory();
        }
        if (newcomersSaved != "")
        {
            Inventory.instance.Newcomers.Clear();
        }

        /*Inventory.instance.ClearInventory();
        Inventory.instance.Newcomers.Clear();*/

        string[] resourcesSavedArray = resourcesSaved.Split(",");
        string[] characterSavedArray = characterSaved.Split(",");
        string[] deceasedCharactersSavedArray = deceasedCharactersSaved.Split(",");
        string[] newcomersSavedArray = newcomersSaved.Split(",");

        for (int i = 0; i < resourcesSavedArray.Length; i++)
        {
            if (resourcesSavedArray[i] != "")
            {
                int id = int.Parse(resourcesSavedArray[i]);
                FoodData currentResource = ResourcesDataBase.instance.allResources.Single(x => x.id == id);
                Inventory.instance.content.Add(currentResource);
            }
        }
        for (int i = 0; i < characterSavedArray.Length; i++)
        {
            if (characterSavedArray[i] != "")
            {
                int id = int.Parse(characterSavedArray[i]);
                CharactersData character = CharacterDatabase.instance.allCharacters.Single(x => x.id == id);
                Inventory.instance.Characters[i] = character;
            }
        }
        for (int i = 0; i < deceasedCharactersSavedArray.Length; i++)
        {
            if (deceasedCharactersSavedArray[i] != "")
            {
                int id = int.Parse(deceasedCharactersSavedArray[i]);
                CharactersData character = CharacterDatabase.instance.allCharacters.Single(x => x.id == id);
                Inventory.instance.DeceasedCharacters.Add(character);
            }
        }
        for(int i=0; i<newcomersSavedArray.Length;i++)
        {
            if(newcomersSavedArray[i] != "")
            {
                int id = int.Parse(newcomersSavedArray[i]);
                Debug.LogWarning(id);
                CharactersData character = CharacterDatabase.instance.allCharacters.Single(x => x.id == id);
                Inventory.instance.Newcomers.Add(character);
            }
        }
        PanelUi.UpdatePanelUI();
    }

    public void SaveData()
    {
        string resourcesInInventory = string.Join(",", Inventory.instance.content.Select(x => x.id));
        PlayerPrefs.SetString("inventoryResources", resourcesInInventory);
        string charactersPresent = string.Join(",", Inventory.instance.Characters.Select(x => x.id));
        PlayerPrefs.SetString("characters", charactersPresent);
        string deceasedCharacters = string.Join(",", Inventory.instance.DeceasedCharacters.Select(x => x.id));
        PlayerPrefs.SetString("deceasedCharacters", deceasedCharacters);
        string newcomers = string.Join(",", Inventory.instance.Newcomers.Select(x => x.id));
        PlayerPrefs.SetString("newcomers", newcomers);
    }
}
