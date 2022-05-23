using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationSpeed = 80.0f;
    private Animation playerAnimation;
    private Transform playerTransform;

    private readonly float initHp = 10;
    public  float currentHp = 0;

    private IEnumerator Start()
    {
        playerTransform = GetComponent<Transform>();
        playerAnimation = GetComponent<Animation>();

        currentHp = initHp;

        playerAnimation.Play();

        rotationSpeed = 0f;
        yield return new WaitForSeconds(0.3f);
        rotationSpeed = 110.0f;
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PUNCH") && currentHp >= 0f)
        {
            currentHp -= 1f;

            if(currentHp <= 0.0f)
            {
                PlayerDie();
            }
        }
    }

    private void PlayerDie()
    {
        Debug.Log("DIE");
        GameObject[] monsters = GameObject.FindGameObjectsWithTag("MONSTER");

        foreach(GameObject obj in monsters)
        {
            obj.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        }

        GameManager.GetInstance().IsGameOver = true;
    }
}