using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoolManager : MonoBehaviour
{
    private static Dictionary<string, Stack<GameObject>> pools = new Dictionary<string, Stack<GameObject>>();
    private static Transform poolTransform;

    private void Awake()
    {
        poolTransform = transform;
    }

    public static void Push(GameObject obj)
    {
        obj.name = obj.name.Trim();
        obj.transform.position = Vector3.one;

        if (!pools.ContainsKey(obj.name))
        {
            pools.Add(obj.name, new Stack<GameObject>());
        }

        obj.transform.SetParent(poolTransform);
        pools[obj.name].Push(obj);
        obj.SetActive(false);
    }

    public static void Push(GameObject obj, float delay)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.InsertCallback(delay, () =>
        {
            if (obj.activeSelf)
                Push(obj);
        });
    }

    public static GameObject Pop(GameObject item, bool isActive = true)
    {
        item.name = item.name.Trim();
        GameObject value = null;

        if (pools.ContainsKey(item.name))
        {
            if (pools[item.name].Count > 0)
                value = pools[item.name].Pop();
        }

        value ??= Instantiate(item, null);
        value.name = item.name;
        value.transform.SetParent(null);

        if (isActive)
            value.SetActive(isActive);
        return value;
    }

    public static GameObject Pop(GameObject item, Vector3 position, Quaternion rotation)
    {
        GameObject value = Pop(item, false);
        value.transform.SetPositionAndRotation(position, rotation);
        value.gameObject.SetActive(true);

        return value;
    }

    public static GameObject Pop(string item)
    {
        GameObject value = null;

        if (pools.ContainsKey(item))
        {
            if (pools[item].Count > 0)
                value = pools[item].Pop();
        }

        value ??= Instantiate(Resources.Load<GameObject>(item));
        value.name = item;
        value.SetActive(true);
        return value;
    }
}