using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Color[] baseColors;

    [SerializeField] private bool isPairColorAndBase;

    private TrailRenderer trailRenderer;

    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        int index = Random.Range(0, colors.Length);
        meshRenderer.material.SetColor("_BaseColor", colors[index]);

        if(!isPairColorAndBase)
        {
            index = Random.Range(0, baseColors.Length);
        }

        meshRenderer.material.SetColor("_1st_ShadeColor", baseColors[index]);

        trailRenderer.material = meshRenderer.material;
    }
}
