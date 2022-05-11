using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemData", menuName = "Resources")]
public class ItemData : ScriptableObject
{
    public int id;
    public string type;
    public string itemName;
    public Sprite itemImage;
    public int rarity;
    public int daysBeforeExpiration;
    public int nutritiveValue;
    public int heat;
    public int health;
    public int efficiencyAtWork;

 
}
