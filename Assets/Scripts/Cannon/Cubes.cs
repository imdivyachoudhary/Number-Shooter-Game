using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Cubes : MonoBehaviour
{

    public TextMeshProUGUI[] AllTheNumbers;
    public int number;
    public GameObject MyParticle;
    public Material MyColor;
    public Material DamageMaterial;
    public bool isGamePause = false;

    void Start()
    {
        if (gameObject.tag != "Blade")
        SetToAllText();
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(gameObject.tag!="Blade")
        if (col.gameObject.tag == "Ball")
        {
            number -= (int)saveload.myLevel/10;
            SetToAllText();
            //Destroy(col.gameObject);
            CheckIfDead();
            StartCoroutine(DamageAnim());
            FindObjectOfType<AudioManager>().Play("Impact1");
        }

        if (col.gameObject.tag == "Blade")
        {
            number = 0;
            CheckIfDead();
        }

        if (col.gameObject.tag == "Damage")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().DeductCubeCount(number);
            FindObjectOfType<AudioManager>().Play("Impact3");
            Destroy(gameObject);
            
        }
    }

    IEnumerator DamageAnim()
    {
        gameObject.GetComponent<Renderer>().material = DamageMaterial;
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<Renderer>().material = MyColor;
    }

    

    public void CheckIfDead()
    {
        if (number < 1)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            GameObject go = Instantiate(MyParticle, gameObject.transform.position, gameObject.transform.rotation);
            foreach (Transform t in go.transform)
            {
                t.GetComponent<Renderer>().material = MyColor;
            }
            go.GetComponent<Rigidbody>().AddForceAtPosition(Vector3.up*100, gameObject.transform.position, ForceMode.Impulse);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().IncreaseHighscore();
            Destroy(go, 1);
            Destroy(gameObject,1);
            FindObjectOfType<AudioManager>().Play("Impact2");
        }
    }

    void SetToAllText()
    {
        foreach (TextMeshProUGUI g in AllTheNumbers)
        {
            g.text = number.ToString();
        }
    }
}
