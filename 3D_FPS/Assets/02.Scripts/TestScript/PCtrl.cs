using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PCtrl : MonoBehaviour
{
    public UnityEvent onPlayerDead;

    public float moveSpeed = 2f;
    public float rotateSpeed = 100f;
    public float rotateBodySpeed = 2f;

    private Vector3 vecNowVelocity = Vector3.zero;
    private Vector3 vecNowDirection = Vector3.zero;

    private CharacterController playerController;
    private CollisionFlags playerCollisionFlag = CollisionFlags.None;

    void Start()
    {
        playerController = GetComponent<CharacterController>();
        playerCollisionFlag = playerController.collisionFlags;
    }

    void Update()
    {
        Move();
        DirectionChangeBody();
    }

    private void Move()
    {
        Transform cameraTransform = Camera.main.transform;
        Vector3 forward = cameraTransform.TransformDirection(Vector3.forward);
        Vector3 right = cameraTransform.TransformDirection(Vector3.right);

        forward.y = 0f;
        right.y = 0f;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 targetDirection = right * h + forward * v;
        vecNowDirection = Vector3.RotateTowards(vecNowDirection, targetDirection, rotateSpeed * Time.deltaTime, 1000f);
        vecNowDirection.Normalize();

        Vector3 moveAmount = vecNowDirection * moveSpeed * Time.deltaTime;

        if ((playerCollisionFlag & CollisionFlags.Below) == 0)
            moveAmount += Physics.gravity;
        else
            moveAmount.y = 0f;

        playerCollisionFlag = playerController.Move(moveAmount);
    }

    private void DirectionChangeBody()
    {
        if (GetNowVelocity() > 0f)
        {
            //ㅎ현재 캐릭터으 ㅣ이동 방향
            Vector3 newForward = playerController.velocity;
            newForward.y = 0f;

            transform.forward = Vector3.Lerp(transform.forward, newForward, rotateBodySpeed * Time.deltaTime);
        }
    }

    private float GetNowVelocity()
    {
        //if (playerController.velocity.sqrMagnitude > 0f)
        //{
        //    Vector3 velocity = playerController.velocity;
        //    velocity.y = 0f;

        //    vecNowVelocity = Vector3.Lerp(vecNowVelocity, velocity, Time.fixedDeltaTime);
        //}
        //else
        //{
        //    vecNowVelocity = Vector3.zero;
        //}

        //return vecNowVelocity.magnitude;
        return playerController.velocity.sqrMagnitude;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("DeadZone"))
        {
            onPlayerDead.Invoke();
            Destroy(gameObject);
        }
    }
}
