using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeTrigger : MonoBehaviour
{
    void OnStart()
    {
        Destroy(gameObject, 20);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Cube")
        {
            col.gameObject.GetComponent<Cubes>().number = 0;
            col.gameObject.GetComponent<Cubes>().CheckIfDead();
            FindObjectOfType<AudioManager>().Play("Impact2");

        }
        if (col.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("Gunner").GetComponent<GunController>().MyCannonLevel--;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().DecreaseHighscore();
            FindObjectOfType<AudioManager>().Play("Impact3");
            Destroy(gameObject);
        }

        if (col.tag == "Damage")
        {
            Destroy(gameObject);
            
        }
    }
}
