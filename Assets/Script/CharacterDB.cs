using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDB : MonoBehaviour
{
    public DataBaseCharacter db = new DataBaseCharacter();

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
    }

    public void LoadJsonFile()
    {
        string filePath = "C:/Users/Sonia Hammami/Documents/GitHub/Playing-in-the-Remnant/Assets/Json/TrueCharacterData.json";
        string initialDB = System.IO.File.ReadAllText(filePath);
        db = JsonUtility.FromJson<DataBaseCharacter>(initialDB);
        Debug.Log("Data loaded");
    }
}

[System.Serializable]
public class DataBaseCharacter
{
    public Character[] allTrueCharacters;
    public string[] FirstnameForPlaceHolder;
    public string[] LastNameForPlaceHolder;
}

