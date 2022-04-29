using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "FoodData", menuName = "Resources/Food Data")]
public class FoodData : ScriptableObject
{
    public int id;
    public string foodName;
    public Sprite foodImage;
 
}
