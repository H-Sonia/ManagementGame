using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxingManagerScript : MonoBehaviour
{
    //[SerializeField]
    //FoodData prize;

    [SerializeField]
    TMP_Text wintext;

    [SerializeField]
    GameObject resultsScreen;

    public void Fight()
    {
        EndFight(0);
        //Debug.Log(PlayerManager.instance.check);
        //FighterStats ps = PlayerManager.instance.fighterStats;
        ////Create a random fighter
        //FighterStats fighter = new FighterStats(Random.Range(ps.strength-2,ps.strength +2), Random.Range(ps.strength - 2, ps.dexterity + 1), Random.Range(1,8));

        ////Check scores
        //float score = ps.strength + (ps.strength * ps.dexterity) * 0.1f + 10 - ps.hunger;
        //float enScore = fighter.strength + (fighter.strength * fighter.dexterity) * 0.1f + 10 - fighter.hunger;
        //EndFight(score - enScore);
    }

    void EndFight(float num)
    {
        resultsScreen.SetActive(true);
        //Debug.Log(num);
        num = Random.Range(0,100);
        if (num > 20)
        {
            Debug.Log("WIN");
            wintext.text = "WIN";
            //Inventory.instance.content.Add(prize);
        }
        else
        {
            wintext.text = "LOSE";
            Debug.Log("LOSE");
        }
        MainManager.instance.Pause(true);
        MapManagerScript.instance.boxingOpen = false;
    }
}
