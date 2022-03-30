using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public GameObject BallGO;
    public GameObject[] MyCannons;
    public int MyCannonLevel = 1;

    
    void OnEnable()
    {
        oldversion = 0;
        MyCannonLevel =1;
        StartCoroutine(WaitAndSpawnCannonBall());
    }

    IEnumerator WaitAndSpawnCannonBall()
    {
        yield return new WaitForSeconds(saveload.delayToSpawnBalls);
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuController>().isGameStart)
        {
            
            SpawnBullets();
        }
        StartCoroutine(WaitAndSpawnCannonBall());
    }

    int oldversion = 0;
    public void SetMyCannonUpgrade(int num)
    {
        MyCannonLevel = num;
        if (MyCannonLevel >= MyCannons.Length)
            MyCannonLevel = MyCannons.Length - 1;
        if (MyCannonLevel < 1)
        {
            MyCannonLevel = 1;
        }
        
        for (int i = 0; i < MyCannons.Length; i++)
        {
            MyCannons[i].SetActive(false);
        }

        for (int i = 0; i <= MyCannonLevel; i++)
        {
            MyCannons[i].SetActive(true);
        }

        if(oldversion<MyCannonLevel)
            FindObjectOfType<AudioManager>().Play("Upgrade");
        oldversion = MyCannonLevel;
    }



    void SpawnBullets()
    {
        for (int i = 0; i <= MyCannonLevel; i++)
        {
            GameObject go = Instantiate(BallGO, MyCannons[i].transform.Find("Pos").transform.position, MyCannons[i].transform.Find("Pos").transform.rotation);
        }
        
    }
    
}
