using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehave : MonoBehaviour
{
    int isCollide = 0;
    public int MaxCollide = 3;
    int translatecollider= 0;
    void OnCollisionEnter(Collision col)
    {
        if (MaxCollide > 0)
        {
            translatecollider++;
            if (translatecollider == 1)
            {
                ChangeVelocityBeforeColide();
            }
            var speed = lastVelocity.magnitude;
            Vector3 _wallNormal = col.contacts[0].normal;
            Vector3 m_dir = Vector3.Reflect(lastVelocity.normalized, _wallNormal);
            isCollide++;
            gameObject.GetComponent<Rigidbody>().velocity = m_dir * speed;
            MaxCollide--;
        }
        else
        {
            Destroy(gameObject);
        }
       
        
    }
    Vector3 CurrentDestination;

    void Start()
    {
        translatecollider = 0;
        Destroy(gameObject,1);
        CurrentDestination = gameObject.GetComponent<LineReflection>().myPoints[0];
        gameObject.GetComponent<Rigidbody>().velocity =Vector3.forward * 2;
        gameObject.transform.Translate(Vector3.forward * 1f);
        //gameObject.GetComponent<Rigidbody>().AddForce( CurrentDestination *10f,ForceMode.Force);
    }

    Vector3 lastVelocity;

    void Update()
    {
        /*
        if (isCollide == 0)
        {
            gameObject.transform.Translate(0, 0, 0.1f);
            
        }
        else if (isCollide == 1)
        {
            gameObject.GetComponent<Rigidbody>().velocity = CurrentDestination * 5;
        }*/

        if (translatecollider == 0)
        {
            gameObject.transform.Translate(Vector3.forward * 0.7f);
            
        }
        if (translatecollider == 1)
        {
            //gameObject.GetComponent<Rigidbody>().velocity = Vector3.forward * 20;
            translatecollider++;
        }
        else
        {
            lastVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        }
        //lastVelocity = gameObject.GetComponent<Rigidbody>().velocity;
        
        if (lastVelocity.x == 0 && lastVelocity.z == 0 && translatecollider>1)
        {
            Destroy(gameObject);
        }
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, -4.5f, gameObject.transform.position.z);
    }

    void ChangeVelocityBeforeColide()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.forward * 20;
        lastVelocity = Vector3.forward * 20;
    }

    /*
    void Start()
    {
        Destroy(gameObject, 8);
        CurrentDestination = gameObject.GetComponent<LineReflection>().myPoints[0];
    }

    public Vector3 CurrentDestination;
    void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, CurrentDestination) < 0.3f)
        {
            
            CurrentDestination = gameObject.GetComponent<LineReflection>().myPoints[1];
            gameObject.transform.LookAt(CurrentDestination);
            gameObject.GetComponent<LineReflection>().DrayLineAndGetPoints();
        }
        gameObject.transform.Translate(0, 0, 0.1f);
    }
     */
}
