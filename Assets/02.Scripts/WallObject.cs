using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObject : MonoBehaviour
{
    private float height = 5f;

    void Awake()
    {
        EventManager.StartListening("GameStart", SetArea);
    }

    private void SetArea()
    {
        Transform areaTransform = GameManager.Instance.CurrentArea.areaTransform;
        transform.position = areaTransform.position;

        switch (transform.GetSiblingIndex())
        {
            case 0:
                transform.localScale = new Vector3(areaTransform.localScale.x, 1f, height);
                transform.position -= areaTransform.localScale.z * 5f * Vector3.forward;
                break;

            case 1:
                transform.localScale = new Vector3(areaTransform.localScale.x, 1f, height);
                transform.position += areaTransform.localScale.z * 5f * Vector3.forward;
                break;

            case 2:
                transform.localScale = new Vector3(height, 1f, areaTransform.localScale.z);
                transform.position += areaTransform.localScale.x * 5f * Vector3.right;
                break;

            case 3:
                transform.localScale = new Vector3(height, 1f, areaTransform.localScale.z);
                transform.position -= areaTransform.localScale.x * 5f * Vector3.right;
                break;
        }

        Vector3 lookPos = areaTransform.position;
        lookPos.y = transform.position.y;
        transform.up = (lookPos - transform.position).normalized;

        transform.position += Vector3.up * height * 5f;
    }
}
