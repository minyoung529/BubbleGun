using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    private float distance = 1000f;
    public Transform firePosition;

    void Update()
    {
        RaycastHit[] raycastHits;
        Debug.DrawRay(firePosition.position, firePosition.forward * distance, Color.red);

        if(Input.GetMouseButtonDown(0))
        {
            raycastHits = Physics.RaycastAll(firePosition.position, firePosition.forward, distance);

            foreach(RaycastHit info in raycastHits)
            {
                Debug.Log(info.collider.name);
            }
        }

        Move();
    }

    private void Move()
    {
        transform.Rotate(Vector3.up * Input.GetAxis("Horizontal"));
        transform.position += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * 3f;
    }
}
