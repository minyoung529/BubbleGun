using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    private Color paintColor;

    [Space]
    ParticleSystem part;
    List<ParticleCollisionEvent> collisionEvents;

    private int baseColorID = Shader.PropertyToID("_BaseColor");

    void Start()
    {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        paintColor = GetComponent<ParticleSystemRenderer>().material.GetColor(baseColorID);
    }

    void OnParticleCollision(GameObject other)
    {
        if (part == null) return;
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Debug.Log("Collide");
        Paintable p = other.GetComponent<Paintable>();

        if (p == null) return;

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;
            float radius = Random.Range(0.1f, 0.3f);
            GameManager.GetInstance().PaintManager.Paint(p, pos, radius, 1f, 1f, paintColor);
        }
    }
}