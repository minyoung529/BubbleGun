using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pcpc : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            GameObject obj = FindNearestObjectByTag("Enemy");
            obj.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    private GameObject FindNearestObjectByTag(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        GameObject nearestObject = objects.OrderBy(x =>
        {
            return Vector3.Distance(transform.position, x.transform.position);
        }).FirstOrDefault();

        return nearestObject;
    }

}
