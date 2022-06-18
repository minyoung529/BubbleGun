using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DangerImage : MonoBehaviour
{
    private Image image;
    public Sound sound;

    void Start()
    {
        image = GetComponent<Image>();

        Sequence sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(0.5f, 1f));
        sequence.Append(image.DOFade(0.8f, 1f));
        sequence.SetLoops(-1, LoopType.Restart);
    }

    private void OnEnable()
    {
        SoundManager.Instance.PlayOneShot(sound.chanel, sound.clip);
    }
}
