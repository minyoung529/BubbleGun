using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObj : MonoBehaviour
{
    private void Awake()
    {
        Clicker.onClick += OnClicked;
    }

    public void OnClicked(GameObject obj)
    {
        if(obj == gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}
