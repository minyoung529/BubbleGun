using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalObject : MonoBehaviour
{
    private void Awake()
    {
        EventManager<Area>.StartListening("AreaClear", OnActiveSignal);
        gameObject.SetActive(false);
    }

    private void OnActiveSignal(Area area)
    {
        gameObject.SetActive(true);

        Vector3 position = area.areaTransform.position;
        position.y = transform.localScale.y * 0.5f;
        transform.position = position;
    }
}
