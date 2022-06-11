using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    [SerializeField] private Color[] colors;
    [SerializeField] private Color[] baseColors;

    [SerializeField] private bool isPairColorAndBase;

    [Header("Change Color On Update")]
    [SerializeField] private bool isUpdate = false;
    [SerializeField] private float maxTime = 1f;
    private float timer = 0f;

    private MeshRenderer meshRenderer;

    [Header("Apply Trail Renderer")]
    [SerializeField] private bool isTrailRenderer = true;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        ChangeColor();

        if (isTrailRenderer)
        {
            GetComponentInChildren<TrailRenderer>().material = meshRenderer.material;
        }
    }

    private void Update()
    {
        if(isUpdate)
        {
            timer += Time.deltaTime;

            if (timer > maxTime)
            {
                ChangeColor();
                timer = 0f;
            }
        }
    }

    private void ChangeColor()
    {
        int index = Random.Range(0, colors.Length);
        meshRenderer.material.SetColor("_BaseColor", colors[index]);

        if (!isPairColorAndBase)
        {
            index = Random.Range(0, baseColors.Length);
        }

        meshRenderer.material.SetColor("_1st_ShadeColor", baseColors[index]);
    }
}
