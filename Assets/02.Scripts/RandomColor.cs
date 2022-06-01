using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    private TrailRenderer trailRenderer;

    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();

        Color randomColor = colors[Random.Range(0, colors.Length)];
        meshRenderer.material.SetColor("_BaseColor", randomColor);

        Color32 color = randomColor;
        color.r -= 50;
        color.r = (byte)Mathf.Clamp(color.r, 0, 255);
        color.g -= 50;
        color.r = (byte)Mathf.Clamp(color.g, 0, 255);
        color.b -= 50;
        color.r = (byte)Mathf.Clamp(color.b, 0, 255);

        randomColor = color;

        meshRenderer.material.SetColor("_1st_ShadeColor", randomColor);

        trailRenderer.material = meshRenderer.material;
    }
}
