using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PM : MonoBehaviour
{
    public delegate void OnCollide(int value);
    public static event OnCollide OnPlayerCollide;

    public UnityEvent<int> OnPlayerCol;

    int hp = 100;
    public int HP
    {
        get => hp;
        set
        {
            hp = value;
            OnPlayerCol.Invoke(hp);
        }
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        transform.Translate(new Vector3(h, 0f, v) * Time.deltaTime * 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("BARREL"))
        {
            HP -= 10;
        }
    }
}
