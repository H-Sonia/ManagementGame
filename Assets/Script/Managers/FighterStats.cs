using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterStats
{
    public int strength; // Stronger attacks make fighter more powerful
    public int dexterity; // more attacks add slight advantage to fights
    public int hunger; //0-10 values, impacts performance

    public FighterStats(int str, int dex, int hun)
    {
        strength = str; dexterity = dex; hunger = hun;
    }
}
