using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area
{
    public Transform areaTransform;

    // index: 0 => carrot
    // index: 1 => pepper
    public int[] monsterCount;

    [TextArea]
    public string infoMessage;
}
