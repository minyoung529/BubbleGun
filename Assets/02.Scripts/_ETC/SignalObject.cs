using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SignalObject : MonoBehaviour
{
    Vector3 signalScale;
    private void Awake()
    {
        EventManager<Area>.StartListening("AreaClear", OnActiveSignal);
        signalScale = transform.localScale;
        gameObject.SetActive(false);
    }

    private void OnActiveSignal(Area area)
    {
        Vector3 scale = signalScale;
        scale.y = 0f;
        transform.localScale = scale;

        transform.DOScale(signalScale, 1f);

        Vector3 position = area.areaTransform.position;
        position.y = transform.localScale.y * 0.5f;
        transform.position = position;

        gameObject.SetActive(true);
    }
}
