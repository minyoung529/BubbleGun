using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActEvent : MonoBehaviour
{
    public static void ActCoroutine(IEnumerator coroutine, float delay)
    {
        GameObject obj = new GameObject();
        obj.AddComponent<ActEvent>().StartCoroutine(coroutine);

        Destroy(obj, delay + 0.5f);
    }
}
