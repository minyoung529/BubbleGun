using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

public class StoryText : MonoBehaviour
{
    [TextArea]
    [SerializeField] private List<string> storyText;
    [SerializeField] private Text text;
    [SerializeField] private float textDelay = 0.1f;

    private WaitForSeconds delay = new WaitForSeconds(0.5f);

    [SerializeField] UnityEvent startEvent;
    [SerializeField] UnityEvent endEvent;

    private void OnEnable()
    {
        text = GetComponent<Text>();
        StartCoroutine(ShowTextCoroutine());
    }

    private IEnumerator ShowTextCoroutine()
    {
        startEvent.Invoke();
        yield return delay;

        for (int i = 0; i < storyText.Count; i++)
        {
            text.text = "";
            text.DOText(storyText[i], storyText[i].Length * textDelay);

            yield return delay;
        }

        text.text = "";
        endEvent.Invoke();
    }
}
