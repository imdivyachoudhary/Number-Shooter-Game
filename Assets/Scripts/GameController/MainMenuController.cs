using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuController : MonoBehaviour
{

    public GameObject[] GamePannelObjects;
    public GameObject MainMenuPannel;
    void Awake()
    {
        saveload.Load();
        isGameStart = false;
        CheackAccount();
        StartCoroutine(IncreaseTimePlayed());
    }

    #region account creation

    void CheackAccount()
    {
        if (saveload.accountID == " ")
        {
            //means create new account
            StartCoroutine(CreateAccountToServer());
        }
        else
        {
            //update repeat user
            UpdateRepeat();
        }
        InitializeValues();
    }

    IEnumerator CreateAccountToServer()
    {
        saveload.playerName = "Player" + Random.Range(1111, 99999);
        saveload.Save();
        WWWForm form1 = new WWWForm();
        form1.AddField("name", saveload.playerName);
        WWW www = new WWW(saveload.serverLocation + saveload.serverCreateAccount, form1);
        yield return www;
        
        if (www.text != "" && www.text!=" " && !www.text.Contains("<"))
        {
            string ane = GetDataValue(www.text,"Created:");
            saveload.accountID = ane;
            saveload.Save();
        }
        
    }

    #endregion

    public GameObject TapButton;
    public bool isGameStart = false;
    public void OnTapButtonPressed()
    {
        SwipeButton.SetActive(false);
        TapButton.SetActive(false);
        isGameStart = true;
        CheckForSwipeTut();
        MainMenuPannel.SetActive(false);
        foreach (GameObject g in GamePannelObjects)
        {
            g.SetActive(true);
        }
        gameObject.GetComponent<HigscoreController>().HideHighscorepannel();
        FindObjectOfType<AudioManager>().Play("Click1");
    }

    #region Swipe Tut

    [Header("Swipe Mech")]
    public GameObject SwipeButton;
    void CheckForSwipeTut()
    {
        SwipeButton.SetActive(false);
        if (saveload.swipeTut == 0)
        {
            saveload.swipeTut = 1;
            SwipeButton.SetActive(true);
            StartCoroutine(DisableSwipeButton());
        }
        else
        {
            
            SwipeButton.SetActive(true);
            StartCoroutine(DisableSwipeButton());
        }
    }

    IEnumerator DisableSwipeButton()
    {
        yield return new WaitForSeconds(4);
        SwipeButton.SetActive(false);
    }

    #endregion

    #region PauseSystem

    [Header("Pause Mec")]
    public GameObject PausePannel;
    
    public void OnPauseButtonPressed()
    {
        PausePannel.SetActive(true);
        PauseFunction();
    }

    public void PauseFunction()
    {
        GameObject[] gg = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject g in gg)
            g.GetComponent<Cubes>().isGamePause = true;

        GameObject[] gdo = GameObject.FindGameObjectsWithTag("Blade");
        foreach (GameObject g in gdo)
        {
            if (g.GetComponent<Cubes>() != null)
                g.GetComponent<Cubes>().isGamePause = true;
        }

        Time.timeScale = 0;
        gameObject.GetComponent<HigscoreController>().LoadAndShowHighscorePannel();
        FindObjectOfType<AudioManager>().Play("Click1");
    }

    public void OnCancelOrResumeButtonPressed()
    {
        PausePannel.SetActive(false);
        OnResumeFunction();
    }

    public void OnResumeFunction()
    {
        Time.timeScale = 1;
        GameObject[] go = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject g in go)
            g.GetComponent<Cubes>().isGamePause = false;

        GameObject[] gdo = GameObject.FindGameObjectsWithTag("Blade");
        foreach (GameObject g in gdo)
        {
            if (g.GetComponent<Cubes>() != null)
                g.GetComponent<Cubes>().isGamePause = false;

        }
        gameObject.GetComponent<HigscoreController>().HideHighscorepannel();
        FindObjectOfType<AudioManager>().Play("Click1");
    }

    #endregion

    #region Setting
    [Header("Setting Mech")]
    public Text ReportText;
    public GameObject SettingPannel;
    public Sprite MusicOnSprite;
    public Sprite MusicOfSprite;
    public Image MusicImage;
    public Sprite SoundEffectOnSprite;
    public Sprite SoundEffectOfSprite;
    public Image SoundEffectImage;

    bool isMusicOn = true;
    bool isSoundEffectOn = true;


    public void OnSettingButtonPressed()
    {
        SettingPannel.SetActive(true);
        InitializeSettingThings();
        FindObjectOfType<AudioManager>().Play("Click1");
    }

    public void OnSettingCloseButtonPressed()
    {
        SettingPannel.SetActive(false);
        FindObjectOfType<AudioManager>().Play("Click1");
    }

    void InitializeSettingThings()
    {
        isMusicOn = saveload.isMusicOn;
        isSoundEffectOn = saveload.isSoundEffectOn;
        SetImageOfMusicAndSoundEffect();
        nameInputField.text = saveload.playerName;
    }

    public void OnMusicButtonPressed()
    {
        if (isMusicOn)
            isMusicOn = false;
        else
            isMusicOn = true;
        SetImageOfMusicAndSoundEffect();
        FindObjectOfType<AudioManager>().Play("Click1");
    }

    public void OnSoundEffectButtonPressed()
    {
        if (isSoundEffectOn)
        {
            isSoundEffectOn = false;
            saveload.isSoundEffectOn = false;
            FindObjectOfType<AudioManager>().Mute();
        }
        else
        {
            isSoundEffectOn = true;
            saveload.isSoundEffectOn = true;
            FindObjectOfType<AudioManager>().UnMute();
        }
        SetImageOfMusicAndSoundEffect();
        FindObjectOfType<AudioManager>().Play("Click1");
        
    }

    

    void SetImageOfMusicAndSoundEffect()
    {
        if (isMusicOn)
            MusicImage.sprite = MusicOnSprite;
        else
            MusicImage.sprite = MusicOfSprite;
        if (isSoundEffectOn)
            SoundEffectImage.sprite = SoundEffectOnSprite;
        else
            SoundEffectImage.sprite = SoundEffectOfSprite;
    }

    #region setting change Name

    [Header("Change Name")]
    public InputField nameInputField;
    public void ChangeName()
    {
        string name = nameInputField.text.ToString();
        if (name != "" && name != null)
        {
            //change name
            saveload.playerName = name;
            //on server after every 10 second name will be change on the server
            saveload.Save();
            ReportText.gameObject.SetActive(true);
            ReportText.text = "Name Changed";
            gameObject.GetComponent<HigscoreController>().LoadAndShowHighscorePannel();
            StartCoroutine(HideReportText());
        }
    }


    IEnumerator HideReportText()
    {
        yield return new WaitForSeconds(3);
        ReportText.gameObject.SetActive(false);
    }
    #endregion

    #endregion

    #region Updating repeat to server

    void UpdateRepeat()
    {
        saveload.repeatUser++;
        saveload.Save();
        print(saveload.repeatUser);
        StartCoroutine(UpdateRepeatToServer());
    }

    IEnumerator UpdateRepeatToServer()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        form1.AddField("repeat", saveload.repeatUser);

        WWW www = new WWW(saveload.serverLocation + saveload.serverUpdateRepeat, form1);
        yield return www;

    }

    #endregion

    public void OnPrivacyPolicyButtonPressed()
    {
        Application.OpenURL("https://kreasarstudio.wixsite.com/home/dice-shooter");
        FindObjectOfType<AudioManager>().Play("Click1");
    }

    #region Counters

    public void IncreaseSpeacialCounter()
    {
        saveload.speacialTime++;
        saveload.Save();
    }

    public void IncreaseAdsCounter()
    {
        saveload.adsTime++;
        saveload.Save();
    }   //TODO

    IEnumerator IncreaseTimePlayed()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            saveload.timePlayed += 10;
            saveload.Save();

            if (saveload.accountID != " ")
            {
                //send data to sever
                UpdateStats();
            }
        }
    }

    #endregion

    #region Update TimePlayed And Rest Data

    public void UpdateStats()
    {
        StartCoroutine(UpdateStatsServer());
    }

    IEnumerator UpdateStatsServer()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        form1.AddField("name", saveload.playerName);
        form1.AddField("timeplayed", saveload.timePlayed);
        form1.AddField("higscore", saveload.maxHighscore);
        form1.AddField("ads", saveload.adsTime);
        form1.AddField("speacial", saveload.speacialTime);
        WWW www = new WWW(saveload.serverLocation + saveload.serverUpdateStats, form1);
        yield return www;
        

    }

    #endregion

    #region Initialize Values From Server

    void InitializeValues()
    {
        StartCoroutine(InitializeFromServer());
    }

    IEnumerator InitializeFromServer()
    {
        WWW www = new WWW(saveload.serverLocation + saveload.serverinitialize);
        yield return www;
        print(www.text);
        //dpl:50|bt:20|blt:60|cs:-0.01|
        if (www.text != ""&& www.text.Contains("dpl"))
        {
            saveload.dividedPartLevel =  int.Parse(GetDataValue(www.text, "dpl:"));
            saveload.boosterTimer = int.Parse(GetDataValue(www.text, "bt:"));
            saveload.boosterLoadTimer = int.Parse(GetDataValue(www.text, "blt:"));
            saveload.cubeSpeed = float.Parse(GetDataValue(www.text, "cs:"));
            saveload.Save();
        }
        
    }

    #endregion

    string GetDataValue(string data, string index)
    {
        string value = data.Substring(data.IndexOf(index) + index.Length);
        if (value.Contains("|"))
            value = value.Remove(value.IndexOf("|"));
        return value;
    }
}
