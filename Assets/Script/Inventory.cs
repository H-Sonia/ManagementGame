using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public List<FoodData> content = new List<FoodData>();
    public int currentResource = 0;
    public CharactersData empty;
    public CharactersData[] Characters;
    public List<CharactersData> DeceasedCharacters = new List<CharactersData>();
    public List<CharactersData> Newcomers = new List<CharactersData>(); 
    public int currentCharacter;
    public UIDisplay ui;
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
    public void Start()
    {
        FillCharacters();
        ui.UpdateMainUi();
    }
    public void ClearInventory()
    {
        content.Clear();
    }



    public void FillNewcomers()
    {
        for (int i = 1; i < CharacterDatabase.instance.allCharacters.Length; i++)
        {
            Newcomers.Add(CharacterDatabase.instance.allCharacters[i]);
        }
    }

    public void FillCharacters()
    {
        FillNewcomers();
        List<CharactersData> emptyList = new List<CharactersData>();
        for (int i=0; i < 12; i++)
        {
            emptyList.Add(empty);
        }
        Characters = emptyList.ToArray();


        System.Random random = new System.Random();
        int charactersNb = random.Next(12);
        for (int i = 0; i < charactersNb; i++)
        {
            int index = random.Next(Newcomers.Count);
            Characters[i] = Newcomers[index];
            Newcomers.Remove(Newcomers[index]);
        }
    }


    void SomeoneDisappears()
    {
        System.Random random = new System.Random();
        int index = random.Next(12);
        if (Characters[index].id != 0)
        {
           DeceasedCharacters.Add(Characters[index]);
           Characters[index] = empty;
        }
    }

    int IsEmptySpot()
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            if (Characters[i].id == 0)
            {
                return i;
            }
        }
        return -1;
    }

    void SomeoneAppears()
    {
        int index = IsEmptySpot();

        if (Newcomers.Count <= 0)
        {
            return;
        }

        if (index != -1)
        {
            Characters[index] = Inventory.instance.Newcomers[0];
            Newcomers.Remove(Inventory.instance.Newcomers[0]);
        }
    }

    public void UpdateCharactersLists()
    {
        System.Random random = new System.Random();
        int probability = random.Next(100);
        if(probability > 70)
        {
            SomeoneDisappears();
        }
        System.Random random2 = new System.Random();
        int probability2 = random2.Next(100);
        if (probability2 > 70)
        {
            SomeoneAppears();
        }
        ui.UpdateMainUi();
    }

}
