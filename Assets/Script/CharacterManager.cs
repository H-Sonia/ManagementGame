using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CharacterManager : MonoBehaviour
{
    public CharacterDataLists charactersLists = new CharacterDataLists();
    public Sprite netralPicture;
    public UIDisplay ui;

    public static CharacterManager instance;


    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one Character Manager instance in this scene");
            return;
        }
        instance = this;
    }

    public void Start()
    {
        InitializeCharactersData();
        ui.UpdateMainUi("", false);
    }

    public void UpdateCharacterLists()
    {
        /*int firstTimeInDorms = PlayerPrefs.GetInt("firstTimeInDorms", 0);
        if (firstTimeInDorms != 0)
        {
            string sickPeolple = "";
            string nbOfPeopleDisappearing = "";
            string friendsWhoDisappeared = "";
            //UpdateCharactersResources();
            *//*UpdateCharactersState(ref sickPeolple);
            UpdateCharactersPresent(ref nbOfPeopleDisappearing, ref friendsWhoDisappeared);
            infos.text = nbOfPeopleDisappearing + friendsWhoDisappeared + sickPeolple;
            ui.UpdateMainUi("", true);*//*
        }*/
    }

    public void InitializeCharactersData()
    {
        FillTrueNewcomers();
        GetAllFriends();
        FillNameNotUsed();
        FillWithPlaceHolders();
        charactersLists.CharactersInDorm =  ShuffleCharacterList(charactersLists.CharactersInDorm);
    }

    public void FillTrueNewcomers()
    {
        for (int i = 0; i < CharacterDB.instance.db.allTrueCharacters.Length; i++)
        {
            if(!CharacterDB.instance.db.allTrueCharacters[i].alreadyKnown)
            {
                charactersLists.TrueNewcomers.Add(CharacterDB.instance.db.allTrueCharacters[i]);
            }
            
        }
    }

    public void GetAllFriends()
    {
        for (int i = 0; i < CharacterDB.instance.db.allTrueCharacters.Length; i++)
        {
            if (CharacterDB.instance.db.allTrueCharacters[i].alreadyKnown)
            {
                charactersLists.CharactersInDorm.Add(CharacterDB.instance.db.allTrueCharacters[i]);
            }

        }
    }

    public void FillNameNotUsed()
    {
        for (int i = 0; i < CharacterDB.instance.db.FirstnameForPlaceHolder.Length; i++)
        {
            charactersLists.nameNotUsed.Add(i);
        }
    }

    public void FillWithPlaceHolders()
    {
        int nbEmptyBeds = charactersLists.DormCapacity - charactersLists.CharactersInDorm.Count;
        System.Random random = new System.Random();

        for (int i=0; i < nbEmptyBeds; i++)
        {
            int index = random.Next(charactersLists.nameNotUsed.Count);
            string firstname = CharacterDB.instance.db.FirstnameForPlaceHolder[charactersLists.nameNotUsed[index]];
            string lastname = CharacterDB.instance.db.LastNameForPlaceHolder[charactersLists.nameNotUsed[index]];
            charactersLists.nameNotUsed.Remove(charactersLists.nameNotUsed[index]);

            Character character = new Character();
            character.alreadyKnown = false;
            character.surviveUntilTheEnd = false;
            character.id = -1;
            character.firstname = firstname;
            character.surname = lastname;
            character.picture = netralPicture;
            character.health = 100;
            character.efficiencyAtWork = 50;
            character.resourcesAttribuated = new List<ItemData>();
            character.daysBeforeExpiration = new List<int>();

    charactersLists.CharactersInDorm.Add(character);
        }
    }

    public List<Character> ShuffleCharacterList(List<Character> listToShuffle)
    {
        System.Random rd = new System.Random();
        var shuffledList = listToShuffle.OrderBy(item => rd.Next()).ToList();
        return shuffledList;
    }

    public void UpdateCharactersResources()
    {
        for (int i = 0; i < charactersLists.CharactersInDorm.Count; i++)
        {
            for (int j = 0; j < charactersLists.CharactersInDorm[i].resourcesAttribuated.Count; j++)
            {
                ConsumeItem(i, j);
                charactersLists.CharactersInDorm[i].daysBeforeExpiration[j]--;
                if (charactersLists.CharactersInDorm[i].daysBeforeExpiration[j] <= 0)
                {
                    charactersLists.CharactersInDorm[i].resourcesAttribuated.RemoveAt(j);
                    charactersLists.CharactersInDorm[i].daysBeforeExpiration.RemoveAt(j);

                }
            }
        }
    }


    public void ConsumeItem(int indexCharacter, int indexItem)
    {
        switch (charactersLists.CharactersInDorm[indexCharacter].resourcesAttribuated[indexItem].type)
        {
            case "food":
                charactersLists.CharactersInDorm[indexCharacter].hunger -= charactersLists.CharactersInDorm[indexCharacter].resourcesAttribuated[indexItem].nutritiveValue;
                break;
            case "clothe":
                charactersLists.CharactersInDorm[indexCharacter].cold -= charactersLists.CharactersInDorm[indexCharacter].resourcesAttribuated[indexItem].heat;
                if (charactersLists.CharactersInDorm[indexCharacter].efficiencyAtWork < 100)
                {
                    charactersLists.CharactersInDorm[indexCharacter].efficiencyAtWork += charactersLists.CharactersInDorm[indexCharacter].resourcesAttribuated[indexItem].efficiencyAtWork;
                }
                break;
            case "medicine":
                charactersLists.CharactersInDorm[indexCharacter].isSick = false;
                if (charactersLists.CharactersInDorm[indexCharacter].health < 100)
                {
                    charactersLists.CharactersInDorm[indexCharacter].health += charactersLists.CharactersInDorm[indexCharacter].resourcesAttribuated[indexItem].health;
                }
                break;
            default:
                Debug.LogWarning("unknown type");
                break;
        }
    }



    public void SaveToJson()
    {
        string charactersListsData = JsonUtility.ToJson(charactersLists);
        string filePath = Application.persistentDataPath + "/CharactersData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, charactersListsData);
        Debug.Log("Data saved");
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/CharactersData.json";
        string charactersListsData = System.IO.File.ReadAllText(filePath);
        charactersLists = JsonUtility.FromJson<CharacterDataLists>(charactersListsData);
        Debug.Log("Data loaded");
    }
}

[System.Serializable]
public class CharacterDataLists
{
    public int DormCapacity = 18;
    public int currentCharacter; 
    public List<Character> CharactersInDorm = new List<Character>();
    public List<Character> TrueNewcomers = new List<Character>();
    public List<Character> DeadCharacters = new List<Character>();
    public List<int> nameNotUsed = new List<int>();
}

[System.Serializable]
public class Character
{
    public bool alreadyKnown;
    public bool surviveUntilTheEnd;
    public int id;
    public string firstname;
    public string surname;
    public Sprite picture;
    public string infos;
    public TextData[] message;
    public int friendshipLevel;
    public List<ItemData> resourcesAttribuated;
    public List<int> daysBeforeExpiration;
    public int hunger;
    public int cold;
    public bool isSick;
    public int health;
    public int efficiencyAtWork;
}