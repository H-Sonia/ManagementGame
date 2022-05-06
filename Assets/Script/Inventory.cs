using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public List<FoodData> content = new List<FoodData>();
    public int currentResource = 0;
    public CharactersData[] Characters;
    public List<CharactersData> DeceasedCharacters = new List<CharactersData>();
    public List<CharactersData> Newcomers = new List<CharactersData>(); 
    public int currentCharacter;
    public static Inventory instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("There is more than one Inventory instance in this scene");
            return;
        }
        instance = this;
    }

    public void ClearInventory()
    {
        content.Clear();
    }

    // public void FillNewcomers()
    // {
    //     for(int i=0; i < CharacterDatabase.instance.allCharacters.Length;i++)
    //     {
            
    //     }
    // }

    // public void RandomAssignement()
    // {
    //     System.Random random = new System.Random();
    //     int listCount = random.Next(Characters.Length);
    //     for(i=0; i< listCount; i++)
    //     {
    //         System.Random random2 = new System.Random();
    //         int character = random2.Next(CharacterDatabase.instance.allCharacters.Length);
    //         Characters[i] = CharacterDatabase.instance.allCharacters[i];

    //     }

    // }

   
}
