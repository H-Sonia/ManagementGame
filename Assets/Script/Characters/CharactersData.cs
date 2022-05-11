using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharactersData", menuName = "Character")]
public class CharactersData : ScriptableObject
{
    public int id;
    public string firstname;
    public string surname;
    public Sprite picture;
    public string infos;
    public int nbOfUseOfLastItem;
    public List<FoodData> resourcesAttribuated;
    public int nbOfDaysWithoutFood;
    
}
