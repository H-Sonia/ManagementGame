using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class saveDetails
{ 
    public int[] details;
    public string evdeet;
    public saveDetails(int[] array, string ed)
    {
        details = array;
        evdeet = ed;
    }

}

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
    TMP_Text dayCount, dayNight, seasonTxt, yearText;

    [SerializeField]
    private GameObject[] morningCovers;
    [SerializeField]
    private GameObject[] hideOnChange;

    [SerializeField]
    private GameObject panel;

    //0 = Spring 1 = Summer 2 = Autumn 3 = Winter 
    public int season = 1;
    int lastSeason = 0, maxSeason = 5;
    public bool isDay = true;
    public int year = 1943;

    string[] texts = { "Spring", "Summer", "Autumn", "Winter" };


    eventDetails details;
    public string SaveDetails()
    {
        details =  EventManager.instance.saveEvents();
        Debug.Log(details);
        string saveSeason = JsonUtility.ToJson(season);
        string saveLastSeason = JsonUtility.ToJson(lastSeason);
        string saveMaxSeason = JsonUtility.ToJson(maxSeason);
        string saveYear = JsonUtility.ToJson(year);

        string EventDetails = JsonUtility.ToJson(details);

        int[] savedeets = new int[]{season, lastSeason, maxSeason, year};
        saveDetails s = new saveDetails(savedeets, EventDetails);

        string save = JsonUtility.ToJson(s);

        //string save = (saveSeason + "\n" + saveLastSeason + "\n" + saveMaxSeason + "\n" + saveYear);
        //string save = (season + "\n" + lastSeason + "\n" + maxSeason + "\n" + year);

        string filePath = Application.persistentDataPath + "/DataSave.json";

        System.IO.File.WriteAllText(filePath, save);
        return null;
    }
    public void LoadDetails()
    {
        string filePath = Application.persistentDataPath + "/DataSave.json";
        if (System.IO.File.Exists(filePath))
        {
            string loaded = System.IO.File.ReadAllText(filePath);
            saveDetails output = JsonUtility.FromJson<saveDetails>(loaded);

            season = output.details[0];
            lastSeason = output.details[1];
            maxSeason = output.details[2];
            year = output.details[3];

            //always null
            Debug.Log(output.evdeet);

            //save events
            details = JsonUtility.FromJson<eventDetails>(output.evdeet);

            EventManager.instance.loadEvents(details);

            seasonTxt.text = texts[season];
            yearText.text = year.ToString();
            MapManagerScript.instance.ChangeSeason(season);
        }
    }

    //timer for forced progression in seconds
    float Timer = 10.0f, morningTimer = 20.0f;
    bool paused = false;

    int daycount = 1;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;

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
        season = 1;
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

    public GameObject morninginfos;
    //DayToNight
    public void ChangeTime(bool forced = false)
    {

        CharacterManager.instance.UpdateCharacterLists();
        if (!forced)
        {
            //Check ONCE for inventory if changing day
            if (!isDay)
            {
                if (!checkedInv && InventoryCheck())
                {
                    checkedInv = true;
                    return;
                }
            }
        }

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
        
        StartCoroutine("TimeChangeFunction");
        Timer = 60.0f;
        Event();
        mManager.ChangeTime();
    }

    bool checkedInv = false;

    //true if empty
    public bool InventoryCheck()
    {
        if (Inventory.instance.content.Count > 0)
        {
            panel.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "I still have resources, I should hand them out before they are confiscated.";
            panel.SetActive(true);
            return true;
        }
        else
            return false;
    }

    //NEEDS CHECKING FOR ACCURACY WHEN PROPER SPRITES AVAILABLE
    public void ChangeDay()
    {
        Event();
        foreach (Character c in CharacterManager.instance.charactersLists.CharactersInDorm)
            c.fedToday = false;

        Inventory.instance.DayFunction();
        //cue sounds here
        //

        lastSeason++;
        if(lastSeason >= maxSeason)
        {
            seasonChange = true;
            ChangeSeason();
        }

        //UI change day
        kManager.DayFunction();
        mManager.ChangeRoomState(0);
        daycount++;
        CharacterManager.instance.SaveToJson();
        paused = true;
        Inventory.instance.ClearInventory();
        checkedInv = false;
    }

    //SeasonChange
    public void ChangeSeason()
    {
        seasonChange = true;

        lastSeason = 0;
        if (season < 4)
            season++;
        else
        {
            season = 0;
            year += 1;
            yearText.text = year.ToString();
            if (year == 1945)
            {
                Debug.Log("END OF GAME");
                UnityEngine.SceneManagement.SceneManager.LoadScene(3);
            }
        }

        if (season < 2)
            maxSeason = 5;
        else
            maxSeason = 7;

        mManager.ChangeSeason(season);
    }

    //for events on day/season change
    void Event()
    {
        EventManager.instance.UpdateDay();

        if (daycount % 4 == 0)
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

    string[] seasons = {
        "Autumn arrives. \n" +
            " The days get shorter, the air turns bitterly cold.\n" +
            " It’s hard to track the passing of time in the camp; every day feels like the last, months and seasons falling into one another.  ",

        "Winter is here.\n" +
            " Everything is covered with snow.\n" +
            " Back home, I would have thought it beautiful, though I had never seen snow in Greece.\n" +
            " Here at Auschwitz, beauty doesn’t cross my mind. ",

        "The weather is getting warmer, leaves beginning to sprout in the barren trees.\n" +
            " It’s Spring. ",

        "A year has passed since I arrived at the camps. \n" +
            " I’ve seen so many people die – worked to death, starved, or simply taken away and murdered.\n" +
            " How much longer will I last? How much longer will any of us last?",

        "Autumn is here again. \n" +
            " I can feel the change in the air, the briskness, the bite. \n" +
            " It is the season of wilting, of dwindling harvests. The season of death. ",

        "The winter is upon us once more.\n" +
            " I am cold. We are all cold.\n" +
            " This is a cold I had never imagined – it freezes your skin, your bones and marrow, chills your brain deep down to the neurons. \n" +
            "I will not last much longer. "
    };
    int seasonText = 0;
    bool seasonChange = false;

    IEnumerator TimeChangeFunction()
    {
        morninginfos.SetActive(false);
        float val = 5f;

        string temp = "";
        if (seasonChange)
        { 
            seasonChange = false;
            temp = seasons[seasonText];
            seasonText++;
            val = 10;
        }
        else
        {
            if (isDay)
                temp = "The dawn is upon us. The band plays as we are shoved outside. My comrades are marched to their graves. ";
            if (!isDay)
            {
                Debug.Log("nightime and : " + EventManager.instance.firstNight);
                if (EventManager.instance.firstNight)
                {
                    temp = "Night comes. \n" +
                        "After everyone has returned from their posts, we trickle into our blocks, a sea of grey.\n" +
                        "Exhausted, depleted, we drag our feet in our clogs. Some of us do not even have these cheap wooden shoes and must make do with rags haphazardly tied around our feet, or else with nothing at all. ";
                    EventManager.instance.firstNight = false;
                    val = 10;
                }
                else
                    temp = "Another night comes. We return to our blocks, exhausted. ";
            }
        }

        StartCoroutine(MorningFunction());
        TimeCover.gameObject.SetActive(true);
        TimeCover.GetChild(0).GetComponent<Image>().color = Color.black;
        TimeCover.GetChild(0).GetComponent<Image>().CrossFadeAlpha(0, val, false);
        TimeCover.GetChild(1).GetComponent<TMP_Text>().CrossFadeAlpha(0, val, false);
        TimeCover.GetChild(2).GetChild(0).GetComponent<TMP_Text>().CrossFadeAlpha(0, val, false);
        TimeCover.GetChild(2).GetChild(1).GetComponent<TMP_Text>().CrossFadeAlpha(0, val, false);


        TimeCover.GetChild(1).GetComponent<TMP_Text>().text = temp;
        yield return new WaitForSeconds(val);
        TimeCover.gameObject.SetActive(false);

        if(isDay && morninginfos.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text != "")
            morninginfos.SetActive(true);
    }
}
