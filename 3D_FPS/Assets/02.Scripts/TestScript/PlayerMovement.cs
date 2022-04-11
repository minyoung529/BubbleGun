using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpPower = 10f;
    public float pushPower = 2f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            moveDirection = new Vector3(h, 0f, v);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= moveSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y += jumpPower;
            }
        }

        moveDirection += Physics.gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if ((controller.collisionFlags & CollisionFlags.Sides) != 0)
        {
            Debug.Log("Side");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
        if (rb == null || rb.isKinematic) return;

        Vector3 pushDir = new Vector3(hit.moveDirection.normalized.x, 0, hit.moveDirection.normalized.z);
        //Vector3 pushDir = new Vector3(moveDirection.normalized.x, 0, moveDirection.normalized.z);
        //Vector3 pushDir = hit.transform.position - transform.position;
        rb.velocity = pushDir * pushPower;
    }
}
