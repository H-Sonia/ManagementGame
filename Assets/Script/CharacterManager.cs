using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public CharacterDataLists charactersLists = new CharacterDataLists();

    public void Start()
    {
        int firstTimeInDorms = PlayerPrefs.GetInt("firstTimeInDorms", 0);
        if (firstTimeInDorms == 0)
        {
            InitializeCharactersData();
        }
        else
        {
            LoadFromJson();
        }

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            SaveToJson();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadFromJson();
        }
    }

    public void InitializeCharactersData()
    {
        FillTrueNewcomers();
        GetAllFriends();
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
        try
        {
            string filePath = Application.persistentDataPath + "/CharactersData.json";
            string charactersListsData = System.IO.File.ReadAllText(filePath);
            charactersLists = JsonUtility.FromJson<CharacterDataLists>(charactersListsData);
            Debug.Log("Data loaded");
        }
        catch
        {
            Debug.Log("NO SAVE DATA FOUND, CREATING NEW");
            InitializeCharactersData();
        }
    }
}

[System.Serializable]
public class CharacterDataLists
{
    public int DormCapacity = 18;
    public List<Character> CharactersInDorm = new List<Character>();
    public List<Character> TrueNewcomers = new List<Character>();
    public List<Character> DeadCharacters = new List<Character>();
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