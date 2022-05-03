using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public CharactersData[] allCharacters;

    public static CharacterDatabase instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of ResourcesDataBase in this scene");
            return;
        }
        instance = this;
    }
}
