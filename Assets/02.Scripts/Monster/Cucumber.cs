using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cucumber : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    new private Collider collider;

    int cucomberColor = Shader.PropertyToID("_BaseColor");

    void Awake()
    {
        meshRenderer ??= GetComponent<MeshRenderer>();
        collider ??= GetComponent<Collider>();
    }

    public void Ready()
    {
        meshRenderer ??= GetComponent<MeshRenderer>();
        collider ??= GetComponent<Collider>();

        meshRenderer.material.SetColor(cucomberColor, Color.red);
        collider.enabled = false;
    }

    public void OnActive()
    {
        meshRenderer.material.SetColor(cucomberColor, Color.white);
        collider.enabled = true;
    }
}
