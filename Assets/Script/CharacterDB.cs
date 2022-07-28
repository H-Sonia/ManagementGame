using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDB : MonoBehaviour
{
    public DataBaseCharacter db = new DataBaseCharacter();
    public Sprite[] allTrueCharactersPicture;
    public string[] FirstnameForPlaceHolder;
    public string[] LastNameForPlaceHolder;

    public static CharacterDB instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of CharacterDataBase in this scene");
            return;
        }
        instance = this;
    }

    public void Start()
    {
        LoadJsonFile(); 
        LoadAllPicture();
        string filePath = Application.persistentDataPath + "/CharactersData.json";
        if(!System.IO.File.Exists(filePath))
        {
            InitCharacterManager();
        }
        
    }

    public void InitCharacterManager()
    {
        CharacterManager.instance.charactersLists.DaysPassedSinceLastTrueCharacter = 0;
        CharacterManager.instance.charactersLists.NbMaxTrueCharacter = 3;
        CharacterManager.instance.charactersLists.DormCapacity = 18;
        //FillTrueNewcomers();
        GetAllFriends();
    }

    public void LoadJsonFile()
    {
        string filePath = Application.dataPath + "/Json/TrueCharacterData.json";
        string initialDB = System.IO.File.ReadAllText(filePath);
        db = JsonUtility.FromJson<DataBaseCharacter>(initialDB);
        Debug.Log("Data loaded");
    }

    public void LoadAllPicture()
    {
       for (int i = 0; i < db.allTrueCharacters.Length; i++)
        {
            db.allTrueCharacters[i].picture = allTrueCharactersPicture[i];
        }
    }


    public void FillTrueNewcomers()
    {
        for (int i = 0; i < db.allTrueCharacters.Length; i++)
        {
            if(!db.allTrueCharacters[i].alreadyKnown)
            {
                CharacterManager.instance.charactersLists.TrueNewcomers.Add(db.allTrueCharacters[i]);
            }
            
        }
    }

    public void GetAllFriends()
    {
        for (int i = 0; i < db.allTrueCharacters.Length; i++)
        {
            if (db.allTrueCharacters[i].alreadyKnown)
            {
                Character character = db.allTrueCharacters[i];
                character.friendshipLevel = 5;
                CharacterManager.instance.charactersLists.CharactersInDorm.Add(character);
                CharacterManager.instance.charactersLists.NbOfTrueCharacter++;
            }

        }
        
    }
}

[System.Serializable]
public class DataBaseCharacter
{
    public Character[] allTrueCharacters;
}

