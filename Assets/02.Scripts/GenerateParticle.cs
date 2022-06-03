using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class GenerateParticle : MonoBehaviour
{
    [SerializeField] private GameObject particle;
    [SerializeField] private bool isColor = false;
    [SerializeField] private bool isParent = false;

    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = Instantiate(particle);

        Vector3 pos = collision.GetContact(0).point;
        Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);

        obj.transform.SetPositionAndRotation(pos, rot);

        if (isColor)
        {
            Color myColor = GetComponent<Renderer>().materials[0].GetColor("_BaseColor");
            ParticleSystemRenderer[] objs = obj.GetComponentsInChildren<ParticleSystemRenderer>();

            foreach(ParticleSystemRenderer p in objs)
            {
                p.material.color = myColor;
            }
        }

        if(isParent)
        {
            obj.transform.SetParent(collision.transform);
        }
        Destroy(obj, 3f);
    }
}