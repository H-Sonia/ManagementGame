using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using TMPro;

public class Inventory : MonoBehaviour
{
    public List<ItemData> content = new List<ItemData>();
    public int currentResource = 0;
    public CharactersData empty;
    public CharactersData[] Characters;
    public List<CharactersData> DeceasedCharacters = new List<CharactersData>();
    public List<CharactersData> Newcomers = new List<CharactersData>(); 
    public int currentCharacter;
    bool isWinter;
    public UIDisplay ui;
    public TMP_Text infos;
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
        ui.UpdateMainUi("",true);
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
            CharacterDatabase.instance.allCharacters[i].daysBeforeExpiration.Clear();
            CharacterDatabase.instance.allCharacters[i].cold = 0;
            CharacterDatabase.instance.allCharacters[i].hunger = 0;
            CharacterDatabase.instance.allCharacters[i].isSick = false;
            CharacterDatabase.instance.allCharacters[i].health = 100;
            CharacterDatabase.instance.allCharacters[i].efficiencyAtWork = 50;
            CharacterDatabase.instance.allCharacters[i].friendshipLevel = 0;
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

    public void UpdateCharactersResources()
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            for (int j = 0; j < Characters[i].resourcesAttribuated.Count; j++)
            {
                ConsumeItem(i, j);
                Characters[i].daysBeforeExpiration[j]--;
                if(Characters[i].daysBeforeExpiration[j] <= 0)
                {
                    Characters[i].resourcesAttribuated.RemoveAt(j);
                    Characters[i].daysBeforeExpiration.RemoveAt(j);

                }
            }       
        }
    }

    public void ConsumeItem(int indexCharacter, int indexItem)
    {
        switch(Characters[indexCharacter].resourcesAttribuated[indexItem].type)
        {
            case "food":
                Characters[indexCharacter].hunger -= Characters[indexCharacter].resourcesAttribuated[indexItem].nutritiveValue;
                break;
            case "clothe":
                Characters[indexCharacter].cold -= Characters[indexCharacter].resourcesAttribuated[indexItem].heat;
                if (Characters[indexCharacter].efficiencyAtWork < 100)
                {
                    Characters[indexCharacter].efficiencyAtWork += Characters[indexCharacter].resourcesAttribuated[indexItem].efficiencyAtWork;
                }
                break;
            case "medicine":
                Characters[indexCharacter].isSick = false;
                if (Characters[indexCharacter].health < 100)
                {
                    Characters[indexCharacter].health += Characters[indexCharacter].resourcesAttribuated[indexItem].health;
                }
                break;
            default:
                Debug.LogWarning("unknown type");
                break;
        }
    }

    void UpdateCharactersState(ref string sickPeople)
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            if(Characters[i].cold < 100)
            {
                Characters[i].cold += 10;
            }
            if(Characters[i].hunger < 100)
            {
                Characters[i].hunger += 10;
            }
            if(!Characters[i].isSick)
            {
                Characters[i].isSick = isBecomingSick(i);
                if(Characters[i].isSick && Characters[i].friendshipLevel > 0)
                {
                    sickPeople += Characters[i].firstname + " seems sick today\n";
                }
            }
            else
            {
                Characters[i].health -= 10;
                Characters[i].efficiencyAtWork -= 10;
                if (Characters[i].friendshipLevel > 0)
                {
                    sickPeople += Characters[i].firstname + " still sick today\n";
                }
            }
        }

    }

    bool isBecomingSick(int indexCharacter)
    {
        int probability = (Characters[indexCharacter].cold + Characters[indexCharacter].hunger)/2;
        if(probability < 50)
        {
            return false;
        }

        System.Random random = new System.Random();
        if(random.Next(101) < probability)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public List<KeyValuePair<int, int>> SortPeopleByMostLikelyToDisapear()
    {
        Dictionary<int, int> chanceOfDisapearing = new Dictionary<int, int>();
        for (int i = 0; i < Characters.Length; i++)
        {
            chanceOfDisapearing.Add(i, (Characters[i].health + Characters[i].efficiencyAtWork));
        }
        List<KeyValuePair<int,int>> sortedList = chanceOfDisapearing.ToList();
        sortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        return sortedList;
    }

    public List<int> CreateDrawList(List<KeyValuePair<int, int>> sortedList)
    {
        int nbOfOccurences = 1;
        List<int> drawList = new List<int>();
        foreach (var value in sortedList)
        {
            for (int i = 0; i < nbOfOccurences; i++)
            {
                drawList.Add(value.Key);
            }
            nbOfOccurences += 2;
        }
        return drawList;
    }

    public List<int> ShuffleList(List<int> listToShuffle)
    {
        System.Random rd = new System.Random();
        var shuffledList = listToShuffle.OrderBy(a => rd.Next()).ToList();
        return shuffledList;
    }

    public void ChangeCharacters(int howManyChanges, List<int> shuffledList, ref string friendsWhoDisappeared, ref int disappearingCounter)
    {
        System.Random rd = new System.Random();
        for (int i = 0; i < howManyChanges; i++)
        {
            int index = shuffledList[rd.Next(shuffledList.Count())];
            CharacterDisappears(index, ref friendsWhoDisappeared, ref disappearingCounter);
            SomeoneAppears(index);
            shuffledList.RemoveAll(item => item == index);

        }
    }
    void UpdateCharactersPresent(ref string nbOfPeopleDisappearing, ref string friendsWhoDisappeared)
    {
        List<KeyValuePair<int, int>> sortedList = SortPeopleByMostLikelyToDisapear();

        List<int> drawList = CreateDrawList(sortedList);

        List<int> shuffledList = ShuffleList(drawList);

        int howManyDisappear = HowManyDisappear();

        int disappearingCounter = 0; 

        ChangeCharacters(howManyDisappear, shuffledList, ref friendsWhoDisappeared, ref disappearingCounter);

        switch (disappearingCounter)
        {
            case 0:
                nbOfPeopleDisappearing = "Nobody disappeared today\n";
                break;
            case 1:
                nbOfPeopleDisappearing = "Someone disappeared today\n";
                break;
            default:
                nbOfPeopleDisappearing = howManyDisappear + " people disappeared today\n";
                break;
        }

    }
    public void UpdateCharactersLists()
    {
        string sickPeolple = "";
        string nbOfPeopleDisappearing = "";
        string friendsWhoDisappeared = "";
        UpdateCharactersResources();
        UpdateCharactersState(ref sickPeolple);
        UpdateCharactersPresent(ref nbOfPeopleDisappearing, ref friendsWhoDisappeared);
        infos.text = nbOfPeopleDisappearing + friendsWhoDisappeared + sickPeolple;
        ui.UpdateMainUi("",true);
    }

    int HowManyDisappear()
    {
        System.Random random = new System.Random();
        if (isWinter)
        {
            return random.Next(5);
        }
        else
        {
            return random.Next(3);
        }
    }


    
    bool MayDisappear(int index)
    {
        System.Random random = new System.Random();
        int luck = random.Next(101);
        int probabilityOfDisapearing = (luck + Characters[index].health + Characters[index].efficiencyAtWork) / 3;
        int x = random.Next(101);
        Debug.LogWarning("x = " + x);
        Debug.LogWarning(probabilityOfDisapearing);
        if(x > probabilityOfDisapearing)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    void SomeoneAppears(int index)
    {
        if (Newcomers.Count > 0)
        {
            Characters[index] = Newcomers[0];
            Newcomers.Remove(Newcomers[0]);
        }
    }
    void CharacterDisappears(int characterIndex, ref string friendsWhoDisappeared, ref int disappearingCounter)
    {
        Debug.LogWarning(characterIndex);
        if (Characters[characterIndex].id != 0)
        {
            disappearingCounter += 1;
            DeceasedCharacters.Add(Characters[characterIndex]);
            if(Characters[characterIndex].friendshipLevel > 0)
            {
                Debug.LogWarning(Characters[characterIndex].firstname);
                friendsWhoDisappeared += Characters[characterIndex].firstname + " disappeared.\n";
            }
            Characters[characterIndex] = empty;
        }
    }

}
