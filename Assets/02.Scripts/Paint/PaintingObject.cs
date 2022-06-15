using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingObject : MonoBehaviour
{
    private Color color;
    private int baseColorID = Shader.PropertyToID("_BaseColor");

    private void Start()
    {
        color = GetComponent<Renderer>().materials[0].GetColor(baseColorID);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Paintable[] paintableObjects = collision.transform.GetComponentsInChildren<Paintable>();

        if (paintableObjects == null || paintableObjects.Length == 0) return;

        Vector3 point = collision.contacts[0].point;

        foreach (Paintable obj in paintableObjects)
        {
            GameManager.Instance.PaintManager.Paint(obj, point, Random.Range(1f, 3f), 1, 1, color);
        }
    }
}
