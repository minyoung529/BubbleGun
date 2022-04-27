using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUN : MonoBehaviour
{
    public GameObject bulletPrefab;

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
        mousePos.z = 0f;

        Vector3 direction = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Debug.Log(angle);

        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}
