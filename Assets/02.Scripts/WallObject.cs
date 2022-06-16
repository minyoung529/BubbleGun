using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    private float height = 100f;

    void Awake()
    {
        //EventManager<Area>.StartListening("AreaClear", SetArea);

        //TEST
        Area area = new Area();
        area.areaTransform = GameObject.Find("Area1").transform;
        SetArea(area);
    }

    private void SetArea(Area area)
    {
        Transform areaTransform = area.areaTransform;
        transform.position = areaTransform.position;

        switch (transform.GetSiblingIndex())
        {
            case 0:
                transform.localScale = new Vector3(height, 1f, areaTransform.localScale.x);
                transform.position -= new Vector3(areaTransform.localScale.x * 5f, 0f);
                break;

            case 1:
                transform.localScale = new Vector3(height, 1f, areaTransform.localScale.x);
                transform.position += new Vector3(areaTransform.localScale.x * 5f, 0f);
                break;

            case 2:
                break;

            case 3:
                break;
        }
    }
}
