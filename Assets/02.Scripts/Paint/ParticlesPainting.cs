using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesPainting : MonoBehaviour
{
    private Color paintColor;

    [Space]
    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

    private int baseColorID = Shader.PropertyToID("_BaseColor");

    [SerializeField] private float minRadius = 0.1f;
    [SerializeField] private float maxRadius = 0.3f;

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        paintColor = GetComponent<ParticleSystemRenderer>().material.color;
    }

    void OnParticleCollision(GameObject other)
    {
        if (part == null) return;
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Paintable p = other.GetComponent<Paintable>();

        if (p == null) return;

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;

            float radius = Random.Range(minRadius, maxRadius);
            GameManager.Instance.PaintManager.Paint(p, pos, radius, 1f, 1f, paintColor);
        }
    }
}