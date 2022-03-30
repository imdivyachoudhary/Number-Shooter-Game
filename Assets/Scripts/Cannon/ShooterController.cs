using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {

            if (hit.point.z > 10)
            {
                gameObject.transform.LookAt(hit.point);
                Vector3 pos = new Vector3(0, gameObject.transform.rotation.y, 0);
                gameObject.transform.rotation = Quaternion.EulerAngles(pos);
            }


            //for line swipe
            if (hit.point.z < 10)
            {
                gameObject.transform.localPosition = hit.point;
                gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, 0, 0);
            }

        }
    }
}
