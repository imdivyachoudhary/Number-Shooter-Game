 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
	public float cubeSpeed=-0.01f;
    MainMenuController mainController;
    GameObject MyChildObject;
    void Start()
    {
        cubeSpeed=saveload.cubeSpeed;
        mainController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuController>();
        MyChildObject = transform.Find("Cube").gameObject;
    }

    void Update()
    {

        if (MyChildObject != null && mainController.isGameStart && MyChildObject.GetComponent<Cubes>().isGamePause == false)
        {
            if (MyChildObject.tag == "Blade")
            {
                MyChildObject.GetComponent<Rigidbody>().position += new Vector3(0, 0, cubeSpeed*10);
            }
            else
            {
                MyChildObject.GetComponent<Rigidbody>().position += new Vector3(0, 0, cubeSpeed*3);
            }
            
        }
    }
}
