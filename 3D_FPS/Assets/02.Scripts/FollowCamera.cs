using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform targetTransform;
    private Transform cameraTransform;

    [Range(2f, 20f)]
    public float distance = 10f;

    [Range(0f, 10f)]
    public float height = 2f;

    public float moveDamping = 15f;
    public float rotateDamping = 10f;

    public float targetOffset = 2f;

    void Start()
    {
        cameraTransform = GetComponent<Transform>();
    }

    void Update()
    {
        Vector3 pos = targetTransform.position
                    + (-targetTransform.forward * distance)
                    + (Vector3.up * height);
        // cam
        // |
        // |¤Ñ¤Ñ¤Ñ player

        cameraTransform.position = Vector3.Slerp(cameraTransform.position, pos, Time.deltaTime * moveDamping);
        cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, targetTransform.rotation, Time.deltaTime * rotateDamping);
        cameraTransform.LookAt(targetTransform.position + Vector3.up * targetOffset);
        //+ offset
    }
}
