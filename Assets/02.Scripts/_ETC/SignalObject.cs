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
        EventManager.StartListening("Boss", Inactive);
        signalScale = transform.localScale;
        transform.localScale = Vector3.zero;
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
    }

    private void Inactive()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager<Area>.StopListening("AreaClear", OnActiveSignal);
    }
}
