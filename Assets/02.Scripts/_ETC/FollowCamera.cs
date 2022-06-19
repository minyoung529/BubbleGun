using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowCamera : MonoBehaviour
{
    [Header("Follow")]
    public Transform targetTransform;

    private Transform cameraTransform;

    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;
    private float curDistance;

    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    public float moveDamping = 15f;
    public float rotateDamping = 10f;
    public float rotateSpeed = 2.0f;

    public float targetOffset = 2.0f;
    private Vector3 forward = Vector3.zero;

    float angle = 0;

    public static Vector3 cameraDirection;

    private bool canFollow = true;

    [Header("Cut Scene")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Transform cityView;

    public Sound shuttleSound;

    private Vector3 TargetLookAtPosition
    {
        get => targetTransform.position + (targetTransform.up * targetOffset);
    }

    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        curDistance = distance;
        forward = -targetTransform.forward;

        EventManager<Area>.StartListening("AreaClear", Clear);
        EventManager.StartListening("Win", ShowCity);
    }

    void Update()
    {
        if (canFollow)
        {
            FollowTarget();
        }

        cameraDirection = cameraTransform.forward;
        cameraDirection.y = 0f;
        cameraDirection.Normalize();
    }

    private void FollowTarget()
    {
        float x = Input.GetAxisRaw("Mouse X") * rotateSpeed;
        angle += x;

        //Ray ray = new Ray(targetTransform.position, transform.position - targetTransform.position);
        //RaycastHit hitInfo;

        //if (Physics.Raycast(ray, out hitInfo, distance, obstacleLayer))
        //{
        //    pos = targetTransform.position
        //              + (forward * (hitInfo.distance - 2.5f))
        //              + (Vector3.up * height);

        //    cameraTransform.position = Vector3.Slerp(cameraTransform.position, pos, moveDamping * Time.deltaTime);
        //}
        //else
        //{
        cameraTransform.position = Vector3.Slerp(cameraTransform.position, GetCameraPosition(), moveDamping * Time.deltaTime);
        //}

        cameraTransform.LookAt(TargetLookAtPosition);
    }

    private Vector3 GetCameraPosition()
    {
        forward.x += Mathf.Sin(angle * Mathf.Deg2Rad);
        forward.z += Mathf.Cos(angle * Mathf.Deg2Rad);
        forward.Normalize();

        Vector3 pos = targetTransform.position
                      + (forward * curDistance)
                      + (Vector3.up * height);
        return pos;
    }

    private void Clear(Area area)
    {
        Vector3 targetPoint = area.areaTransform.position;
        targetPoint.y += 3f;
        targetPoint -= (targetPoint - transform.position).normalized * 10f;

        SoundManager.Instance.PlayOneShot(shuttleSound.chanel, shuttleSound.clip);
        ShuttleMove(targetPoint, targetPoint, TargetLookAtPosition, 3f);
    }

    private void ShowCity()
    {
        ShuttleMove
            (
            cityView.position, Vector3.zero,
           TargetLookAtPosition, 5f,
            GameManager.Instance.UIManager.OnGameEnd
            );
    }

    private void ShuttleMove(Vector3 targetPoint, Vector3 startTarget, Vector3 endTarget, float time, TweenCallback callback = null)
    {
        Sequence seq = DOTween.Sequence();

        canFollow = false;
        GameManager.Instance.GameState = GameState.None;

        seq.SetDelay(1.5f);

        seq.Append(transform.DOMove(targetPoint, time));
        seq.Join(transform.DOLookAt(startTarget, time));

        seq.Append(transform.DOMove(GetCameraPosition(), time));
        seq.Append(transform.DOLookAt(endTarget, time));

        seq.AppendCallback(() => GameManager.Instance.GameState = GameState.Ready);
        seq.AppendCallback(() => canFollow = true);

        if (callback != null)
            seq.AppendCallback(callback);
    }

    private void OnDestroy()
    {
        EventManager<Area>.StopListening("AreaClear", Clear);
        EventManager.StopListening("Win", ShowCity);
    }
}
