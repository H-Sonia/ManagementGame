using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CharactersData", menuName = "Character")]
public class CharactersData : ScriptableObject
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
