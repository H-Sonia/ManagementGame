using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    [SerializeField]
    private CharacterManager cManager;
    [SerializeField]
    private GameAudio aManager;
    [SerializeField]
    TMP_Text dayCount, dayNight;

    [SerializeField]
    private GameObject[] morningCovers;
    [SerializeField]
    private GameObject[] hideOnChange;

    //0 = Summer 1 = Autumn 2 = Winter 3 = Spring
    public int season = 0;
    public bool isDay = true;

    //timer for forced progression in seconds
    float Timer = 10.0f, morningTimer = 20.0f;
    bool paused = false;

    int daycount = 1;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;
        //if (instance != null)
        //{
        //        Debug.LogWarning("There is more than one MainManager instance in this scene");
        //        return;
        //}


        //in case something goes wrong and values are unassigned
        if (uiManager == null)
            uiManager = GameObject.Find("EventSystem").GetComponent<UIDisplay>();
        if (mManager == null)
            mManager = GameObject.Find("MapManager").GetComponent<MapManagerScript>();
        if (bManager == null)
            bManager = GameObject.Find("Boxing").GetComponent<BoxingManagerScript>();
        if (kManager == null)
            kManager = GameObject.Find("KitchenManager").GetComponent<KitchenScript>();

        MainStart();
    }

    public static MainManager Instance()
    {
        return instance;
    }

    void MainStart()
    {
        GameObject.Find("EventSystem").GetComponent<UIDisplay>().Setup();
        GameObject.Find("EventSystem").GetComponent<PanelController>().Setup();
        Timer = 10.0f;
        season = 0;
        isDay = true;
        daycount = 1;
        MainCheck();
    }

    int mainCheck = 0;
    public void MainCheck()
    {
        mainCheck++;
        if (mainCheck > 2)
        {
            mManager.ChangeTime(true);
        }
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
       else if (Timer <= 0)
            ChangeTime();
    }

    //DayToNight
    public void ChangeTime()
    {
        if (!isDay)
        {
            if (!checkedInv)
            {
                if (InventoryCheck())
                {
                    Debug.Log("PROBLEM");
                    return;
                }
            }
        }

        StartCoroutine("TimeChangeFunction");
        isDay = !isDay;

        if (isDay)
        {

            dayNight.text = "Day";
            dayCount.text = daycount.ToString();
            ChangeDay();
        }
        if(!isDay)
        {
            dayNight.text = "Night";
        }

        Timer = 60.0f;
        Event();
        mManager.ChangeTime();

    }

    bool checkedInv = false;

    //true if empty
    public bool InventoryCheck()
    { 
        checkedInv = true;
        if (Inventory.instance.content.Count > 0)
            return false;
        else
            Debug.Log("YOU HAVE INVENTORY");
            return true;


    }

    //NEEDS CHECKING FOR ACCURACY WHEN PROPER SPRITES AVAILABLE
    public void ChangeDay()
    {
        CharacterManager.instance.UpdateCharacterLists();
        Inventory.instance.DayFunction();
        //cue sounds here
        //
        //Change season every 7 days
        if (daycount % 2 == 0)
        {
            Debug.Log("NEWSEASON: ");
            ChangeSeason();
        }
        //UI change day
        uiManager.DayFunction();
        kManager.DayFunction();
        mManager.ChangeRoomState(0);
        daycount++;
        CharacterManager.instance.SaveToJson();
        StartCoroutine(MorningFunction());
        paused = true;
        Inventory.instance.ClearInventory();
        if (daycount == 7 * 7)
            Debug.Log("ENDING NOW");
        checkedInv = false;
    }

    //SeasonChange
    public void ChangeSeason()
    {
        if (season < 4)
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

    public void Pause(bool ToPause)
    {
        paused = ToPause;
    }

    //Function to stop buttons being pressed on mornings
    IEnumerator MorningFunction()
    {
        //Play music
        GameAudio.instance.PlayDayMusic();
        //set appropriate objects to be uninteractable
        foreach (GameObject g in morningCovers)
            g.SetActive(true);
        foreach (GameObject g in hideOnChange)
            g.SetActive(false);
        //Wait for 20S CHANGE VALUE IF NEEDED FOR TESTING
        yield return new WaitForSeconds(5.0f);
        //reactivate appropriate objects
        foreach (GameObject g in morningCovers)
            g.SetActive(false);
        foreach (GameObject g in hideOnChange)
            g.SetActive(true);
        //allows music source to change
        GameAudio.instance.playMorning = false;
        GameAudio.instance.ChangeMusic(0);
        //Start countdown
        paused = false;
    }

    [SerializeField]
    Transform TimeCover;
    IEnumerator TimeChangeFunction()
    {
        TimeCover.gameObject.SetActive(true);
        TimeCover.GetChild(0).GetComponent<Image>().color = Color.black;
        TimeCover.GetChild(0).GetComponent<Image>().CrossFadeAlpha(0, 2f, false);
        TimeCover.GetChild(1).GetComponent<TMP_Text>().CrossFadeAlpha(0, 2f, false);

        string temp = "";
        if (!isDay)
            temp = "Day";
        if (isDay)
            temp = "Night";

        TimeCover.GetChild(1).GetComponent<TMP_Text>().text = temp;
        yield return new WaitForSeconds(2f);
        TimeCover.gameObject.SetActive(false);
    }
}
