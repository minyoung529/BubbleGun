using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class GenerateParticle : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    [SerializeField] private bool isColor = false;
    [SerializeField] private bool isParent = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("PLAYER")) return;

        GameObject obj = PoolManager.Pop(particle);

        Vector3 pos = collision.GetContact(0).point;
        Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);

        obj.transform.SetPositionAndRotation(pos, rot);

        if (isColor)
        {
            Color myColor = GetComponent<Renderer>().material.GetColor("_BaseColor");
            ParticleSystemRenderer[] objs = obj.GetComponentsInChildren<ParticleSystemRenderer>();

            foreach(ParticleSystemRenderer p in objs)
                p.material.color = myColor;
        }

        if(isParent)
        {
            obj.transform.SetParent(collision.transform);
        }
        PoolManager.Push(obj, 3f);
    }
}