using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineReflection : MonoBehaviour
{
    public int reflections;
    public float maxLenght;

    private LineRenderer lineRenderer;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;

    public Vector3[] myPoints = new Vector3[10];

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        DrayLineAndGetPoints();
   }

    private bool isFound = false;

    public void DrayLineAndGetPoints()
    {
        ray = new Ray(transform.position, transform.forward);
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
        float remaningLength = maxLenght;

        for (int i = 0; i < reflections; i++)
        {
            if (!isFound)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, remaningLength))
                {
                    if (hit.collider.gameObject.tag == "RayTarget")
                    {
                        //hit.collider.gameObject.GetComponent<Raytarget>().PlaceKey();
                    }

                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
                    remaningLength -= Vector3.Distance(ray.direction, hit.normal);
                    ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                    myPoints[i] = hit.point;
                    if (hit.collider.tag != "RayMirror")
                    {
                        break;
                    }
                }
                else
                {
                    lineRenderer.positionCount += 1;
                    lineRenderer.SetPosition(lineRenderer.positionCount - 1, ray.origin + ray.direction * remaningLength);
                }
            }

        }
    }

}
