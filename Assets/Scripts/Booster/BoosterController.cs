using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    public MainMenuController mainController;

    public GameObject AllCananns;
    public GameObject CannonBallGO;
    public GameObject MyPlayer;
    bool isBoosterActive = false;
    public Animator boosterAnim;
    bool isReadyForShoot = false;

    void Start()
    {
        isBoosterActive = false;
        isReadyForShoot = true;
    }

    public void ResetBooster()
    {
        isBoosterActive = true;
    }

    
    public void ShowAndStartCannon()
    {
        if (isBoosterActive && isReadyForShoot)
        {
            FireSpeacialCannon();
        }
        else
        {
            if (FindObjectOfType<AdScript>().isAdLoaded() && isReadyForShoot)
            ShowWatchAdPannel();
        }
    }

    void FireSpeacialCannon()
    {
        isReadyForShoot = false;
        isBoosterActive = false;
        AllCananns.SetActive(true);
        MyPlayer.SetActive(false);
        boosterAnim.Play("ShowAnim");
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().BoosterUsed();
        StartCoroutine(FireForaTime());
        GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuController>().IncreaseSpeacialCounter();
    }

    IEnumerator FireForaTime()
    {
        yield return new WaitForSeconds(0.5f);
        int timeStay = saveload.RewardCannonStayTime*10;
        
        while (timeStay > 0)
        {
            timeStay--;
            yield return new WaitForSeconds(0.1f);
            
            foreach (Transform cannon in AllCananns.transform)
            {
                //print(cannon.name);
                Transform temp=cannon.Find("BallPos1").transform;
                GameObject go = Instantiate(CannonBallGO, temp.transform.position, temp.transform.rotation);
                go.GetComponent<BallBehave>().MaxCollide = 1;
                
            }
        }
        boosterAnim.Play("HideAnim");
        MyPlayer.SetActive(true);
        isReadyForShoot = true;
    }

    #region watchads system

    [Header("Watch Ads")]
    public GameObject WatchAdPannel;

    void ShowWatchAdPannel()
    {
        mainController.PauseFunction();
        WatchAdPannel.SetActive(true);
    }

    public void OnWatchAdButtonPressed()
    {
        if (FindObjectOfType<AdScript>().isAdLoaded())
        {
            isWatchAdCanels = false;
            FindObjectOfType<AdScript>().ShowRewardVideoAdsSwitch();
            StartCoroutine(WaitForEnd());
        }
    }

    bool isWatchAdCanels = false;
    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(1);
        if (FindObjectOfType<AdScript>().CheckRewardCompleted())
        {
            OnCloseWatchAdPannel();
            FireSpeacialCannon();
        }
        else if (isWatchAdCanels==false)
        {
            StartCoroutine(WaitForEnd());
        }
    }

    public void OnCloseWatchAdPannel()
    {
        mainController.OnResumeFunction();
        isWatchAdCanels = true;
        WatchAdPannel.SetActive(false);
    }

    #endregion


}
