using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public GameController gameController;
    public GameObject Cube;
    public GameObject Blade;
    public Material[] CubeColor;
    public Transform[] CubeSpawnLocation;
    public Transform[] CubeSpawnLocation1;
    public Transform[] CubeSpawnLocation2;
    public Transform[] CubeSpawnLocation3;

    private float timeToSpawnCube;
    public float minCubeSpawnTime=1;
    public float maxCubeSpawnTime=3;
    
    void Start()
    {
        saveload.myLevel = 10;
        timeToSpawnCube=2;
        StartCoroutine(DealayAndSpawnCube());
        StartCoroutine(IncreaseLevel());
    }

    IEnumerator DealayAndSpawnCube()
    {
        yield return new WaitForSeconds(timeToSpawnCube);
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuController>().isGameStart)
        {
            timeToSpawnCube = Random.Range(minCubeSpawnTime, maxCubeSpawnTime);
            int randomLocation = Random.Range(0, CubeSpawnLocation.Length);
            int randomThings = Random.Range(0, 999);
            if (randomThings < 50)
            {
                //Spawn Blade
                GameObject go = Instantiate(Blade, CubeSpawnLocation[randomLocation].transform.position, CubeSpawnLocation[randomLocation].transform.rotation);

            }
            else
            {
               
                    if (Mathf.Floor(gameController.CubeDestroyed / (float)gameController.dividedPartLevel) >= 3)
                    {
                        GameObject go = Instantiate(Cube, CubeSpawnLocation[randomLocation].transform.position, CubeSpawnLocation[randomLocation].transform.rotation);
                        Instantiate(go,4);
                        randomLocation = Random.Range(0, CubeSpawnLocation.Length);
                        go = Instantiate(Cube, CubeSpawnLocation1[randomLocation].transform.position, CubeSpawnLocation1[randomLocation].transform.rotation);
                        Instantiate(go,4);
                        randomLocation = Random.Range(0, CubeSpawnLocation.Length);
                        go = Instantiate(Cube, CubeSpawnLocation2[randomLocation].transform.position, CubeSpawnLocation1[randomLocation].transform.rotation);
                        Instantiate(go,4);
                        randomLocation = Random.Range(0, CubeSpawnLocation.Length);
                        go = Instantiate(Cube, CubeSpawnLocation3[randomLocation].transform.position, CubeSpawnLocation1[randomLocation].transform.rotation);
                        Instantiate(go,4);
                    }
                    else if (Mathf.Floor(gameController.CubeDestroyed / (float)gameController.dividedPartLevel) >= 2)
                    {
                        GameObject go = Instantiate(Cube, CubeSpawnLocation[randomLocation].transform.position, CubeSpawnLocation[randomLocation].transform.rotation);
                        Instantiate(go,3);
                        randomLocation = Random.Range(0, CubeSpawnLocation.Length);
                        go = Instantiate(Cube, CubeSpawnLocation1[randomLocation].transform.position, CubeSpawnLocation1[randomLocation].transform.rotation);
                        Instantiate(go,3);
                        randomLocation = Random.Range(0, CubeSpawnLocation.Length);
                        go = Instantiate(Cube, CubeSpawnLocation2[randomLocation].transform.position, CubeSpawnLocation1[randomLocation].transform.rotation);
                        Instantiate(go,3);
                        
                    }
                    else if (Mathf.Floor(gameController.CubeDestroyed / (float)gameController.dividedPartLevel) >= 1)
                    {
                        GameObject go = Instantiate(Cube, CubeSpawnLocation[randomLocation].transform.position, CubeSpawnLocation[randomLocation].transform.rotation);
                        Instantiate(go,2);
                        randomLocation = Random.Range(0, CubeSpawnLocation.Length);
                        go = Instantiate(Cube, CubeSpawnLocation1[randomLocation].transform.position, CubeSpawnLocation1[randomLocation].transform.rotation);
                        Instantiate(go,2);
                        
                    }
                    else
                    {
                        GameObject go = Instantiate(Cube, CubeSpawnLocation[randomLocation].transform.position, CubeSpawnLocation[randomLocation].transform.rotation);
                        Instantiate(go,1);
                    }
                
                
            }
        }
        StartCoroutine(DealayAndSpawnCube());
    }

    void Instantiate(GameObject go,int num)
    {
        int angle = Random.Range(0, 360);
        go.transform.Find("Cube").rotation = Quaternion.EulerAngles(0, angle, 0);
        int randomMaterial = Random.Range(0, CubeColor.Length);
        go.transform.Find("Cube").GetComponent<Renderer>().material = CubeColor[randomMaterial];
        go.transform.Find("Cube").GetComponent<Cubes>().number = (int)(saveload.myLevel + 5f/(float)num);
        go.transform.Find("Cube").GetComponent<Cubes>().MyColor = CubeColor[randomMaterial];
        Destroy(go, 100);
    }

    IEnumerator IncreaseLevel()
    {
        yield return new WaitForSeconds(20);
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuController>().isGameStart)
        {
            saveload.myLevel++;
        }
        StartCoroutine(IncreaseLevel());
    }

    
}
