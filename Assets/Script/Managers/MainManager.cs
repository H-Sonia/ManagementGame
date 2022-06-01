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
    private BoxingManagerScript bManager;
    [SerializeField]
    private KitchenScript kManager;

    /* UNCOMMENT WHEN SOUND IN THIS BUILD
    [SerializeField]
    private SoundManager soundManager;
    */

    //0 = spring 1 = Summer 2 = Autumn 3 = Winter
    public int season = 0;

    bool isDay = true;

    //timer for forced progression in seconds
    float Timer = 10.0f;
    bool paused = false;

    int daycount = 1;

    // Start is called before the first frame update
    void Start()
    {

        if (instance != null)
        {
            if (instance != null)
            {
                Debug.LogWarning("There is more than one MainManager instance in this scene");
                return;
            }
        }
        instance = this;

        //in case something goes wrong and values are unassigned
        if (uiManager == null)
            uiManager = GameObject.Find("EventSystem").GetComponent<UIDisplay>();
        if (mManager == null)
            mManager = GameObject.Find("MapManager").GetComponent<MapManagerScript>();
        if (bManager == null)
            bManager = GameObject.Find("Boxing").GetComponent<BoxingManagerScript>();
        if (kManager == null)
            kManager = GameObject.Find("Kitchen Manager").GetComponent<KitchenScript>();

        MainStart();
    }

    void MainStart()
    {
        GameObject.Find("EventSystem").GetComponent<UIDisplay>().Setup();
        GameObject.Find("EventSystem").GetComponent<PanelController>().Setup();
        MainCheck();
    }
    int mainCheck = 0;
    public void MainCheck()
    {
        mainCheck++;
        if(mainCheck > 2)
            mManager.ChangeRoomState(0);
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
        Timer = 60.0f;
        mManager.ChangeTime(isDay);
        Event();
    }

    //NEEDS CHECKING FOR ACCURACY WHEN PROPER SPRITES AVAILABLE
    public void ChangeDay()
    {
        CharacterManager.instance.UpdateCharacterLists();
        Inventory.instance.DayFunction();
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
        kManager.DayFunction();
        daycount++;
        CharacterManager.instance.SaveToJson();
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
        if (daycount % 1 == 0)
            mManager.boxingOpen = true;
        else
            mManager.boxingOpen = false;
    }

    public void Pause(bool ToPause)
    {
        paused = ToPause;
    }
}
