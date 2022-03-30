using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;


public class saveload : MonoBehaviour
{
    //Server files
    public static string serverLocation = "http://kreasaard.atwebpages.com/DiceShooter/";
    public static string serverCreateAccount = "createaccount.php";
    public static string serverUpdateRepeat = "updaterepeat.php";
    public static string serverUpdateStats = "updatestats.php";
    public static string servergetRank = "getrank.php";
    public static string serveradnet = "adnet.php";
    public static string serverinitialize = "initialize.php";

    //initializeValue
    public static int dividedPartLevel=50;
    public static int boosterTimer = 20;//cube destroyed
    public static int boosterLoadTimer = 60;//sec
    public static float cubeSpeed = -0.01f;


    public static float delayToSpawnBalls=0.3f;
    public static int RewardCannonStayTime = 10;
    public static int myLevel = 1;
   
    public static int swipeTut = 0;
    public static int highScore = 0;
    public static int maxHighscore = 0;
    public static string accountID = " ";
    public static string playerName = " ";
    public static string MyRank = "";
    public static int timePlayed=0;
    public static int speacialTime=0;
    public static int repeatUser = 0;
    public static int adsTime = 0;

    public static bool isMusicOn = true;
    public static bool isSoundEffectOn = true;

    public static string current_filename = "info.dat";

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + current_filename);
        Notebook_Data data = new Notebook_Data();


        data.AccountID = accountID;
        data.PlayerName = Encrypt(playerName);
        data.RepeatUser = repeatUser;
        data.TimePlayed = timePlayed;
        data.AdsTime = adsTime;
        data.SpeacialTime = speacialTime;
        data.SwipeTut = swipeTut;
        data.Highscore = highScore;
        data.MaxHighscore = maxHighscore;
        data.IsMusicOn = isMusicOn;
        data.IsSoundEffectOn = isSoundEffectOn;
        data.DividedPartLevel = dividedPartLevel;
        data.BoosterTimer = boosterTimer;
        data.BoosterLoadTimer = boosterLoadTimer;
        data.CubeSpeed = cubeSpeed;

        bf.Serialize(file, data);
        file.Close();
    }

    public static void Load()
    {

        if (File.Exists(Application.persistentDataPath + "/" + current_filename))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + current_filename, FileMode.Open);/* */
            Notebook_Data data = (Notebook_Data)bf.Deserialize(file);

            accountID=data.AccountID;
            playerName=Decrypt(data.PlayerName);
            repeatUser = data.RepeatUser;
            timePlayed = data.TimePlayed;
            adsTime = data.AdsTime;
            speacialTime = data.SpeacialTime;
            swipeTut = data.SwipeTut;
            highScore = data.Highscore;
            maxHighscore = data.MaxHighscore;
            isMusicOn = data.IsMusicOn;
            isSoundEffectOn = data.IsSoundEffectOn;
            dividedPartLevel = data.DividedPartLevel;
            boosterTimer = data.BoosterTimer;
            boosterLoadTimer = data.BoosterLoadTimer;
            cubeSpeed = data.CubeSpeed;

            file.Close();

        }
        else
        {
            current_filename = "info.dat";
            accountID = " ";
            saveload.Save();

        }
    }

    private static string hash="9452@abc";

    public static string Encrypt(string input)
    {
        byte[] data = UTF8Encoding.UTF8.GetBytes(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB,Padding= PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateEncryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }
        }
    }

    public static string Decrypt(string input)
    {
        byte[] data = Convert.FromBase64String(input);
        using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
        {
            byte[] key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider trip = new TripleDESCryptoServiceProvider() { Key = key, Mode = CipherMode.ECB,Padding= PaddingMode.PKCS7 })
            {
                ICryptoTransform tr = trip.CreateDecryptor();
                byte[] results = tr.TransformFinalBlock(data, 0, data.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }

   
}


[Serializable]
class Notebook_Data
{
    public  string AccountID;
    public  string PlayerName;
    public int RepeatUser;
    public int TimePlayed;
    public int AdsTime;
    public int SpeacialTime;
    public int SwipeTut;
    public int Highscore;
    public int MaxHighscore;
    public bool IsMusicOn;
    public bool IsSoundEffectOn;

    //initialize
    public int DividedPartLevel;
    public int BoosterTimer;
    public int BoosterLoadTimer;
    public float CubeSpeed;
}