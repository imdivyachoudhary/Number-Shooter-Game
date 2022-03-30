using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int dividedPartLevel = 50;
    public int CubeDestroyed = 0;
    int BoosterUnlocker = 20;
    GameObject GunnerGameObject;
    public CubeController cubeController;
    public GunController gunController;
    public BoosterController boosterController;
    public MainMenuController menuController;
    public Text DeductionCubeText;


    void Start()
    {
        dividedPartLevel = saveload.dividedPartLevel;
        print(dividedPartLevel);
        BoosterUnlocker=saveload.boosterTimer;
        GunnerGameObject = GameObject.FindGameObjectWithTag("Gunner");
        InitializeValue();
        UpdateUI();
        BoosterUIUpdate();
        StartCoroutine(WaitAndLateInitialize());
    }

    IEnumerator WaitAndLateInitialize()
    {
        yield return new WaitForSeconds(3);
        dividedPartLevel = saveload.dividedPartLevel;
        BoosterUnlocker = saveload.boosterTimer;
    }

    void InitializeValue()
    {
        CubeDestroyed = 0;
        DeductionCubeText.gameObject.SetActive(false);
    }

    public void AddCubeCount(int count)
    {
        DeductionCubeText.gameObject.SetActive(true);
        DeductionCubeText.text = "+" + count.ToString();
        DeductionCubeText.color = Color.green;
        StartCoroutine(WaitandDisableDeductCount());
    }

    public void DeductCubeCount(int count)
    {
        DeductionCubeText.gameObject.SetActive(true);
        DeductionCubeText.text = "-" + count.ToString();
        DeductionCubeText.color = Color.red;
        CubeDestroyed -= count;
        if (CubeDestroyed < 0)
        {
            CubeDestroyed = 0;
        }
        StartCoroutine(WaitandDisableDeductCount());
    }

    IEnumerator WaitandDisableDeductCount()
    {
        UpdateUI();
        yield return new WaitForSeconds(5);
        DeductionCubeText.gameObject.SetActive(false);
        UpdateUI();
    }

    #region Set MyCannon Upgrade

    void CheckScore()
    {
        
        if (Mathf.Floor(CubeDestroyed / (float)dividedPartLevel) >= 3)
        {
            gunController.SetMyCannonUpgrade(4);
        }
        else if (Mathf.Floor(CubeDestroyed / (float)dividedPartLevel) >= 2)
        {
            gunController.SetMyCannonUpgrade(3);
        }
        else if (Mathf.Floor(CubeDestroyed / (float)dividedPartLevel) >= 1)
        {
            gunController.SetMyCannonUpgrade(2);
        }
        else 
        {
            gunController.SetMyCannonUpgrade(1);
        }
    }

    #endregion

    #region For Power Booster

    [Header("Booster")]
    public Image BoosterImage;
    float currentUse = 0;
    float cubeCounter = 0;
    bool iscubecounterincrementing = false;
    //deduct score to use this booster or watch ad to use it for free
    void CheckAndUpdateUIForBooster()
    {
        if (CubeDestroyed >= BoosterUnlocker)
        {
            if (currentUse >= saveload.boosterLoadTimer)
            {
                ResetToBoosterController();
            }
            
        }

        if (iscubecounterincrementing == false)
        {
            iscubecounterincrementing = true;
            StartCoroutine(IncrementCurrentUseBooster());
        }
        BoosterUIUpdate();
    }

    IEnumerator IncrementCurrentUseBooster()
    {

        yield return new WaitForSeconds(1);
        if (currentUse < saveload.boosterLoadTimer)
        {
            iscubecounterincrementing = true;
            currentUse++;
            BoosterUIUpdate();
            StartCoroutine(IncrementCurrentUseBooster());
        }
        else
        {
            if (CubeDestroyed >= BoosterUnlocker)
                ResetToBoosterController();
            iscubecounterincrementing = false;
        }
    }

    public void BoosterUsed()
    {
        currentUse = 0;
    }

    void BoosterUIUpdate()
    {
        if (CubeDestroyed < BoosterUnlocker)
            BoosterImage.fillAmount = 1- CubeDestroyed / (float)BoosterUnlocker;
        else
            BoosterImage.fillAmount = 1 - currentUse / (float)saveload.boosterLoadTimer;
    }

    void ResetToBoosterController()
    {
        boosterController.ResetBooster();
    }

    #endregion

    #region Higscore
    [Header("Higscore")]
    public Text HigscoreText;
    public void IncreaseHighscore()
    {
        CubeDestroyed++;
        if (CubeDestroyed > saveload.maxHighscore)
        {
            saveload.maxHighscore = CubeDestroyed;
            saveload.Save();
        }

        AddCubeCount(1);
        UpdateUI();
        CheckConcept();
        CheckAndUpdateUIForBooster();
    }

    public void DecreaseHighscore()
    {
        CubeDestroyed -= 53;
        if (CubeDestroyed < 0)
        {
            GameOverThings();
        }
        StartCoroutine(DeadEffect());
        UpdateUI();
    }

    #endregion

    void GameOverThings()
    {
        CubeDestroyed = 0;
        StartCoroutine(DeadEffect());
    }

    IEnumerator DeadEffect()
    {
        yield return new WaitForSeconds(0.4f);
        GunnerGameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        GunnerGameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GunnerGameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        GunnerGameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GunnerGameObject.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        GunnerGameObject.SetActive(true);
        
    }

    #region GameConcepts of Bar

    void CheckConcept()
    {
        if (CubeDestroyed % dividedPartLevel == 0)
        {
            gunController.MyCannonLevel++;
        }
    }

    #endregion
    [Header("Basic UI")]
    public Image cannonLevelBarImage;

    void UpdateUI()
    {
        HigscoreText.text = CubeDestroyed.ToString();
        UpdateCannonLevelBar();
        CheckScore();
    }

    void UpdateCannonLevelBar()
    {
        float amount = CubeDestroyed / (float)(dividedPartLevel*4);
        if(amount>1)
            amount=1;
        float t = amount;
        
        cannonLevelBarImage.fillAmount = t;
    }
}
