using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HigscoreController : MonoBehaviour
{
    public GameObject HighscorePannel;
    public Text MyRank;
    public Text MyName;
    public Text MyScore;
    bool isConnectedToInternet = false;

    bool isActiveHigscore = false;
    void Start()
    {
        isConnectedToInternet = false;
        LoadAndShowHighscorePannel();
        HighscorePannel.SetActive(false);
    }

    public void HideHighscorepannel()
    {
        isActiveHigscore = false;
        HighscorePannel.SetActive(false);
    }

    public void LoadAndShowHighscorePannel()
    {
        isActiveHigscore = true;
        HighscorePannel.SetActive(true);
        MyName.text = saveload.playerName;
        MyScore.text = saveload.maxHighscore.ToString();
        StartCoroutine(LoadHigscoreDataFromLocal());

        if(isConnectedToInternet==false)
            HighscorePannel.SetActive(false);
    }

    #region HigscoreArea

    IEnumerator LoadHigscoreDataFromLocal()
    {
        yield return new WaitForSeconds(1);

        if (saveload.accountID != " ")
        {
            //get the currentRank
            StartCoroutine(Getrank());
        }
        else
        {
            StartCoroutine(LoadHigscoreDataFromLocal());
        }
    }

    IEnumerator Getrank()
    {
        WWWForm form1 = new WWWForm();
        form1.AddField("id", saveload.accountID);
        WWW www = new WWW(saveload.serverLocation + saveload.servergetRank, form1);
        yield return www;
        if (www.text == "")
        {
            isConnectedToInternet = false;
        }
        else
        {
            isConnectedToInternet = true;
        }
        saveload.MyRank = www.text;
        saveload.Save();
        SetAllData();
        
    }

    void SetAllData()
    {
        if (isActiveHigscore)
        HighscorePannel.SetActive(true);
        MyRank.text = saveload.MyRank;
        MyName.text = saveload.playerName;
        MyScore.text = saveload.maxHighscore.ToString();

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
