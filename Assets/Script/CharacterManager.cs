using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using TMPro;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public CharacterDataLists charactersLists = new CharacterDataLists();
    public Sprite netralPicture;

    public TMP_Text infos;
    public TMP_Text infos2;

    public UIDisplay ui;
    public string ifSomeoneDie;

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
        string filePath = Application.persistentDataPath + "/CharactersData.json";
        if(!System.IO.File.Exists(filePath))
        {
            InitializeCharactersData();
        }
        else
        {
            LoadFromJson();
        }

        ui.UpdateMainUi("", false);
    }

    public void UpdateCharacterLists()
    {
        string filePath = Application.persistentDataPath + "/CharactersData.json";
        if (System.IO.File.Exists(filePath))
        {
            string sickPeolple = "";
            string nbOfPeopleDisappearing = "";
            string friendsWhoDisappeared = "";
            UpdateCharactersResources();
            UpdateCharactersState(ref sickPeolple);
            UpdateCharactersPresent(ref nbOfPeopleDisappearing, ref friendsWhoDisappeared);
            if(MainManager.instance.isDay)
                infos.text = nbOfPeopleDisappearing + friendsWhoDisappeared + sickPeolple;
            else if (!MainManager.instance.isDay)
                infos2.text = nbOfPeopleDisappearing + friendsWhoDisappeared + sickPeolple;
            ui.UpdateMainUi("", true);
        }
    }

    public void InitializeCharactersData()
    {
        FillWithKey();
        FillNameNotUsed();
        FillWithPlaceHolders();
        charactersLists.CharactersInDorm =  ShuffleCharacterList(charactersLists.CharactersInDorm);
    }

    void FillWithKey()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < charactersLists.TrueNewcomers.Count; i++)
        {      
            Character character = charactersLists.TrueNewcomers[i];
            if(character.firstname == "" )
            {
                if (character.id == 3)
                    EventManager.instance.key3 = character;
                else if(character.id == 5)
                    EventManager.instance.key5 = character;

                if (charactersLists.nameNotUsed.Count <= 0)
                {
                    charactersLists.nameNotUsed = charactersLists.nameUsed;
                    charactersLists.nameUsed.Clear();
                }

                //int index = random.Next(charactersLists.nameNotUsed.Count);
                int index = UnityEngine.Random.Range(0, CharacterDB.instance.FirstnameForPlaceHolder.Length);
                character.firstname = CharacterDB.instance.FirstnameForPlaceHolder[index];
                index = UnityEngine.Random.Range(0, CharacterDB.instance.FirstnameForPlaceHolder.Length);
                character.surname = CharacterDB.instance.LastNameForPlaceHolder[index];

                charactersLists.nameUsed.Add(index);
                charactersLists.nameNotUsed.Remove(index);
            }

            charactersLists.CharactersInDorm.Add(character);
        }
    }

    public void FillNameNotUsed()
    {
        for (int i = 0; i < CharacterDB.instance.FirstnameForPlaceHolder.Length; i++)
        {
            charactersLists.nameNotUsed.Add(i);
        }
    }

    public void FillWithPlaceHolders()
    {
        int nbEmptyBeds = charactersLists.DormCapacity - charactersLists.CharactersInDorm.Count;
        for (int i=5; i < nbEmptyBeds; i++)
        {
            Character character = CreatePlaceHolderCharacter();
            charactersLists.CharactersInDorm.Add(character);
        }
    }
    Character CreatePlaceHolderCharacter()
    {
        if (charactersLists.nameNotUsed.Count == 0)
        {
            charactersLists.nameNotUsed = charactersLists.nameUsed;
            charactersLists.nameUsed.Clear();
        }

        System.Random random = new System.Random();
        int index = random.Next(charactersLists.nameNotUsed.Count);

        string firstname = "FirstName";
        string lastname = "LastName";

        if (charactersLists.nameNotUsed.Count > 0)
        {
            firstname = CharacterDB.instance.FirstnameForPlaceHolder[charactersLists.nameNotUsed[index]];
            lastname = CharacterDB.instance.LastNameForPlaceHolder[charactersLists.nameNotUsed[index]];
            charactersLists.nameUsed.Add(charactersLists.nameNotUsed[index]);
            charactersLists.nameNotUsed.Remove(charactersLists.nameNotUsed[index]);
        }
        else
        {
            Debug.Log("USING TEMP FILL!");
        }


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

        return character;
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
            List<int> newDaysBeforeExpiration = new List<int>();
            List<ItemData> newResourcesAttribuated = new List<ItemData>();
            if(charactersLists.CharactersInDorm[i].resourcesAttribuated.Count > 0)
            {
                for (int j = 0; j < charactersLists.CharactersInDorm[i].resourcesAttribuated.Count; j++)
                {
                    ConsumeItem(i, j);
                    charactersLists.CharactersInDorm[i].daysBeforeExpiration[j]--;
                    if (charactersLists.CharactersInDorm[i].daysBeforeExpiration[j] > 0)
                    {
                        newDaysBeforeExpiration.Add(charactersLists.CharactersInDorm[i].daysBeforeExpiration[j]);
                        newResourcesAttribuated.Add(charactersLists.CharactersInDorm[i].resourcesAttribuated[j]);
                    }
                }
                charactersLists.CharactersInDorm[i].daysBeforeExpiration = newDaysBeforeExpiration;
                charactersLists.CharactersInDorm[i].resourcesAttribuated = newResourcesAttribuated;
            }
            
        }
    }


    public void ConsumeItem(int indexCharacter, int indexItem)
    {
        if (charactersLists.CharactersInDorm[indexCharacter].resourcesAttribuated.Count > 0)
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
    }


    [SerializeField]
    GameObject panel;

    void UpdateCharactersState(ref string sickPeople)
    {
        for (int i = 0; i < charactersLists.CharactersInDorm.Count; i++)
        {
            charactersLists.CharactersInDorm[i].fedToday = false;
            if (charactersLists.CharactersInDorm[i].cold < 100)
            {
                charactersLists.CharactersInDorm[i].cold += 10;
            }
            if(charactersLists.CharactersInDorm[i].hunger < 100)
            {
                charactersLists.CharactersInDorm[i].hunger += 10;
            }
            if(!charactersLists.CharactersInDorm[i].isSick)
            {
                charactersLists.CharactersInDorm[i].isSick = isBecomingSick(i);
                panel.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = charactersLists.CharactersInDorm[i].firstname + " seems sick today\n";
                panel.SetActive(true);
                Debug.Log(charactersLists.CharactersInDorm[i] + "is sick!");
                if(charactersLists.CharactersInDorm[i].isSick && charactersLists.CharactersInDorm[i].friendshipLevel > 0)
                {
                    sickPeople += charactersLists.CharactersInDorm[i].firstname + " seems sick today\n";
                }
            }
            else
            {
                charactersLists.CharactersInDorm[i].health -= 10;
                charactersLists.CharactersInDorm[i].efficiencyAtWork -= 10;
                if (charactersLists.CharactersInDorm[i].friendshipLevel > 0)
                {
                    sickPeople += charactersLists.CharactersInDorm[i].firstname + " still sick today\n";
                }
            }
        }

    }

    bool isBecomingSick(int indexCharacter)
    {
        int probability = (charactersLists.CharactersInDorm[indexCharacter].cold + charactersLists.CharactersInDorm[indexCharacter].hunger)/2;
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
                nbOfPeopleDisappearing = "The morning comes and we have to get up for another day in this hell, but for how much time left ?";
                break;

            default:
                //night
                int rand = UnityEngine.Random.Range(0, 100);

                if (!MainManager.instance.isDay)
                {
                    //Morning
                    rand = UnityEngine.Random.Range(0, 100);
                    if (rand >= 90)
                        ifSomeoneDie = "Perhaps it was illness; perhaps hunger. It does not matter. \n" +
                            "We all know the truth, as the bodies are carried outside. We watch in silence, wondering who will be next. \n" +
                            "Maybe we don’t even wonder. Maybe we only remember wondering.\n" +
                            "Maybe we wonder when we remember.";
                    else if (rand >= 80)
                        ifSomeoneDie = "We watch in silence, wondering who will be next. Maybe we don’t even wonder. Maybe we only remember wondering. Maybe we wonder when we remember. ";
                    else if (rand >= 70)
                        ifSomeoneDie = "We watch in silence, wondering who will be next.";
                    else if (rand >= 55)
                        ifSomeoneDie = "Perhaps it was illness; perhaps hunger. It does not matter. \n" +
                            "We all know the truth, as the bodies are carried outside. The camp took them.";
                    else if (rand < 55)
                        ifSomeoneDie = "";
                    nbOfPeopleDisappearing = "The morning comes. Some people do not survive the night. \n" + ifSomeoneDie + "\n";
                }
                else if (MainManager.instance.isDay)
                { 
                //NIGHT
                if (rand >= 75)
                    ifSomeoneDie = "Not everyone makes it back to the camp. I am only grateful that I did not have to witness the Nazis putting a bullet through their head once their body was too weak to continue.";
                else if (rand >= 40)
                    ifSomeoneDie = "Some people do not return that evening.\n" +
                        "We do not ask what happened – there is no need to ask those who were there to relive it.";
                else
                    ifSomeoneDie = "Fewer people return at night.\n" +
                        "We do not ask what happened.\n" +
                        "We don’t need to.";

                nbOfPeopleDisappearing = ifSomeoneDie;

                }

                break;
        }

    }

    public List<KeyValuePair<int, int>> SortPeopleByMostLikelyToDisapear()
    {
        Dictionary<int, int> chanceOfDisapearing = new Dictionary<int, int>();
        for (int i = 0; i < charactersLists.CharactersInDorm.Count; i++)
        {
            if(CanDie(i))
            {
                if(charactersLists.CharactersInDorm[i].id != -1)
                {
                    chanceOfDisapearing.Add(i, (charactersLists.DaysPassedSinceLastTrueCharacter + charactersLists.CharactersInDorm[i].health + charactersLists.CharactersInDorm[i].efficiencyAtWork) / 2);
                }
                else
                {
                    chanceOfDisapearing.Add(i, (charactersLists.CharactersInDorm[i].health + charactersLists.CharactersInDorm[i].efficiencyAtWork) / 2);
                }
                
            }
            
        }
        List<KeyValuePair<int,int>> sortedList = chanceOfDisapearing.ToList();
        sortedList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        return sortedList;
    }

    bool CanDie(int characterIndex)
    {
        if(!charactersLists.CharactersInDorm[characterIndex].surviveUntilTheEnd)
        {
            if(charactersLists.CharactersInDorm[characterIndex].id != -1)
            {
                if(charactersLists.CharactersInDorm[characterIndex].friendshipLevel > 1)
                {
                    return true;
                }
                return false;
            }
            return true;
        }
        return false;
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

    int HowManyDisappear()
    {
        System.Random random = new System.Random();
        int nbDeath; 
        switch (MainManager.instance.season)
        {
            case 0:
                nbDeath = random.Next(3);
                break;
            case 1:
                nbDeath = random.Next(3);
                break;
            case 2:
                nbDeath = random.Next(7);
                break;
            case 3:
                nbDeath = random.Next(9);
                break;
            default:
                nbDeath = 0;
                Debug.LogWarning("Season index out of range");
                break;
        }

        return nbDeath;
    }

    public void ChangeCharacters(int howManyChanges, List<int> shuffledList, ref string friendsWhoDisappeared, ref int disappearingCounter)
    {
        System.Random rd = new System.Random();
        for (int i = 0; i < howManyChanges; i++)
        {
            if(shuffledList.Count > 0)
            {
                int rdNumber = rd.Next(shuffledList.Count());
                int index = shuffledList[rdNumber];
                CharacterDisappears(index, ref friendsWhoDisappeared, ref disappearingCounter);
                SomeoneAppears(index);
                shuffledList.RemoveAll(item => item == index);
            }
            

        }
    }

    void SomeoneAppears(int index)
    {
        if(charactersLists.DaysPassedSinceLastTrueCharacter > 5 && charactersLists.NbOfTrueCharacter < charactersLists.NbMaxTrueCharacter)
        {
            if (charactersLists.TrueNewcomers.Count > 0)
            {
                charactersLists.CharactersInDorm[index] = charactersLists.TrueNewcomers[0];
                charactersLists.TrueNewcomers.Remove(charactersLists.TrueNewcomers[0]);
                charactersLists.NbOfTrueCharacter++;
                charactersLists.DaysPassedSinceLastTrueCharacter = 0;
            }
        }
        else
        {
            Character character = CreatePlaceHolderCharacter();
            charactersLists.CharactersInDorm[index] = character;
            charactersLists.DaysPassedSinceLastTrueCharacter++;
        }
        
       
        
    }

    public void RemoveCharacter(Character c)
    {
        charactersLists.CharactersInDorm.Remove(c);
    }

    void CharacterDisappears(int characterIndex, ref string friendsWhoDisappeared, ref int disappearingCounter)
    {
        if (charactersLists.CharactersInDorm[characterIndex].id != 0)
        {
            disappearingCounter += 1;
            charactersLists.DeadCharacters.Add(charactersLists.CharactersInDorm[characterIndex]);
            if(charactersLists.CharactersInDorm[characterIndex].friendshipLevel > 0)
            {
                friendsWhoDisappeared += charactersLists.CharactersInDorm[characterIndex].firstname + " disappeared.\n";
            }
            if(charactersLists.CharactersInDorm[characterIndex].id != -1)
            {
                charactersLists.NbOfTrueCharacter--;
            }
            Character empty = new Character();
            empty.id = 0;
            empty.resourcesAttribuated = new List<ItemData>();
            empty.daysBeforeExpiration = new List<int>();
            charactersLists.CharactersInDorm[characterIndex] = empty;
        }
    }

    public void SaveToJson()
    {
        string charactersListsData = JsonUtility.ToJson(charactersLists);
        string filePath = Application.persistentDataPath + "/CharactersData.json";

        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, charactersListsData);

        MainManager.instance.SaveDetails();
        Debug.Log("Data saved");
    }

    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/CharactersData.json";
        string charactersListsData = System.IO.File.ReadAllText(filePath);
        charactersLists = JsonUtility.FromJson<CharacterDataLists>(charactersListsData);
        Debug.Log("Data loaded");
        ui.UpdateMainUi("", false);
        MainManager.instance.LoadDetails();
    }
}

[System.Serializable]
public class CharacterDataLists
{
    public int DaysPassedSinceLastTrueCharacter;
    public int NbOfTrueCharacter;
    public int NbMaxTrueCharacter = 3;
    public int DormCapacity = 18;
    public int currentCharacter; 
    public List<Character> CharactersInDorm = new List<Character>();
    public List<Character> TrueNewcomers = new List<Character>();
    public List<Character> DeadCharacters = new List<Character>();
    public List<int> nameNotUsed = new List<int>();
    public List<int> nameUsed = new List<int>();
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
    public bool fedToday = false;
    public int friendshipLevel;
    public List<ItemData> resourcesAttribuated;
    public List<int> daysBeforeExpiration;
    public int hunger;
    public int cold;
    public bool isSick;
    public int health;
    public int efficiencyAtWork;
    public bool isKey;
    public int keyStage;
}