using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FollowCamera : MonoBehaviour
{
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

    [SerializeField] private LayerMask obstacleLayer;

    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        curDistance = distance;
        forward = -targetTransform.forward;
    }

    void Update()
    {
        float x = Input.GetAxisRaw("Mouse X") * rotateSpeed;
        angle += x;

        forward.x += Mathf.Sin(angle * Mathf.Deg2Rad);
        forward.z += Mathf.Cos(angle * Mathf.Deg2Rad);
        forward.Normalize();

        Vector3 pos = targetTransform.position
                      + (forward * curDistance)
                      + (Vector3.up * height);

        Ray ray = new Ray(targetTransform.position, transform.position - targetTransform.position);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, distance, obstacleLayer))
        {
            pos = targetTransform.position
                      + (forward * (hitInfo.distance - 2.5f))
                      + (Vector3.up * height);

            cameraTransform.position = Vector3.Slerp(cameraTransform.position, pos, moveDamping * Time.deltaTime);
        }
        else
        {
            cameraTransform.position = Vector3.Slerp(cameraTransform.position, pos, moveDamping * Time.deltaTime);
        }

        cameraTransform.LookAt(targetTransform.position + (targetTransform.up * targetOffset));

        cameraDirection = cameraTransform.forward;
        cameraDirection.y = 0f;
        cameraDirection.Normalize();
    }
}
