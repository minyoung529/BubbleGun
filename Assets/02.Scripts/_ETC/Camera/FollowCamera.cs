using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FollowCamera : MonoBehaviour
{
    [Header("Follow")]
    public Transform targetTransform;

    [Range(2.0f, 20.0f)]
    public float distance = 10.0f;

    [Range(0.0f, 10.0f)]
    public float height = 2.0f;

    public float moveDamping = 15f;
    public float rotateSpeed = 2.0f;

    public float targetOffset = 2.0f;
    private Vector3 forward = Vector3.zero;

    float angle = 0;

    public static Vector3 cameraDirection;

    private bool canFollow = true;

    public Sound shuttleSound;

    void Start()
    {
        forward = -targetTransform.forward;

        EventManager<Area>.StartListening("AreaClear", Clear);
        EventManager.StartListening("Win", ShowCity);
    }

    void LateUpdate()
    {
        if (canFollow)
        {
            FollowTarget();
        }

        cameraDirection = transform.forward;
        cameraDirection.y = 0f;
        cameraDirection.Normalize();
    }

    private void FollowTarget()
    {
        float x = Input.GetAxisRaw("Mouse X") * rotateSpeed;
        angle += x;

        transform.position = Vector3.Slerp(transform.position, GetCameraPosition(), moveDamping * Time.deltaTime);
        transform.rotation = Rotate();
    }

    private Vector3 GetCameraPosition()
    {
        forward.x += Mathf.Sin(angle * Mathf.Deg2Rad);
        forward.z += Mathf.Cos(angle * Mathf.Deg2Rad);
        forward.Normalize();

        Vector3 pos = targetTransform.position
                      + (forward * distance)
                      + (Vector3.up * (height + targetOffset));

        return pos;
    }

    private void Clear(Area area)
    {
        Vector3 targetPoint = area.areaTransform.position;
        targetPoint.y += 3f;
        targetPoint -= (targetPoint - transform.position).normalized * 10f;

        SoundManager.Instance.PlayOneShot(shuttleSound.chanel, shuttleSound.clip);
        StartCoroutine(CameraScene(targetPoint, GetCameraPosition(), 3f));
    }

    private void ShowCity()
    {
        canFollow = false;
        transform.DOLookAt(targetTransform.position, 10f);
        transform.DOMove(new Vector3(-210, 246, -174), 10f);
    }

    Quaternion Rotate()
    {
        Quaternion LookRot = Quaternion.LookRotation(targetTransform.position - transform.position);
        Vector3 angles = LookRot.eulerAngles;
        angles.x = 12f;
        LookRot.eulerAngles = angles;

        return LookRot;
    }

    private IEnumerator CameraScene(Vector3 targetPoint, Vector3 endTarget, float time, System.Action callback = null)
    {
        Quaternion rotation = transform.rotation;
        canFollow = false;
        GameManager.Instance.GameState = GameState.None;

        yield return new WaitForSeconds(1.5f);

        transform.DOMove(targetPoint, time);
        transform.DOLookAt(targetPoint, time);

        yield return new WaitForSeconds(time);

        transform.DOMove(endTarget, time);
        transform.DORotateQuaternion(rotation,time);

        yield return new WaitForSeconds(time);

        canFollow = true;

        GameManager.Instance.GameState = GameState.Ready;

        callback?.Invoke();
    }

    private void OnDestroy()
    {
        EventManager<Area>.StopListening("AreaClear", Clear);
        EventManager.StopListening("Win", ShowCity);
    }
}