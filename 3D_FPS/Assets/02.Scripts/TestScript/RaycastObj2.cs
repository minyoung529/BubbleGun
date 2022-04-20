using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObj2 : MonoBehaviour
{
    MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        Clicker.onClick += OnClicked;
    }

    public void OnClicked(GameObject obj)
    {
        if (obj == gameObject)
        {
            meshRenderer.material.color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
}
