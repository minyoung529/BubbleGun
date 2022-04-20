using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    public delegate void OnClickEvent(GameObject obj);
    public static event OnClickEvent onClick;

    private void Start()
    {
        
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
        RaycastHit raycastHit;

        if(Physics.Raycast(ray,out raycastHit, 100f))
        {
            if(Input.GetMouseButtonDown(0))
            {
                onClick.Invoke(raycastHit.transform.gameObject);
            }
        }
    }
}
