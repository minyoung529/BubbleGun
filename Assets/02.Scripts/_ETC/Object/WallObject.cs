using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public class WallObject : MonoBehaviour
{
    private float height = 5f;
    private float originalZSize;
    private NavMeshObstacle obstacle;

    void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        EventManager.StartListening("GameStart", SetArea);
        EventManager<Area>.StartListening("AreaClear", Inactive);
    }

    private void SetArea()
    {
        if (GameManager.Instance.IsClear) return;

        gameObject.SetActive(true);

        Transform areaTransform = GameManager.Instance.CurrentArea.areaTransform;
        transform.position = areaTransform.position;

        switch (transform.GetSiblingIndex())
        {
            case 0:
                transform.localScale = new Vector3(areaTransform.localScale.x, 1f, 0f);
                transform.DOScaleZ(height, 1f);
                transform.position -= areaTransform.localScale.z * 5f * Vector3.forward;
                break;

            case 1:
                transform.localScale = new Vector3(areaTransform.localScale.x, 1f, 0f);
                transform.DOScaleZ(height, 1f);
                transform.position += areaTransform.localScale.z * 5f * Vector3.forward;
                break;

            case 2:
                transform.localScale = new Vector3(0f, 1f, areaTransform.localScale.z);
                transform.DOScaleX(height, 1f);
                transform.position += areaTransform.localScale.x * 5f * Vector3.right;
                break;

            case 3:
                transform.localScale = new Vector3(0f, 1f, areaTransform.localScale.z);
                transform.DOScaleX(height, 1f);
                transform.position -= areaTransform.localScale.x * 5f * Vector3.right;
                break;
        }

        Vector3 lookPos = areaTransform.position;
        lookPos.y = transform.position.y;
        transform.up = (lookPos - transform.position).normalized;

        transform.position += Vector3.up * height * 5f;

        StartCoroutine(SetObstacleSize());
    }

    private IEnumerator SetObstacleSize()
    {
        yield return new WaitForSeconds(1f);
        obstacle.size = transform.localScale;
    }

    private void Inactive(Area area = null)
    {
        if (transform.GetSiblingIndex() >= 2)
        {
            transform.DOScaleZ(0f, 1f).OnComplete(() => gameObject.SetActive(true));
        }
        else
        {
            transform.DOScaleX(0f, 1f).OnComplete(() => gameObject.SetActive(true));
        }
    }

    private void OnDestroy()
    {
        EventManager.StopListening("GameStart", SetArea);
        EventManager<Area>.StopListening("AreaClear", Inactive);
    }
}
