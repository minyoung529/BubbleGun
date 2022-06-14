using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoryText : MonoBehaviour
{
    [TextArea]
    [SerializeField] private List<string> storyText;
    [SerializeField] private Text text;

    private WaitForSeconds textDelay = new WaitForSeconds(2.5f);

    private void OnEnable()
    {
        text = GetComponent<Text>();
        StartCoroutine(ShowTextCoroutine());
    }

    private IEnumerator ShowTextCoroutine()
    {
        float delay = 0.1f;
        yield return textDelay;

        for (int i = 0; i < storyText.Count; i++)
        {
            text.text = "";
            text.DOText(storyText[i], storyText[i].Length * delay);

            yield return textDelay;
        }

        text.text = "";
    }
}
