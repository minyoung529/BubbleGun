using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingObject : MonoBehaviour
{
    private Color color;
    private int baseColorID = Shader.PropertyToID("_BaseColor");
    new private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        color = renderer.material.GetColor(baseColorID);

        Paintable[] paintableObjects = collision.transform.GetComponentsInChildren<Paintable>();

        if (paintableObjects == null || paintableObjects.Length == 0) return;

        Vector3 point = collision.contacts[0].point;

        foreach (Paintable obj in paintableObjects)
        {
            GameManager.Instance.PaintManager.Paint(obj, point, Random.Range(0.5f, 0.9f), 1, 1, color);
        }
    }
}
