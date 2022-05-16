using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager instance;

    //Various managers to call daily
    [SerializeField]
    private PlayerManager pManager;
    [SerializeField]
    private MapManagerScript mManager;
    [SerializeField]
    private UIDisplay uiManager;
    [SerializeField]
    private BoxingManagerScript bScript;

    /* UNCOMMENT WHEN SOUND IN THIS BUILD
    [SerializeField]
    private SoundManager soundManager;
    */

    //0 = spring 1 = Summer 2 = Autumn 3 = Winter
    int season = 0;

    bool isDay = true;

    //timer for forced progression
    float Timer = 1.0f;
    bool paused = false;

    int daycount = 1;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Debug.Log(daycount);
    }

    // Update is called once per frame
    void Update()
    {
        //debugging for days
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeTime();
        }

        //Day timer
        if(!paused)
            Timer -= Time.deltaTime;

        if (Timer <= 0)
            ChangeTime();
    }

    //DayToNight
    public void ChangeTime()
    {
        isDay = !isDay;
        if (isDay)
            ChangeDay();
        Timer = 5.0f;
        mManager.ChangeTime(isDay);
        Event();
    }

    //NEEDS CHECKING FOR ACCURACY WHEN PROPER SPRITES AVAILABLE
    public void ChangeDay()
    {
        //cue sounds here
        //
        //

        //Change season every 7 days
        if(daycount %7 == 0)
        {
            ChangeSeason();
        }
        //UI change day
        uiManager.DayFunction();
        daycount++;
    }

    //SeasonChange
    public void ChangeSeason()
    {
        if (season < 2)
            season++;
        else
            season = 0;
        mManager.ChangeSeason(season);
    }

    //for events on day/season change
    void Event()
    {
        if (daycount % 3 == 0)
            mManager.boxingOpen = true;
        else
            mManager.boxingOpen = false;
    }
}
