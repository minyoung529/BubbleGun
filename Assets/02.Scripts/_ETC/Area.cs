using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Area
{
    public Transform areaTransform;

    public MonsterGenerate[] monsterGenerates;

    [TextArea]
    public string infoMessage;
}

[System.Serializable]
public struct MonsterGenerate
{
    public float time;
    public MonsterType monsterType;
    public int count;
}

public enum MonsterType
{
    Carrot,
    Pepper
}