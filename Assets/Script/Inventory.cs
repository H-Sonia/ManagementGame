using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        ClearInventory();
        ClearAllCharactersResources();
        FillCharacters();
        ui.UpdateMainUi();
    }

    public void ClearInventory()
    {
        content.Clear();
    }

    public void ClearAllCharactersResources()
    {
        for(int i=0; i< CharacterDatabase.instance.allCharacters.Length; i++)
        {
            CharacterDatabase.instance.allCharacters[i].resourcesAttribuated.Clear();
            CharacterDatabase.instance.allCharacters[i].nbOfDaysWithoutFood = 0;
        }
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
        for (int i=0; i < 18; i++)
        {
            emptyList.Add(empty);
        }
        Characters = emptyList.ToArray();


        System.Random random = new System.Random();
        for (int i = 0; i < 18; i++)
        {
            int index = random.Next(Newcomers.Count);
            Characters[i] = Newcomers[index];
            Newcomers.Remove(Newcomers[index]);
        }
    }


    void CharacterDisappears(int characterIndex)
    {
        if (Characters[characterIndex].id != 0)
        {
           DeceasedCharacters.Add(Characters[characterIndex]);
           Characters[characterIndex] = empty;
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
        
        // System.Random random = new System.Random();
        // int probability = random.Next(100);
        // if(probability > 70)
        // {
        //     SomeoneDisappears();
        // }
        // System.Random random2 = new System.Random();
        // int probability2 = random2.Next(100);
        // if (probability2 > 70)
        // {
        //     SomeoneAppears();
        // }

        /*for(int i=0; i < Characters.Length; i++)
        {
            if(Characters[i].id != 0)
            {
                if(Characters[i].resourcesAttribuated.Any())
                {
                    Characters[i].nbOfDaysWithoutFood = 0;
                    Characters[i].nbOfUseOfLastItem++;
                    if(Characters[i].resourcesAttribuated[0].amount < Characters[i].nbOfUseOfLastItem)
                    {
                        Characters[i].resourcesAttribuated.Remove(Characters[i].resourcesAttribuated[0]);
                        Characters[i].nbOfUseOfLastItem = 0;
                    } 
                }
                else
                {
                    Characters[i].nbOfDaysWithoutFood++;
                    MayDisappear(i);
                }
            }
        }*/
        ui.UpdateMainUi();
    }
    void MayDisappear(int index)
    {
        int chanceOfDisappearing;
        System.Random random = new System.Random();
        int probability = random.Next(101);
        switch(Characters[index].nbOfDaysWithoutFood)
        {
            case 1:
            chanceOfDisappearing = 50;
            break;
            case 2:
            chanceOfDisappearing = 60;
            break;
            case 3:
            chanceOfDisappearing = 70; 
            break;
            case 4:
            chanceOfDisappearing = 80;
            break;
            case 5:
            chanceOfDisappearing = 90;
            break;
            default:
            chanceOfDisappearing = 100;
            break;
        }
        if(probability < chanceOfDisappearing)
        {
            CharacterDisappears(index);
        }

    }

}
