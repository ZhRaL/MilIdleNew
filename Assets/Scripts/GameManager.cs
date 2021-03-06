using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager INSTANCE;

    private void Awake()
    {
        if (!INSTANCE) INSTANCE = this;
    }

    private bool isInitialized = false;

    #endregion

    public GameObject BaustellenPrefab;

    public const string MARINESAFESTRING = "Marine_Levels";
    public const string ARMYAFESTRING = "Army_Levels";
    public const string AIRFORCEAFESTRING = "Airforce_Levels";
    public const string KITCHENSAFESTRING = "Barracks_Levels";
    public const string BATHSAFESTRING = "Barracks_Levels";
    public const string SLEEPINGSAFESTRING = "Barracks_Levels";
    public const string RECRUITMENTSAFESTRING = "Barracks_Levels";

    #region currencies

    private float _gold, _badges;

    public Text tx_Gold, tx_Badges;

    // Always SaveGame() at Changing Gold or Badge Amount
    public float gold
    {
        get => _gold;
        set
        {
            _gold = value;
            tx_Gold.text = "" + _gold;
            if (isInitialized) SaveGame();
        }
    }

    public float badges
    {
        get => _badges;
        set
        {
            _badges = value;
            tx_Badges.text = "" + _badges;
            if (isInitialized) SaveGame();
        }
    }

    #endregion

    public IController ArmyContr, MarineContr, AirforceContr, KitchenController, BathController, SleepingController, RecruitmentController;

    public GameObject[] Controllers;

    private void Start()
    {
        initializeController();
        LoadGame();
    }

    private void initializeController()
    {
        MarineContr = Controllers[0].GetComponent<MarineController>();
        ArmyContr = Controllers[1].GetComponent<ArmyController>();
        AirforceContr = Controllers[2].GetComponent<AirforceController>();
        KitchenController = Controllers[3].GetComponent<KitchenController>();
        BathController = Controllers[4].GetComponent<BathController>();
        SleepingController = Controllers[5].GetComponent<SleepingController>();
        RecruitmentController = Controllers[6].GetComponent<RecruitmentController>();
    }

    public void SaveGame()
    {
        PlayerPrefs.SetFloat("Gold", gold);
        PlayerPrefs.SetFloat("Badges", badges);
        
        PlayerPrefs.SetString(MARINESAFESTRING, JsonHelper.ToJson(MarineContr.getState()));
        PlayerPrefs.SetString(AIRFORCEAFESTRING, JsonHelper.ToJson(AirforceContr.getState()));
        PlayerPrefs.SetString(ARMYAFESTRING,JsonHelper.ToJson(ArmyContr.getState()));
        PlayerPrefs.SetString(KITCHENSAFESTRING,JsonHelper.ToJson(KitchenController.getState()));
        PlayerPrefs.SetString(BATHSAFESTRING,JsonHelper.ToJson(BathController.getState()));
        PlayerPrefs.SetString(SLEEPINGSAFESTRING,JsonHelper.ToJson(SleepingController.getState()));
    }

    private void LoadGame()
    {
        gold = PlayerPrefs.GetFloat("Gold", 0);
        badges = PlayerPrefs.GetFloat("Badges", 0);
        
        MarineContr.loadState(JsonHelper.FromJson<int>(PlayerPrefs.GetString(MARINESAFESTRING, " {\"Items\":[1,1,0,0,0,0]}")));
        AirforceContr.loadState(JsonHelper.FromJson<int>(PlayerPrefs.GetString(AIRFORCEAFESTRING, " {\"Items\":[1,1,0,0,0,0]}")));
        ArmyContr.loadState(JsonHelper.FromJson<int>(PlayerPrefs.GetString(ARMYAFESTRING," {\"Items\":[1,1,0,0,0,0]}")));
        KitchenController.loadState(JsonHelper.FromJson<int>(PlayerPrefs.GetString(KITCHENSAFESTRING," {\"Items\":[1,1]}")));
        BathController.loadState(JsonHelper.FromJson<int>(PlayerPrefs.GetString(BATHSAFESTRING," {\"Items\":[1,1]}")));
        SleepingController.loadState(JsonHelper.FromJson<int>(PlayerPrefs.GetString(SLEEPINGSAFESTRING," {\"Items\":[1,1]}")));
        isInitialized = true;
    }

    public void ResetPlayerprefs()
    {
        PlayerPrefs.DeleteAll();
    }
}