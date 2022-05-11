using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    public List<ItemData> content = new List<ItemData>();
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
            CharacterDatabase.instance.allCharacters[i].daysBeforeExpiration.Clear();
            CharacterDatabase.instance.allCharacters[i].cold = 0;
            CharacterDatabase.instance.allCharacters[i].hunger = 0;
            CharacterDatabase.instance.allCharacters[i].isSick = false;
            CharacterDatabase.instance.allCharacters[i].health = 100;
            CharacterDatabase.instance.allCharacters[i].efficiencyAtWork = 50;
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

    void UpdateCharactersState()
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
            }
            if(Characters[i].isSick)
            {
                Characters[i].health -= 10;
                Characters[i].efficiencyAtWork -= 10;
            }
        }

    }

    bool isBecomingSick(int indexCharacter)
    {
        int probability = (Characters[indexCharacter].cold + Characters[indexCharacter].hunger)/2;
        System.Random random = new System.Random();
        if(random.Next(101) > probability)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
   

    public void UpdateCharactersLists()
    {
        UpdateCharactersResources();
        UpdateCharactersState();
        for (int i = 0; i < Characters.Length; i++)
        {
            if(MayDisappear(i))
            {
                Debug.LogWarning("Someone as been replaced");
                CharacterDisappears(i);
                SomeoneAppears(i);
            }
        }
       
        ui.UpdateMainUi();
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
    void CharacterDisappears(int characterIndex)
    {
        if (Characters[characterIndex].id != 0)
        {
            DeceasedCharacters.Add(Characters[characterIndex]);
            Characters[characterIndex] = empty;
        }
    }

}
