using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 80.0f;
    private Animation playerAnimation;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GetComponent<Transform>();
        playerAnimation = GetComponent<Animation>();

        playerAnimation.Play();
    }

    void Update()
    {
        MoveAndRotate();
    }

    private void MoveAndRotate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        float r = Input.GetAxisRaw("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);
        moveDir.Normalize();

        playerTransform.Translate(moveDir * moveSpeed * Time.deltaTime);
        playerTransform.Rotate(Vector3.up * r * rotationSpeed * Time.deltaTime);

        PlayerAnimation(h, v);
    }

    private void PlayerAnimation(float h, float v)
    {
        //cross fade => blending animation

        if (h <= -0.01f) // left
        {
            playerAnimation.CrossFade("RunL", 0.25f);
        }
        else if (h >= 0.01f) // Right
        {
            playerAnimation.CrossFade("RunR", 0.25f);
        }
        else if (v <= -0.01f) // Back
        {
            playerAnimation.CrossFade("RunB", 0.25f);
        }
        else if (v >= 0.01f) // Forward
        {
            playerAnimation.CrossFade("RunF", 0.25f);
        }
        else
        {
            playerAnimation.CrossFade("Idle", 0.25f);
        }
    }
}
