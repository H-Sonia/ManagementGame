using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }
    public FighterStats fighterStats = new FighterStats(3, 3, 5);
    public bool check = true;
    // Start is called before the first frame update
    public void Awake()
    {
        instance = this;
        fighterStats = new FighterStats(3, 3, 5);
    }
}
