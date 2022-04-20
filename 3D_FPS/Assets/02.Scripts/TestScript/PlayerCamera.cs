using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("카메라 기본 속성")]
    private Transform cameraTransform = null;
    private Transform targetTransform = null;

    public GameObject targetObject = null;

    public enum CameraTypeState { First, Second, Third }
    public CameraTypeState cameraTypeState = CameraTypeState.Third;

    [Header("3인칭 카메라")]
    public float distance = 6.0f;
    public float height = 1.75f;

    public float heightDamping = 2f;
    public float rotationDamping = 3f;

    [Header("2인칭 카메라")]
    public float rotationSpeed;

    [Header("1인칭 카메라")]
    public float detailX = 5.0f;
    public float detailY = 5.0f;

    public float rotationX = 0.0f;
    public float rotationY = 0.0f;

    public Transform firstCameraTransform;

    void Start()
    {
        cameraTransform = GetComponent<Transform>();
        targetTransform = targetObject?.transform;
    }

    private void LateUpdate()
    {
        if (targetObject == null) return;
        if (targetTransform == null) targetTransform = targetObject.transform;

        switch (cameraTypeState)
        {
            case CameraTypeState.First:
                FistCam();
                break;
            case CameraTypeState.Second:
                SecondCam();
                break;
            case CameraTypeState.Third:
                ThirdCam();
                break;
        }
    }

    private void FistCam()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotationX = cameraTransform.eulerAngles.y + mouseX * detailX;
        rotationX = (rotationX > 180f) ? rotationX - 360f : rotationX;

        rotationY += mouseY * detailY;
        rotationY = Mathf.Clamp(rotationY, -40f, 80f);

        cameraTransform.eulerAngles = new Vector3(-rotationY, rotationX, 0f);
        cameraTransform.position = firstCameraTransform.position;
    }

    private void SecondCam()
    {
        cameraTransform.RotateAround(targetTransform.position, Vector3.up, rotationSpeed * Time.deltaTime);
        cameraTransform.LookAt(targetTransform.position);
    }

    private void ThirdCam()
    {
        float targetAngle = targetTransform.eulerAngles.y;
        float targetHeight = targetTransform.position.y + height;
        float cameraAngle = cameraTransform.eulerAngles.y;
        float cameraHeight = cameraTransform.position.y;

        cameraAngle = Mathf.LerpAngle(cameraAngle, targetAngle, rotationDamping * Time.deltaTime);
        cameraHeight = Mathf.Lerp(cameraHeight, targetHeight, heightDamping * Time.deltaTime);

        Quaternion nowRotation = Quaternion.Euler(0f, cameraAngle, 0f);
        cameraTransform.rotation = nowRotation;

        cameraTransform.position = targetTransform.position;
        cameraTransform.position -= nowRotation * Vector3.forward * distance;

        cameraTransform.position = new Vector3(cameraTransform.position.x, cameraHeight, cameraTransform.position.z);

        cameraTransform.LookAt(targetTransform);
    }
}
