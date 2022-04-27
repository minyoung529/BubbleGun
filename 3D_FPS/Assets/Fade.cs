using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public Image fadeImage;
    private Coroutine coroutine;
    private static readonly WaitForSeconds fadeDelay = new WaitForSeconds(0.001f);

    private void Start()
    {
        coroutine = StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopCoroutine(coroutine);
        }
    }

    private IEnumerator FadeIn()
    {
        Color start = fadeImage.color;
        int count = 100;

        for (int i = 0; i < count; i++)
        {
            start.a = start.a - 1 / (float)count;
            fadeImage.color = start;
            yield return fadeDelay;
        }
    }
}
